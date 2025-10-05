using RowadSystem.API.Helpers;
using RowadSystem.Entity;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Products;

namespace RowadSystem.Services;

public class InvoicesService(ApplicationDbContext context) : IInvoicesService
{
    private readonly ApplicationDbContext _context = context;
    public async Task<Result> AddSalesInvoiceAsync(string createByUserId, SalesInvoiceRequest request)
    {
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            Coupon? coupon = new();
            List<SalesInvoiceDetail> allDetails = new();
            var invoice = request.Adapt<SalesInvoice>();
            Order? order = new();
            string? userId = null;
            var invoiceNumber = await GenerateInvoiceNumberAsync();

            if (request.OrderId.HasValue && request.OrderId.Value > 0)
            {
                order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(c => c.Id == request.OrderId);

                if (order is null)
                    return Result.Failure(OrderErrors.OrderNotFound);

                if (!order.OrderItems.Any())
                    return Result.Failure(OrderErrors.OrderItemsNotFound);

                // نعمل نسخ مستقلة من OrderItems قبل إضافتها
                order.OrderItems.Select(x => new SalesInvoiceDetail
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    UnitId = x.UnitId,
                    Price = x.Price,
                    Total = x.Quantity * x.Price
                }).ToList().ForEach(x => allDetails.Add(new SalesInvoiceDetail
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    UnitId = x.UnitId,
                    Price = x.Price,
                    Total = x.Total
                }));

                userId = await _context.Orders
                    .Where(x => x.Id == request.OrderId.Value)
                    .Select(x => x.UserId)
                    .FirstOrDefaultAsync();

                if (userId is null)
                    return Result.Failure(OrderErrors.MustBeLoggedInToCreateOrder);
            }
            else if (request.CustomerId is null && userId is null)
                return Result.Failure(InvoiceErrors.InvalidPayer);

            var payments = request.Payments.Select(x => new Payment
            {
                Amount = x.Amount,
                Method = x.Method,
                UserId = userId,
                CustomerId = request.CustomerId
            }).ToList();

            invoice.Payments = payments;

            foreach (var p in payments)
            {
                if (userId is not null)
                    p.UserId = userId;

                if (request.CustomerId > 0)
                {
                    var customer = await _context.Customers.FindAsync(request.CustomerId);
                    if (customer is null)
                        return Result.Failure(CustomerErrors.CustomerNotFound);

                    p.CustomerId = request.CustomerId;
                }
            }

            // إنشاء نسخة مستقلة من items
            var detailsList = request.Items.Select(item => new SalesInvoiceDetail
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitId = item.UnitId,
                Price = item.Price,
                Total = item.Quantity * item.Price
            }).ToList();

            var productIds = detailsList.Select(x => x.ProductId).ToList();
            var products = await _context.Products
                .Where(x => productIds.Contains(x.Id))
                .Include(p => p.productUnits)
                .Include(p => p.Inventory)
                .ToListAsync();

            foreach (var item in detailsList)
            {
                var product = products.SingleOrDefault(x => x.Id == item.ProductId);

                if (product == null)
                    return Result.Failure(ProductErrors.ProductNotFound);

                if (product.productUnits is null || !product.productUnits.Any())
                    return Result.Failure(ProductErrors.ProductUnitsNotFound);

                var productUnits = product.productUnits.Where(p => p.UnitId == item.UnitId).ToList();

                if (!productUnits.Any())
                    return Result.Failure(ProductErrors.UnitNotFound);

                if (product.Inventory is null || product.Inventory.Quantity < item.Quantity || item.Quantity < 0)
                    return Result.Failure(ProductErrors.ProductQuantityNotAvailable);

                var price = product.productUnits.Where(p => p.UnitId == item.UnitId)
                                 .Select(p => p.SellingPrice).SingleOrDefault();

                product.Inventory.Quantity -= item.Quantity;
                allDetails.Add(new SalesInvoiceDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = price,
                    UnitId = item.UnitId,
                    Total = item.Quantity * price
                });
            }
            invoice.SalesInvoiceDetails = allDetails;

            if (request.CouponCode is not null)
            {
                var discountCoupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == request.CouponCode
                && c.IsActive && c.EndDate > DateTime.Now);

                if (discountCoupon is null)
                    return Result.Failure(CouponErrors.CouponInvalidOrExpired);

                if (discountCoupon.IsPercentage && (discountCoupon.DiscountValue < 0 || discountCoupon.DiscountValue > 100))
                    return Result.Failure(CouponErrors.CouponInvalidOrExpired);

                if (!discountCoupon.IsPercentage && discountCoupon.DiscountValue < 0)
                    return Result.Failure(CouponErrors.CouponInvalidOrExpired);

                var totalBeforeDiscount = allDetails.Sum(x => x.Price * x.Quantity);

                if (discountCoupon.IsPercentage)
                    coupon.DiscountValue = totalBeforeDiscount * (discountCoupon.DiscountValue / 100m);
                else
                    coupon.DiscountValue = discountCoupon.DiscountValue;

                invoice.CouponId = discountCoupon.Id;
            }

            invoice.TotalAmount = allDetails.Sum(x => x.Total);
            invoice.DiscountAmount = coupon?.DiscountValue ?? 0;
            invoice.NetAmount = allDetails.Sum(x => x.Price * x.Quantity) - (coupon?.DiscountValue ?? 0);

            var totalPaid = payments.Sum(p => p.Amount);

            if (totalPaid > invoice.NetAmount || totalPaid < 0)
                return Result.Failure<int>(InvoiceErrors.PaymentAmountExceedsInvoiceTotal);


            decimal remainingAmount = invoice.TotalAmount - totalPaid;
            if (remainingAmount == 0)
            {
                invoice.PaymentStatus = PaymentStatus.Paid;
            }
            else if (totalPaid < remainingAmount)
            {
                invoice.PaymentStatus = PaymentStatus.PartiallyPaid;
            }
            else
            {
                invoice.PaymentStatus = PaymentStatus.Deferred;
            }
            invoice.OrderId = request.OrderId;
            invoice.UserId = userId;
            invoice.CustomerId = request.CustomerId;
            invoice.Notes = request.Notes;
            invoice.InvoiceNumber = invoiceNumber;
            invoice.CreatedBy = createByUserId;
            invoice.TotalAmount = invoice.NetAmount;

            await _context.AddAsync(invoice);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return Result.Success();

        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


    //public async Task<Result> AddSalesInvoiceAsync(string createByUserId, SalesInvoiceRequest request)
    //{
    //    var transaction = await _context.Database.BeginTransactionAsync();

    //    try
    //    {
    //        Coupon? coupon = new();
    //        List<SalesInvoiceDetail> allDetails = [];
    //        var invoice = request.Adapt<SalesInvoice>();
    //        Order? order = new();
    //        string? userId = null;
    //        var invoiceNumber = await GenerateInvoiceNumberAsync();

    //        if (request.OrderId.HasValue && request.OrderId.Value > 0)
    //        {
    //            order = await _context.Orders
    //                .Include(o => o.OrderItems)
    //                .FirstOrDefaultAsync(c => c.Id == request.OrderId);

    //            if (order is null)
    //                return Result.Failure(OrderErrors.OrderNotFound);

    //            if (!order.OrderItems.Any())
    //                return Result.Failure(OrderErrors.OrderItemsNotFound);

    //            order.OrderItems.Select(x => new SalesInvoiceDetail
    //            {
    //                ProductId = x.ProductId,
    //                Quantity = x.Quantity,
    //                UnitId = x.UnitId,
    //                Price = x.Price,
    //                Total = x.Quantity * x.Price
    //            }).ToList().ForEach(x => allDetails.Add(x));


    //            userId = await _context.Orders
    //                .Where(x => x.Id == request.OrderId.Value)
    //                .Select(x => x.UserId)
    //                .FirstOrDefaultAsync();

    //            if (userId is null)
    //                return Result.Failure(OrderErrors.MustBeLoggedInToCreateOrder);

    //        }
    //        else if (request.CustomerId is null && userId is null)
    //            return Result.Failure(InvoiceErrors.InvalidPayer);


    //        var payments = request.Payments.Select(x => new Payment
    //        {
    //            Amount = x.Amount,
    //            Method = x.Method,
    //            UserId = userId,
    //            CustomerId = request.CustomerId
    //        }).ToList();

    //        invoice.Payments = payments;

    //        foreach (var p in payments)
    //        {
    //            if (userId is not null)
    //                p.UserId = userId;

    //            if (request.CustomerId > 0)
    //            {
    //                var customer = await _context.Customers.FindAsync(request.CustomerId);
    //                if (customer is null)
    //                    return Result.Failure(CustomerErrors.CustomerNotFound);

    //                p.CustomerId = request.CustomerId;
    //            }
    //        }

    //        var detailsList = request.Items.Adapt<List<SalesInvoiceDetail>>();

    //        var productIds = detailsList.Select(x => x.ProductId).ToList();
    //        var products = await _context.Products
    //            .Where(x => productIds.Contains(x.Id))
    //            .Include(p => p.productUnits)
    //            .Include(p => p.Inventory)
    //            .ToListAsync();


    //        foreach (var item in detailsList)
    //        {
    //            var product = products.SingleOrDefault(x => x.Id == item.ProductId);

    //            if (product == null)
    //                return Result.Failure(ProductErrors.ProductNotFound);

    //            if (product.productUnits is null || !product.productUnits.Any())
    //                return Result.Failure(ProductErrors.ProductUnitsNotFound);


    //            var productUnits = product.productUnits.Where(p => p.UnitId == item.UnitId).ToList();

    //            if (!productUnits.Any())
    //                return Result.Failure(ProductErrors.UnitNotFound);

    //            if (product.Inventory is null || product.Inventory.Quantity < item.Quantity || item.Quantity < 0)
    //                return Result.Failure(ProductErrors.ProductQuantityNotAvailable);

    //            var price = product.productUnits.Where(p => p.UnitId == item.UnitId)
    //                              .Select(p => p.SellingPrice).SingleOrDefault();

    //            product.Inventory.Quantity -= item.Quantity;
    //            allDetails.Add(new SalesInvoiceDetail
    //            {
    //                ProductId = item.ProductId,
    //                Quantity = item.Quantity,
    //                Price = price,
    //                UnitId = item.UnitId,
    //                Total = item.Quantity * price

    //            });
    //        }
    //        invoice.SalesInvoiceDetails = allDetails;

    //        if (request.CouponCode is not null)
    //        {
    //            var discountCoupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == request.CouponCode
    //            && c.IsActive && c.EndDate > DateTime.UtcNow);

    //            if (discountCoupon is null)
    //                return Result.Failure(CouponErrors.CouponInvalidOrExpired);

    //            if (discountCoupon.IsPercentage && (discountCoupon.DiscountValue < 0 || discountCoupon.DiscountValue > 100))
    //                return Result.Failure(CouponErrors.CouponInvalidOrExpired);

    //            if (!discountCoupon.IsPercentage && discountCoupon.DiscountValue < 0)
    //                return Result.Failure(CouponErrors.CouponInvalidOrExpired);

    //            var totalBeforeDiscount = allDetails.Sum(x => x.Price * x.Quantity);

    //            if (discountCoupon.IsPercentage)
    //                coupon.DiscountValue = totalBeforeDiscount * (discountCoupon.DiscountValue / 100m);
    //            else
    //                coupon.DiscountValue = discountCoupon.DiscountValue;

    //            invoice.CouponId = discountCoupon.Id;
    //        }

    //        invoice.TotalAmount = allDetails.Sum(x => x.Total);
    //        invoice.DiscountAmount = coupon?.DiscountValue ?? 0;
    //        invoice.NetAmount = allDetails.Sum(x => x.Price * x.Quantity) - (coupon?.DiscountValue ?? 0);

    //        var totalPaid = payments.Sum(p => p.Amount);

    //        if (totalPaid > invoice.NetAmount)
    //            return Result.Failure<int>(InvoiceErrors.PaymentAmountExceedsInvoiceTotal);

    //        if (totalPaid == invoice.NetAmount)
    //            invoice.IsPaid = true;



    //        invoice.OrderId = request.OrderId;
    //        invoice.UserId = userId;
    //        invoice.CustomerId = request.CustomerId;
    //        invoice.Notes = request.Notes;
    //        invoice.InvoiceNumber = invoiceNumber;
    //        invoice.CreatedBy = createByUserId;
    //        invoice.TotalAmount = invoice.NetAmount;

    //        await _context.AddAsync(invoice);
    //        await _context.SaveChangesAsync();
    //        await transaction.CommitAsync();
    //        return Result.Success();

    //    }
    //    catch (Exception)
    //    {
    //        await transaction.RollbackAsync();
    //        throw;
    //    }
    //}
    public async Task<Result> AddSalesReturnInvoiceAsync(string createByUserId, SalesReturnInvoiceRequest request)
    {

        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var salesInvoice = await _context.SalesInvoices
            .Include(x => x.SalesInvoiceDetails)
            .FirstOrDefaultAsync(x => x.Id == request.SalesInvoiceId);

            if (salesInvoice is null)
                return Result.Failure(InvoiceErrors.InvoiceNotFound);

            if (salesInvoice.SalesInvoiceDetails is null || !salesInvoice.SalesInvoiceDetails.Any())
                return Result.Failure(ProductErrors.NoProductsInInvoice);

            var previousReturns = await _context.SalesReturnInvoices
                .Where(x => x.SalesInvoiceId == request.SalesInvoiceId)
                .SelectMany(x => x.salesReturnInvoiceDetails)
                .GroupBy(x => new { x.ProductId, x.UnitId })
                .Select(g => new
                {
                    g.Key.ProductId,
                    g.Key.UnitId,
                    ReturnedQuantity = g.Sum(x => x.Quantity)
                })
                .ToListAsync();

            foreach (var returnItem in request.Items)
            {
                var invoiceItem = salesInvoice.SalesInvoiceDetails
                    .FirstOrDefault(x => x.ProductId == returnItem.ProductId && x.UnitId == returnItem.UnitId);

                if (invoiceItem is null)
                    return Result.Failure(ProductErrors.ProductNotFoundInInvoice);

                var alreadyReturned = previousReturns
                    .FirstOrDefault(x => x.ProductId == returnItem.ProductId && x.UnitId == returnItem.UnitId)?.ReturnedQuantity ?? 0;

                var remaining = invoiceItem.Quantity - alreadyReturned;

                if (returnItem.Quantity > remaining)
                    return Result.Failure(ProductErrors.ExceedsRemainingQuantityToReturn);
            }

            if ((request.UserId is null && request.CustomerId is null)
                || (request.UserId is not null && request.CustomerId is not null))
                return Result.Failure(InvoiceErrors.InvalidPayer);


            List<SalesReturnInvoiceDetail> allDetails = [];


            var invoiceNumber = await GenerateInvoiceNumberAsync();


            //  var payments = request.Adapt<List<Payment>>();
            var payments = request.Payments.Select(x => new Payment
            {
                Amount = x.Amount,
                Method = x.Method,
                UserId = request.UserId,
                CustomerId = request.CustomerId
            }).ToList();


            if ((salesInvoice.UserId is null && salesInvoice.CustomerId is null)
                 || (salesInvoice.UserId is not null && salesInvoice.CustomerId is not null))
                return Result.Failure(InvoiceErrors.InvalidPayer);

            foreach (var payment in payments)
            {
                if (salesInvoice.UserId is not null)
                    payment.UserId = salesInvoice.UserId;
                else
                    payment.CustomerId = salesInvoice.CustomerId;

            }



            var detailsList = request.Items.Adapt<List<SalesReturnInvoiceDetail>>();

            var productIds = detailsList.Select(x => x.ProductId).ToList();
            var products = await _context.Products
               .Where(x => productIds.Contains(x.Id))
               .Include(p => p.productUnits)
               .Include(p => p.Inventory)
               .ToListAsync();

            if (products.Count != productIds.Count)
                return Result.Failure(OrderErrors.SomeProductsNotFound);

            foreach (var item in detailsList)
            {

                var product = products.SingleOrDefault(x => x.Id == item.ProductId);

                if (product == null)
                    return Result.Failure(ProductErrors.ProductNotFound);

                if (product.productUnits is null || !product.productUnits.Any())
                    return Result.Failure(ProductErrors.ProductUnitsNotFound);


                var productUnits = product.productUnits.Where(p => p.UnitId == item.UnitId).ToList();

                if (!productUnits.Any())
                    return Result.Failure(ProductErrors.UnitNotFound);

                if (product.Inventory is null || product.Inventory.Quantity < item.Quantity || item.Quantity < 0)
                    return Result.Failure(ProductErrors.ProductQuantityNotAvailable);

                var price = product.productUnits.Where(p => p.UnitId == item.UnitId)
                                  .Select(p => p.SellingPrice).SingleOrDefault();
                product.Inventory.Quantity += item.Quantity;

                allDetails.Add(new SalesReturnInvoiceDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = price,
                    UnitId = item.UnitId,
                    Total = item.Quantity * price

                });

            }

            var invoice = new SalesReturnInvoice
            {
                UserId = request.UserId,
                CustomerId = request.CustomerId,
                Notes = request.Notes,
                Payments = payments,
                TotalAmount = allDetails.Sum(x => x.Total),
                salesReturnInvoiceDetails = allDetails,
                InvoiceNumber = invoiceNumber,
                SalesInvoiceId = request.SalesInvoiceId,
                CreatedBy = createByUserId
            };


            var totalPaid = payments.Sum(p => p.Amount);

            if (totalPaid > invoice.TotalAmount || totalPaid < 0)
                return Result.Failure<int>(InvoiceErrors.PaymentAmountExceedsInvoiceTotal);


            decimal remainingAmount = invoice.TotalAmount - totalPaid;
            if (remainingAmount == 0)
            {
                invoice.PaymentStatus = PaymentStatus.Paid;
            }
            else if (totalPaid < remainingAmount)
            {
                invoice.PaymentStatus = PaymentStatus.PartiallyPaid;
            }
            else
            {
                invoice.PaymentStatus = PaymentStatus.Deferred;
            }

            await _context.AddAsync(invoice);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return Result.Success();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task<Result> AddPurchaseInvoiceAsync(string createByUserId, PurchaseInvoiceRequest request)
    {
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var invoiceNumber = await GenerateInvoiceNumberAsync();
            List<PurchaseInvoiceDetail> allDetails = [];
            //  var payments = request.Adapt<List<Payment>>();
            var payments = request.Payments.Select(x => new Payment
            {
                UserId = createByUserId,
                Amount = x.Amount,
                Method = x.Method,
            }).ToList();


            var detailsList = request.Items.Adapt<List<PurchaseInvoiceDetail>>();

            var productIds = detailsList.Select(x => x.ProductId).ToList();
            var products = await _context.Products
               .Where(x => productIds.Contains(x.Id))
               .Include(p => p.productUnits)
               .Include(p => p.Inventory)
               .ToListAsync();

            if (products.Count != productIds.Count)
                return Result.Failure(OrderErrors.SomeProductsNotFound);

            foreach (var item in detailsList)
            {

                var product = products.SingleOrDefault(x => x.Id == item.ProductId);

                if (product == null)
                    return Result.Failure(ProductErrors.ProductNotFound);

                if (product.productUnits is null || !product.productUnits.Any())
                    return Result.Failure(ProductErrors.ProductUnitsNotFound);


                var productUnits = product.productUnits.Where(p => p.UnitId == item.UnitId).ToList();

                if (!productUnits.Any())
                    return Result.Failure(ProductErrors.UnitNotFound);


                var price = product.productUnits.Where(p => p.UnitId == item.UnitId)
                                  .Select(p => p.SellingPrice).SingleOrDefault();

                product.Inventory.Quantity += item.Quantity;

                allDetails.Add(new PurchaseInvoiceDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    UnitId = item.UnitId,
                    Total = item.Quantity * item.Price
                });

            }
            //var detailsList = request.Items.Select(x => new PurchaseInvoiceDetail
            //{
            //    ProductId = x.ProductId,
            //    Quantity = x.Quantity,
            //    UnitId = x.UnitId,


            //}).ToList();
            //foreach (var item in detailsList)
            //{
            //    var details = await _context.Products.Where(x => x.Id == item.ProductId)
            //   .Select(x => new PurchaseInvoiceDetail
            //   {
            //       ProductId = x.Id,
            //       Quantity = item.Quantity,
            //       Price = x.PurchasePrice,
            //       UnitId = item.UnitId,
            //       Total = item.Quantity * x.PurchasePrice
            //   }).SingleOrDefaultAsync();


            //    allDetails.Add(details);

            //}

            var invoice = new PurchaseInvoice
            {
                SupplierId = request.SupplierId,
                Notes = request.Notes,
                Payments = payments,
                TotalAmount = allDetails.Sum(x => x.Total),
                purchaseInvoiceDetails = allDetails,
                InvoiceNumber = invoiceNumber,
                CreatedBy = createByUserId
            };
            var totalPaid = payments.Sum(p => p.Amount);

            if (totalPaid > invoice.TotalAmount || totalPaid < 0)
                return Result.Failure<int>(InvoiceErrors.PaymentAmountExceedsInvoiceTotal);


            decimal remainingAmount = invoice.TotalAmount - totalPaid;
            if (remainingAmount == 0)
            {
                invoice.PaymentStatus = PaymentStatus.Paid;
            }
            else if (totalPaid < remainingAmount)
            {
                invoice.PaymentStatus = PaymentStatus.PartiallyPaid;
            }
            else
            {
                invoice.PaymentStatus = PaymentStatus.Deferred;
            }

            await _context.AddAsync(invoice);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Result.Success();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task<Result> AddPurchaseReturnInvoiceAsync(string createByUserId, PurchaseReturnInvoiceRequest request)
    {
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var purchaseInvoice = await _context.PurchaseInvoices
            .Include(x => x.purchaseInvoiceDetails)
            .FirstOrDefaultAsync(x => x.Id == request.PurchaseInvoiceId);

            if (purchaseInvoice is null)
                return Result.Failure(InvoiceErrors.InvoiceNotFound);

            var previousReturns = await _context.PurchaseReturns
                .Where(x => x.PurchaseInvoiceId == request.PurchaseInvoiceId)
                .SelectMany(x => x.PurchaseReturnDetails)
                .GroupBy(x => new { x.ProductId, x.UnitId })
                .Select(g => new
                {
                    g.Key.ProductId,
                    g.Key.UnitId,
                    ReturnedQuantity = g.Sum(x => x.Quantity)
                })
                .ToListAsync();

            if (purchaseInvoice.purchaseInvoiceDetails is null || !purchaseInvoice.purchaseInvoiceDetails.Any())
                return Result.Failure(ProductErrors.NoProductsInInvoice);

            foreach (var returnItem in request.Items)
            {
                var invoiceItem = purchaseInvoice.purchaseInvoiceDetails
                    .FirstOrDefault(x => x.ProductId == returnItem.ProductId && x.UnitId == returnItem.UnitId);

                if (invoiceItem is null)
                    return Result.Failure(ProductErrors.ProductNotFoundInInvoice);

                var alreadyReturned = previousReturns
                    .FirstOrDefault(x => x.ProductId == returnItem.ProductId && x.UnitId == returnItem.UnitId)?.ReturnedQuantity ?? 0;

                var remaining = invoiceItem.Quantity - alreadyReturned;

                if (returnItem.Quantity > remaining)
                    return Result.Failure(ProductErrors.ExceedsRemainingQuantityToReturn);
            }

            var invoiceNumber = await GenerateInvoiceNumberAsync();
            List<PurchaseReturnDetail> allDetails = [];

            var payments = request.Payments.Select(x => new Payment
            {
                UserId = createByUserId,
                Amount = x.Amount,
                Method = x.Method
            }).ToList();

            var detailsList = request.Items.Adapt<List<PurchaseReturnDetail>>();
            var productIds = detailsList.Select(x => x.ProductId).ToList();
            var products = await _context.Products
               .Where(x => productIds.Contains(x.Id))
               .Include(p => p.productUnits)
               .Include(p => p.Inventory)
               .ToListAsync();

            if (products.Count != productIds.Count)
                return Result.Failure(OrderErrors.SomeProductsNotFound);

            foreach (var item in detailsList)
            {

                var product = products.SingleOrDefault(x => x.Id == item.ProductId);

                if (product == null)
                    return Result.Failure(ProductErrors.ProductNotFound);

                if (product.productUnits is null || !product.productUnits.Any())
                    return Result.Failure(ProductErrors.ProductUnitsNotFound);


                var productUnits = product.productUnits.Where(p => p.UnitId == item.UnitId).ToList();

                if (!productUnits.Any())
                    return Result.Failure(ProductErrors.UnitNotFound);

                if (product.Inventory is null || product.Inventory.Quantity < item.Quantity || item.Quantity < 0)
                    return Result.Failure(ProductErrors.ProductQuantityNotAvailable);

                var price = product.productUnits.Where(p => p.UnitId == item.UnitId)
                                  .Select(p => p.SellingPrice).SingleOrDefault();

                product.Inventory.Quantity -= item.Quantity;

                allDetails.Add(new PurchaseReturnDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    UnitId = item.UnitId,
                    Total = item.Quantity * item.Price
                });

            }

            var invoice = new PurchaseReturnInvoice
            {
                SupplierId = request.SupplierId,
                Notes = request.Notes,
                Payments = payments,
                TotalAmount = allDetails.Sum(x => x.Total),
                PurchaseReturnDetails = allDetails,
                PurchaseInvoiceId = request.PurchaseInvoiceId,
                InvoiceNumber = invoiceNumber,
                CreatedBy = createByUserId
            };

            var totalPaid = payments.Sum(p => p.Amount);


            decimal remainingAmount = invoice.TotalAmount - totalPaid;
            if (remainingAmount == 0)
            {
                invoice.PaymentStatus = PaymentStatus.Paid;
            }
            else if (totalPaid < remainingAmount)
            {
                invoice.PaymentStatus = PaymentStatus.PartiallyPaid;
            }
            else
            {
                invoice.PaymentStatus = PaymentStatus.Deferred;
            }

            //if (totalPaid == invoice.TotalAmount)
            //    invoice.IsPaid = true;
            await _context.AddAsync(invoice);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Result.Success();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    private async Task<string> GenerateInvoiceNumberAsync()
    {
        int nextId = await _context.Invoices.MaxAsync(x => (int?)x.Id) ?? 0;
        string invoiceNumber = $"INV-{(nextId + 1).ToString("D4")}";
        return invoiceNumber;
    }

    //public async Task<Result<SalesInvoiceResponse>> GetSalesInvoicesByInvoiceNumber(string invoiceNumber)
    //{
    //    var result = await _context.SalesInvoices
    //        .Where(x => x.InvoiceNumber == invoiceNumber)
    //        .Include(x => x.Customer)
    //        .Include(x => x.User)
    //        .Include(x => x.SalesInvoiceDetails)
    //        .ThenInclude(x => x.Product)
    //        .FirstOrDefaultAsync();

    //    if (result == null)
    //        return Result.Failure<SalesInvoiceResponse>(InvoiceErrors.InvoiceNotFound);

    //    var invoice = result;


    //    var invoiceResponse = new SalesInvoiceResponse
    //    {
    //        SalesInvoiceId = invoice.Id,

    //        InvoiceDate = invoice.InvoiceDate,
    //        CustomerId = invoice.CustomerId,
    //        CustomerName = invoice.Customer.Name,
    //        UserId = invoice.UserId ?? null,
    //        UserName = invoice.User is { FirstName: not null, LastName: not null } u
    //            ? $"{u.FirstName} {u.LastName}"
    //            : null,

    //        InvoiceNumber = invoice.InvoiceNumber,

    //        Payments = invoice.Payments.Select(payment => new PaymentResponse
    //        {
    //            Method = payment.Method,
    //            Amount = payment.Amount,
    //            PaymentDate = payment.PaymentDate,
    //            PaymentStatus = invoice.IsPaid
    //        }).ToList(),

    //    };

    //    List<InvoiceItemResponse> invoiceItem = new List<InvoiceItemResponse>();
    //    foreach (var item in result.SalesInvoiceDetails)
    //    {
    //        var detail = invoice.SalesInvoiceDetails.Select(item => new InvoiceItemResponse
    //        {
    //            Barcode = item.Product?.Barcode!,
    //            ProductId = item.ProductId,
    //            ProductName = item.Product!.Name,
    //            productUnits = item.Product.productUnits.Select(x => new ProductUnitDto
    //            {
    //                Id = x.ProductId,
    //                Price = x.SellingPrice,
    //                NumberOfUnits = x.QuantityInBaseUnit
    //            }).ToList(),
    //            UnitId = item.UnitId,
    //            UnitPrice = item.Price,
    //            Quantity = item.Quantity,
    //            TotalPrice = item.Quantity * item.Price
    //        }).FirstOrDefault();

    //        invoiceItem.Add(detail);
    //    }

    //    invoiceResponse.Items = invoiceItem;



    //    return Result.Success(invoiceResponse);

    //}
    public async Task<Result<SalesInvoiceResponse>> GetSalesInvoicesByInvoiceNumber(string invoiceNumber)
    {
        var result = await _context.SalesInvoices
            .Where(x => x.InvoiceNumber == invoiceNumber)
            .Include(x => x.Customer)
            .Include(x => x.User)
            .Include(x => x.SalesInvoiceDetails)
                .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync();

        if (result == null)
            return Result.Failure<SalesInvoiceResponse>(InvoiceErrors.InvoiceNotFound);

        var invoice = result;

        var invoiceResponse = new SalesInvoiceResponse
        {
            SalesInvoiceId = invoice.Id,
            InvoiceDate = invoice.InvoiceDate,
            CustomerId = invoice.CustomerId,
            CustomerName = invoice.Customer.Name,
            UserId = invoice.UserId ?? null,
            UserName = invoice.User is { FirstName: not null, LastName: not null } u
                ? $"{u.FirstName} {u.LastName}"
                : null,
            InvoiceNumber = invoice.InvoiceNumber,
            Payments = invoice.Payments.Select(payment => new PaymentResponse
            {
                Method = payment.Method,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
            }).ToList(),
            //PaymentStatus = invoice.PaymentStatus
        };

        var invoiceItem = invoice.SalesInvoiceDetails.Select(item => new InvoiceItemResponse
        {
            Barcode = item.Product?.Barcode!,
            ProductId = item.ProductId,
            ProductName = item.Product!.Name,
            productUnits = item.Product.productUnits.Select(x => new ProductUnitDto
            {
                Id = x.UnitId,
                Price = x.SellingPrice,
                NumberOfUnits = x.QuantityInBaseUnit
            }).FirstOrDefault()!,
            UnitId = item.UnitId,
            UnitPrice = item.Price,
            Quantity = item.Quantity,
            TotalPrice = item.Quantity * item.Price
        }).ToList();

        invoiceResponse.Items = invoiceItem;

        return Result.Success(invoiceResponse);
    }

    public async Task<Result<PurchaseInvoiceResponse>> GetPurchaseInvoicesByInvoiceNumber(string invoiceNumber)
    {
        var result = await _context.PurchaseInvoices
            .Where(x => x.InvoiceNumber == invoiceNumber)
            .Include(x => x.Supplier)
            .Include(x => x.purchaseInvoiceDetails)
            .ThenInclude(x => x.Product)
            .ThenInclude(x => x!.productUnits)
            .ThenInclude(x => x.Unit)

            .FirstOrDefaultAsync();

        if (result == null)
            return Result.Failure<PurchaseInvoiceResponse>(InvoiceErrors.InvoiceNotFound);

        var invoice = result;


        var invoiceResponse = new PurchaseInvoiceResponse
        {
            PurchaseInvoiceId = invoice.Id,
            InvoiceDate = invoice.InvoiceDate,
            SupplierId = invoice.SupplierId,
            SupplierName = invoice.Supplier.Name,
            InvoiceNumber = invoice.InvoiceNumber,


            Payments = invoice.Payments.Select(payment => new PaymentResponse
            {
                Method = payment.Method,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                //PaymentStatus = invoice.PaymentStatus
            }).ToList(),

        };

        var invoiceItem = invoice.purchaseInvoiceDetails.Select(item => new InvoiceItemResponse
        {
            Barcode = item.Product?.Barcode!,
            ProductId = item.ProductId,
            ProductName = item.Product!.Name,
            productUnits = item.Product.productUnits.Select(x => new ProductUnitDto
            {
                Id = x.UnitId,
                Price = x.SellingPrice,
                NumberOfUnits = x.QuantityInBaseUnit
            }).FirstOrDefault()!,
            UnitId = item.UnitId,
            UnitPrice = item.Price,
            UnitName = item.Unit.Name,
            Quantity = item.Quantity,
            TotalPrice = item.Quantity * item.Price
        }).ToList();

        //List<InvoiceItemResponse> invoiceItem = new List<InvoiceItemResponse>();
        //foreach (var item in result.purchaseInvoiceDetails)
        //{
        //    var detail = invoice.purchaseInvoiceDetails.Select(item => new InvoiceItemResponse
        //    {
        //        Barcode = item.Product?.Barcode!,
        //        ProductId = item.ProductId,
        //        ProductName = item.Product!.Name,
        //        productUnits = item.Product.productUnits.Select(x => new ProductUnitDto
        //        {
        //            Id = x.ProductId,
        //            Price = item.Price,
        //            NumberOfUnits = x.QuantityInBaseUnit
        //        }).ToList(),
        //        UnitId = item.UnitId,
        //        UnitPrice = item.Price,
        //        Quantity = item.Quantity,
        //        TotalPrice = item.Quantity * item.Price
        //    }).FirstOrDefault();

        //    invoiceItem.Add(detail);
        //}

        invoiceResponse.Items = invoiceItem;



        return Result.Success(invoiceResponse);

    }
    public async Task<Result<SalesReturnInvoiceResponse>> GetSalesReturnInvoicesByInvoiceNumber(string invoiceNumber)
    {
        var result = await _context.SalesReturnInvoices
            .Where(x => x.InvoiceNumber == invoiceNumber)
            .Include(x => x.Customer)
            .Include(x => x.User)
            .Include(x => x.salesReturnInvoiceDetails)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync();

        if (result == null)
            return Result.Failure<SalesReturnInvoiceResponse>(InvoiceErrors.InvoiceNotFound);

        var invoice = result;


        var invoiceResponse = new SalesReturnInvoiceResponse
        {
            SalesReturnInvoiceId = invoice.Id,

            InvoiceDate = invoice.InvoiceDate,
            CustomerId = invoice.CustomerId,
            CustomerName = invoice.Customer?.Name,
            UserId = invoice.UserId ?? null,
            UserName = invoice.User is { FirstName: not null, LastName: not null } u
                ? $"{u.FirstName} {u.LastName}"
                : null,




            InvoiceNumber = invoice.InvoiceNumber,

            Payments = invoice.Payments.Select(payment => new PaymentResponse
            {
                Method = payment.Method,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
            }).ToList(),
            //PaymentStatus = invoice.PaymentStatus

        };
        var invoiceItem = invoice.salesReturnInvoiceDetails.Select(item => new InvoiceItemResponse
        {
            Barcode = item.Product?.Barcode!,
            ProductId = item.ProductId,
            ProductName = item.Product!.Name,
            productUnits = item.Product.productUnits.Select(x => new ProductUnitDto
            {
                Id = x.UnitId,
                Price = x.SellingPrice,
                NumberOfUnits = x.QuantityInBaseUnit
            }).FirstOrDefault()!,
            UnitId = item.UnitId,
            UnitPrice = item.Price,
            Quantity = item.Quantity,
            TotalPrice = item.Quantity * item.Price
        }).ToList();

        //List<InvoiceItemResponse> invoiceItem = new List<InvoiceItemResponse>();
        //foreach (var item in result.salesReturnInvoiceDetails)
        //{
        //    var detail = invoice.salesReturnInvoiceDetails.Select(item => new InvoiceItemResponse
        //    {
        //        Barcode = item.Product?.Barcode!,
        //        ProductId = item.ProductId,
        //        ProductName = item.Product!.Name,
        //        productUnits = item.Product.productUnits.Select(x => new ProductUnitDto
        //        {
        //            Id = x.ProductId,
        //            Price = x.SellingPrice,
        //            NumberOfUnits = x.QuantityInBaseUnit
        //        }).ToList(),
        //        UnitId = item.UnitId,
        //        UnitPrice = item.Price,
        //        Quantity = item.Quantity,
        //        TotalPrice = item.Quantity * item.Price
        //    }).FirstOrDefault();

        //    invoiceItem.Add(detail);
        //}

        invoiceResponse.Items = invoiceItem;



        return Result.Success(invoiceResponse);

    }
    public async Task<Result<PurchaseReturnInvoiceResponse>> GetPurchaseReturnInvoicesByInvoiceNumber(string invoiceNumber)
    {
        var result = await _context.PurchaseReturns
            .Where(x => x.InvoiceNumber == invoiceNumber)
            .Include(x => x.Supplier)
            .Include(x => x.PurchaseReturnDetails)
            .ThenInclude(x => x.Product)
            .ThenInclude(x => x!.productUnits)
            .ThenInclude(x => x.Unit)
            .FirstOrDefaultAsync();

        if (result == null)
            return Result.Failure<PurchaseReturnInvoiceResponse>(InvoiceErrors.InvoiceNotFound);

        var invoice = result;


        var invoiceResponse = new PurchaseReturnInvoiceResponse
        {
            PurchaseReturnInvoiceId = invoice.Id,
            InvoiceDate = invoice.InvoiceDate,
            SupplierId = invoice.SupplierId,
            SupplierName = invoice.Supplier.Name,
            InvoiceNumber = invoice.InvoiceNumber,


            Payments = invoice.Payments.Select(payment => new PaymentResponse
            {
                Method = payment.Method,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                //PaymentStatus = invoice.PaymentStatus
            }).ToList(),

        };
        var invoiceItem = invoice.PurchaseReturnDetails.Select(item => new InvoiceItemResponse
        {
            Barcode = item.Product?.Barcode!,
            ProductId = item.ProductId,
            ProductName = item.Product!.Name,
            productUnits = item.Product.productUnits.Select(x => new ProductUnitDto
            {
                Id = x.UnitId,
                Price = x.SellingPrice,
                NumberOfUnits = x.QuantityInBaseUnit,
                Name = x.Unit?.Name ?? "No Unit"

            }).FirstOrDefault()!,
            UnitId = item.UnitId,
            UnitPrice = item.Price,
            Quantity = item.Quantity,
            UnitName = item.Unit?.Name ?? "",
            TotalPrice = item.Quantity * item.Price
        }).ToList();
        //List<InvoiceItemResponse> invoiceItem = new List<InvoiceItemResponse>();
        //foreach (var item in result.PurchaseReturnDetails)
        //{
        //    var detail = invoice.PurchaseReturnDetails.Select(item => new InvoiceItemResponse
        //    {
        //        Barcode = item.Product?.Barcode!,
        //        ProductId = item.ProductId,
        //        ProductName = item.Product!.Name,
        //        productUnits = item.Product.productUnits.Select(x => new ProductUnitDto
        //        {
        //            Id = x.ProductId,
        //            Price = item.Price,
        //            NumberOfUnits = x.QuantityInBaseUnit
        //        }).ToList(),
        //        UnitId = item.UnitId,
        //        UnitPrice = item.Price,
        //        Quantity = item.Quantity,
        //        TotalPrice = item.Quantity * item.Price
        //    }).FirstOrDefault();

        //    invoiceItem.Add(detail);
        //}

        invoiceResponse.Items = invoiceItem;



        return Result.Success(invoiceResponse);

    }

    public async Task<Result<PaginatedListResponse<InvoiceResponse>>> GetAllSalesInvoices(RequestFilters requestFilters)
    {
        var invoice = _context.SalesInvoices
            .Include(x => x.Customer)
            .Include(x => x.SalesInvoiceDetails)
            .OrderByDescending(x => x.InvoiceDate)
            .Select(x => new InvoiceResponse
            {
                Id = x.Id,
                InvoiceNumber = x.InvoiceNumber,
                InvoiceDate = x.InvoiceDate,
                Notes = x.Notes,
                Items = x.SalesInvoiceDetails.Select(d => new InvoiceDetails
                {
                    ProductName = d.Product.Name,
                    Quantity = d.Quantity,
                    Total = d.Total
                }).ToList(),
                //Payments = x.Payments.Select(payment => new PaymentResponse
                //{
                //    Method = payment.Method,
                //    Amount = payment.Amount,
                //    PaymentDate = payment.PaymentDate,
                //}).ToList() 
                PaymentStatus = x.PaymentStatus

            });


        var response = await PaginatedList<InvoiceResponse>.CreatePaginationAsync(invoice, requestFilters.PageNumber, requestFilters.PageSize);
        if (!response.Items.Any())
            return Result.Failure<PaginatedListResponse<InvoiceResponse>>(InvoiceErrors.InvoiceNotFound);

        return Result.Success(response.Adapt<PaginatedListResponse<InvoiceResponse>>());
    }
    public async Task<Result<PaginatedListResponse<InvoiceResponse>>> GetAllSalesReturnInvoices(RequestFilters requestFilters)
    {
        var invoice = _context.SalesReturnInvoices
            .Include(x => x.Customer)
            .Include(x => x.salesReturnInvoiceDetails)
            .OrderByDescending(x => x.InvoiceDate)
            .Select(x => new InvoiceResponse
            {
                Id = x.Id,
                InvoiceNumber = x.InvoiceNumber,
                InvoiceDate = x.InvoiceDate,
                Notes = x.Notes,
                Items = x.salesReturnInvoiceDetails.Select(d => new InvoiceDetails
                {
                    ProductName = d.Product.Name,
                    Quantity = d.Quantity,
                    Total = d.Total
                }).ToList(),
                //Payments = x.Payments.Select(payment => new PaymentResponse
                //{
                //    Method = payment.Method,
                //    Amount = payment.Amount,
                //    PaymentDate = payment.PaymentDate,
                //}).ToList()
                PaymentStatus = x.PaymentStatus

            });


        var response = await PaginatedList<InvoiceResponse>.CreatePaginationAsync(invoice, requestFilters.PageNumber, requestFilters.PageSize);
        if (!response.Items.Any())
            return Result.Failure<PaginatedListResponse<InvoiceResponse>>(InvoiceErrors.InvoiceNotFound);

        return Result.Success(response.Adapt<PaginatedListResponse<InvoiceResponse>>());
    }
    public async Task<Result<PaginatedListResponse<InvoiceResponse>>> GetAllPurchaseInvoices(RequestFilters requestFilters)
    {
        var invoice = _context.PurchaseInvoices
            .Include(x => x.Supplier)
            .Include(x => x.purchaseInvoiceDetails)
            .OrderByDescending(x => x.InvoiceDate)
            .Select(x => new InvoiceResponse
            {
                Id = x.Id,
                InvoiceNumber = x.InvoiceNumber,
                InvoiceDate = x.InvoiceDate,
                Notes = x.Notes,
                Items = x.purchaseInvoiceDetails.Select(d => new InvoiceDetails
                {
                    ProductName = d.Product.Name,
                    Quantity = d.Quantity,
                    Total = d.Total
                }).ToList(),
                //Payments = x.Payments.Select(payment => new PaymentResponse
                //{
                //    Method = payment.Method,
                //    Amount = payment.Amount,
                //    PaymentDate = payment.PaymentDate,
                //}).ToList()
                PaymentStatus = x.PaymentStatus

            });


        var response = await PaginatedList<InvoiceResponse>.CreatePaginationAsync(invoice, requestFilters.PageNumber, requestFilters.PageSize);
        if (!response.Items.Any())
            return Result.Failure<PaginatedListResponse<InvoiceResponse>>(InvoiceErrors.InvoiceNotFound);

        return Result.Success(response.Adapt<PaginatedListResponse<InvoiceResponse>>());
    }
    public async Task<Result<PaginatedListResponse<InvoiceResponse>>> GetAllPurchaseReturnInvoices(RequestFilters requestFilters)
    {
        var invoice = _context.PurchaseReturns
            .Include(x => x.Supplier)
            .Include(x => x.PurchaseReturnDetails)
            .OrderByDescending(x => x.InvoiceDate)
            .Select(x => new InvoiceResponse
            {
                Id = x.Id,
                InvoiceNumber = x.InvoiceNumber,
                InvoiceDate = x.InvoiceDate,
                Notes = x.Notes,
                Items = x.PurchaseReturnDetails.Select(d => new InvoiceDetails
                {
                    ProductName = d.Product.Name,
                    Quantity = d.Quantity,
                    Total = d.Total
                }).ToList(),
                //Payments = x.Payments.Select(payment => new PaymentResponse
                //{
                //    Method = payment.Method,
                //    Amount = payment.Amount,
                //    PaymentDate = payment.PaymentDate,
                //}).ToList(),
                PaymentStatus = x.PaymentStatus

            });


        var response = await PaginatedList<InvoiceResponse>.CreatePaginationAsync(invoice, requestFilters.PageNumber, requestFilters.PageSize);
        if (!response.Items.Any())
            return Result.Failure<PaginatedListResponse<InvoiceResponse>>(InvoiceErrors.InvoiceNotFound);

        return Result.Success(response.Adapt<PaginatedListResponse<InvoiceResponse>>());
    }

    public async Task<Result> AddPaymentToInvoiceAsync(int invoiceId, PaymentRequest paymentRequest)
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var invoice = await _context.SalesInvoices
                .Include(i => i.Payments)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice == null)
                return Result.Failure(InvoiceErrors.InvoiceNotFound);
           

            var existingPayment = invoice.Payments
                .FirstOrDefault(p => p.Method == paymentRequest.Method && p.CustomerId == invoice.CustomerId);

            var totalPaid = invoice.Payments.Sum(p => p.Amount);
            var total = totalPaid + paymentRequest.Amount;

            if (invoice.NetAmount < total || paymentRequest.Amount <= 0)
                return Result.Failure<int>(InvoiceErrors.PaymentAmountExceedsInvoiceTotal);

            if (existingPayment != null)
            {
                existingPayment.Amount += paymentRequest.Amount;
            }
            else
            {
                var payment = new Payment
                {
                    Amount = paymentRequest.Amount,
                    Method = paymentRequest.Method,
                    CustomerId = invoice.CustomerId,
                    UserId = invoice.UserId
                };
                invoice.Payments.Add(payment);
            }



            if (totalPaid >= invoice.NetAmount)
            {
                invoice.PaymentStatus = PaymentStatus.Paid;
            }
            else if (totalPaid > 0)
            {
                invoice.PaymentStatus = PaymentStatus.PartiallyPaid;
            }
            else
            {
                invoice.PaymentStatus = PaymentStatus.Deferred;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Result.Failure(InvoiceErrors.InvalidPaymentMethod);
        }
    }

}
