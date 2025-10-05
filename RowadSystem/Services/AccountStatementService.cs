using Microsoft.EntityFrameworkCore;
using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Reports;
using System;
using System.Linq;
using InvoiceDetail = RowadSystem.Shard.Contract.Reports.InvoiceDetail;

namespace RowadSystem.API.Services;

public class AccountStatementService(ApplicationDbContext context) : IAccountStatementService
{
    private readonly ApplicationDbContext _context = context;

    //public async Task<Result<InvoiceSummary>> GetDailySalesAsync()
    //{
    //    var today = DateTime.Today;

    //    var invoices = await _dbContext.SalesInvoices
    //        .Where(i => i.InvoiceDate.Date == today)
    //        .Include(i => i.Payments)
    //        .ToListAsync();

    //    if (invoices is null)
    //        return Result.Failure<InvoiceSummary>(InvoiceErrors.InvoiceNotFound);

    //    var result = new InvoiceSummary
    //    {
    //        InvoiceCount = invoices.Count,
    //        TotalAmount = invoices.Sum(i => i.TotalAmount),
    //        TotalPaid = invoices.Sum(i => i.Payments.Sum(p => p.Amount))
    //    };

    //    return Result.Success(result);
    //}


    //public async Task<Result<InvoiceSummary>> GetDailyPurchaseAsync()
    //{
    //    var today = DateTime.Today;

    //    var invoices = await _dbContext.PurchaseInvoices
    //        .Where(i => i.InvoiceDate.Date == today)
    //        .Include(i => i.Payments)
    //        .ToListAsync();

    //    if (invoices is null)
    //        return Result.Failure<InvoiceSummary>(InvoiceErrors.InvoiceNotFound);

    //    var result = new InvoiceSummary
    //    {
    //        InvoiceCount = invoices.Count,
    //        TotalAmount = invoices.Sum(i => i.TotalAmount),
    //        TotalPaid = invoices.Sum(i => i.Payments.Sum(p => p.Amount))
    //    };

    //    return Result.Success(result);
    //}


    //public async Task<Result<InvoiceSummary>> GetDailySalesReturnAsync()
    //{
    //    var today = DateTime.Today;

    //    var invoices = await _dbContext.SalesReturnInvoices
    //        .Where(i => i.InvoiceDate.Date == today)
    //        .Include(i => i.Payments)
    //        .ToListAsync();

    //    if (invoices is null)
    //        return Result.Failure<InvoiceSummary>(InvoiceErrors.InvoiceNotFound);

    //    var result = new InvoiceSummary
    //    {
    //        InvoiceCount = invoices.Count,
    //        TotalAmount = invoices.Sum(i => i.TotalAmount),
    //        TotalPaid = invoices.Sum(i => i.Payments.Sum(p => p.Amount))
    //    };

    //    return Result.Success(result);
    //}

    //public async Task<Result<InvoiceSummary>> GetDailyPurchaseReturnAsync()
    //{
    //    var today = DateTime.Today;

    //    var invoices = await _dbContext.PurchaseReturns
    //        .Where(i => i.InvoiceDate.Date == today)
    //        .Include(i => i.Payments)
    //        .ToListAsync();

    //    if (invoices is null)
    //        return Result.Failure<InvoiceSummary>(InvoiceErrors.InvoiceNotFound);

    //    var result = new InvoiceSummary
    //    {
    //        InvoiceCount = invoices.Count,
    //        TotalAmount = invoices.Sum(i => i.TotalAmount),
    //        TotalPaid = invoices.Sum(i => i.Payments.Sum(p => p.Amount))
    //    };

    //    return Result.Success(result);
    //}


    //public async Task<Result<InvoiceSummary>> GetSalesSummary()
    //{
    //    var invoices = await _context.SalesInvoices
    //        .Include(i => i.Payments)
    //        .ToListAsync();

    //    var summary = new InvoiceSummary
    //    {
    //        InvoiceCount = invoices.Count,
    //        TotalAmount = invoices.Sum(i => i.TotalAmount),
    //        TotalPaid = invoices.Sum(i => i.Payments.Sum(p => p.Amount)),
    //    };

    //    return Result.Success(summary);
    //}

    //public async Task<Result<List<InvoiceDetail>>> GetSalesDetails()
    //{
    //    var invoices = await _context.SalesInvoices
    //        .Include(i => i.SalesInvoiceDetails)
    //        .Include(i => i.Customer)
    //        .Include(i => i.Payments)
    //        .ToListAsync();

    //    var details = invoices.Select(i => new InvoiceDetail
    //    {
    //        InvoiceNumber = i.InvoiceNumber,
    //        CustomerName = i.Customer?.Name??"",
    //        Date = i.InvoiceDate,
    //        TotalAmount = i.TotalAmount,
    //        Paid = i.Payments.Sum(p => p.Amount),
    //        Due = i.TotalAmount - i.Payments.Sum(p => p.Amount),
    //        Items = i.SalesInvoiceDetails.Select(it => new InvoiceItem
    //        {
    //            ProductName = it.Product?.Name??"",
    //            Quantity = it.Quantity,
    //            TotalPrice = it.Price
    //        }).ToList()
    //    }).ToList();

    //    return Result.Success(details);
    //}

    //// نفس الفكرة لباقي الأنواع:
    //public async Task<Result<InvoiceSummary>> GetPurchaseSummary()
    //    {
    //        var invoices = await _context.PurchaseInvoices.ToListAsync();
    //        var summary = new InvoiceSummary
    //        {
    //            InvoiceCount = invoices.Count,
    //            TotalAmount = invoices.Sum(x => x.TotalAmount),
    //            TotalPaid = invoices.Sum(i => i.Payments.Sum(p => p.Amount)),
    //        };
    //        return Result.Success(summary);
    //    }

    //    public async Task<Result<List<InvoiceDetail>>> GetPurchaseDetails()
    //    {
    //        var invoices = await _context.PurchaseInvoices
    //            .Include(i => i.purchaseInvoiceDetails)
    //            .Include(i => i.Supplier)
    //            .ToListAsync();

    //        var details = invoices.Select(i => new InvoiceDetail
    //        {
    //            InvoiceNumber = i.InvoiceNumber,
    //            CustomerName = i.Supplier?.Name??"",
    //            Date = i.InvoiceDate,
    //            TotalAmount = i.TotalAmount,
    //            Paid = i.Payments.Sum(p => p.Amount),
    //            Due = i.TotalAmount - i.Payments.Sum(p => p.Amount),
    //            Items = i.purchaseInvoiceDetails.Select(it => new InvoiceItem
    //            {
    //                ProductName = it.Product?.Name??"",
    //                Quantity = it.Quantity,
    //                TotalPrice = it.Price
    //            }).ToList()
    //        }).ToList();

    //        return Result.Success(details);
    //    }

    //    public async Task<Result<InvoiceSummary>> GetSalesReturnSummary()
    //    {
    //        var invoices = await _context.SalesReturnInvoices.ToListAsync();
    //        var summary = new InvoiceSummary
    //        {
    //            InvoiceCount = invoices.Count,
    //            TotalAmount = invoices.Sum(x => x.TotalAmount),
    //            TotalPaid = invoices.Sum(i => i.Payments.Sum(p => p.Amount)),

    //        };
    //        return Result.Success(summary);
    //    }

    //    public async Task<Result<List<Shard.Contract.Reports.InvoiceDetail>>> GetSalesReturnDetails()
    //    {
    //        var invoices = await _context.SalesReturnInvoices
    //            .Include(i => i.salesReturnInvoiceDetails)
    //            .Include(i => i.Customer)
    //            .ToListAsync();

    //        var details = invoices.Select(i => new InvoiceDetail
    //        {
    //            InvoiceNumber = i.InvoiceNumber,
    //            CustomerName = i.Customer?.Name ?? "",
    //            Date = i.InvoiceDate,
    //            TotalAmount = i.TotalAmount,
    //            Paid = i.Payments.Sum(p => p.Amount),
    //            Due = i.TotalAmount - i.Payments.Sum(p => p.Amount),
    //            Items = i.salesReturnInvoiceDetails.Select(it => new InvoiceItem
    //            {
    //                ProductName = it.Product?.Name ?? "",
    //                Quantity = it.Quantity,
    //                TotalPrice = it.Price
    //            }).ToList()
    //        }).ToList();

    //        return Result.Success(details);
    //    }

    //    public async Task<Result<InvoiceSummary>> GetPurchaseReturnSummary()
    //    {
    //        var invoices = await _context.PurchaseReturns.ToListAsync();
    //        var summary = new InvoiceSummary
    //        {
    //            InvoiceCount = invoices.Count,
    //            TotalAmount = invoices.Sum(x => x.TotalAmount),
    //            TotalPaid = invoices.Sum(i => i.Payments.Sum(p => p.Amount))
    //        };
    //        return Result.Success(summary);
    //    }

    //    public async Task<Result<List<InvoiceDetail>>> GetPurchaseReturnDetails()
    //    {
    //        var invoices = await _context.PurchaseReturns
    //            .Include(i => i.PurchaseReturnDetails)
    //            .Include(i => i.Supplier)
    //            .ToListAsync();

    //        var details = invoices.Select(i => new InvoiceDetail
    //        {
    //            InvoiceNumber = i.InvoiceNumber,
    //            CustomerName = i.Supplier?.Name ?? "",
    //            Date = i.InvoiceDate,
    //            TotalAmount = i.TotalAmount,
    //            Paid = i.Payments.Sum(p => p.Amount),
    //            Due = i.TotalAmount - i.Payments.Sum(p => p.Amount),
    //            Items = i.PurchaseReturnDetails.Select(it => new InvoiceItem
    //            {
    //                ProductName = it.Product?.Name ?? "",
    //                Quantity = it.Quantity,
    //                TotalPrice = it.Price
    //            }).ToList()
    //        }).ToList();

    //        return Result.Success(details);
    //    }



    public async Task<Result<InvoiceSummary>> GetSalesSummary()
    {
        var today = DateTime.Today;
        var invoices = await _context.SalesInvoices
            .Where(i => i.InvoiceDate.Date == today) 
            .Include(i => i.Payments)
            .ToListAsync();

        var summary = new InvoiceSummary
        {
            InvoiceCount = invoices.Count,
            TotalAmount = invoices.Sum(i => i.NetAmount),
            TotalPaid = invoices.Sum(i => i.Payments.Sum(p => p.Amount)),
        };

        return Result.Success(summary);
    }

    public async Task<Result<PaginatedListResponse<InvoiceDetail>>> GetSalesDetails(RequestFilters requestFilters)
    {
        var today = DateTime.Today;

        var query = _context.SalesInvoices
            .Where(i => i.InvoiceDate.Date == today) 
            .Include(i => i.SalesInvoiceDetails)
            .Include(i => i.Customer)
            .Include(i => i.Payments)
            .AsQueryable();

        var source = query.Select(i => new InvoiceDetail
        {
            InvoiceNumber = i.InvoiceNumber,
            CustomerName = i.Customer.Name,
            Date = i.InvoiceDate,
            TotalAmount = i.NetAmount,
            Paid = i.Payments.Sum(p => p.Amount),
            Due = i.NetAmount - i.Payments.Sum(p => p.Amount)
            //Items = i.SalesInvoiceDetails.Select(it => new InvoiceItem
            //{
            //    ProductName = it.Product.Name,
            //    Quantity = it.Quantity,
            //    TotalPrice = it.Price
            //}).ToList()
        });
        var paginatedInvoices = await PaginatedList<InvoiceDetail>.CreatePaginationAsync(source, requestFilters.PageNumber, requestFilters.PageSize);

        if (paginatedInvoices is null)
            Result.Failure<PaginatedListResponse<InvoiceDetail>>(InvoiceErrors.InvoiceNotFound);

        return Result.Success(paginatedInvoices.Adapt<PaginatedListResponse<InvoiceDetail>>());
    }

    //public async Task<Result<List<InvoiceDetail>>> GetSalesDetails()
    //{
    //    var today = DateTime.Today;
    //    var invoices = await _context.SalesInvoices
    //        .Where(i => i.InvoiceDate.Date == today) // فلترة حسب تاريخ اليوم
    //        .Include(i => i.SalesInvoiceDetails)
    //        .Include(i => i.Customer)
    //        .Include(i => i.Payments)
    //        .ToListAsync();

    //    var details = invoices.Select(i => new InvoiceDetail
    //    {
    //        InvoiceNumber = i.InvoiceNumber,
    //        CustomerName = i.Customer?.Name ?? "",
    //        Date = i.InvoiceDate,
    //        TotalAmount = i.TotalAmount,
    //        Paid = i.Payments.Sum(p => p.Amount),
    //        Due = i.TotalAmount - i.Payments.Sum(p => p.Amount),
    //        Items = i.SalesInvoiceDetails.Select(it => new InvoiceItem
    //        {
    //            ProductName = it.Product?.Name ?? "",
    //            Quantity = it.Quantity,
    //            TotalPrice = it.Price
    //        }).ToList()
    //    }).ToList();

    //    return Result.Success(details);
    //}

    // نفس الفكرة لباقي الأنواع:

    public async Task<Result<InvoiceSummary>> GetPurchaseSummary()
    {
        var today = DateTime.Today;
        var invoices = await _context.PurchaseInvoices
            .Where(i => i.InvoiceDate.Date == today)
            .Include(i => i.Payments)
            .ToListAsync();

        var summary = new InvoiceSummary
        {
            InvoiceCount = invoices.Count,
            TotalAmount = invoices.Sum(x => x.TotalAmount),
            TotalPaid = invoices.Sum(i => i.Payments.Sum(p => p.Amount)),
        };

        return Result.Success(summary);
    }

    public async Task<Result<PaginatedListResponse<InvoiceDetail>>> GetPurchaseDetails(RequestFilters requestFilters)
    {
        var today = DateTime.Today;
        var invoices = _context.PurchaseInvoices
            .Where(i => i.InvoiceDate.Date == today)
            .Include(i => i.purchaseInvoiceDetails)
            .Include(i => i.Supplier)
            .AsQueryable();

        var source = invoices.Select(i => new InvoiceDetail
        {
            InvoiceNumber = i.InvoiceNumber,
            CustomerName = i.Supplier.Name ,
            Date = i.InvoiceDate,
            TotalAmount = i.TotalAmount,
            Paid = i.Payments.Sum(p => p.Amount),
            Due = i.TotalAmount - i.Payments.Sum(p => p.Amount)
        });

        var paginatedInvoices = await PaginatedList<InvoiceDetail>.CreatePaginationAsync(source, requestFilters.PageNumber, requestFilters.PageSize);

        if (paginatedInvoices is null)
            Result.Failure<PaginatedListResponse<InvoiceDetail>>(InvoiceErrors.InvoiceNotFound);

        return Result.Success(paginatedInvoices.Adapt<PaginatedListResponse<InvoiceDetail>>());
    }

    public async Task<Result<InvoiceSummary>> GetSalesReturnSummary()
    {
        var today = DateTime.Today;
        var invoices = await _context.SalesReturnInvoices
            .Where(i => i.InvoiceDate.Date == today)
            .Include(i => i.Payments)
            .ToListAsync();

        var summary = new InvoiceSummary
        {
            InvoiceCount = invoices.Count,
            TotalAmount = invoices.Sum(x => x.TotalAmount),
            TotalPaid = invoices.Sum(i => i.Payments.Sum(p => p.Amount)),
        };

        return Result.Success(summary);
    }

    public async Task<Result<PaginatedListResponse<InvoiceDetail>>> GetSalesReturnDetails(RequestFilters requestFilters)
    {
        var today = DateTime.Today;
        var invoices =  _context.SalesReturnInvoices
            .Where(i => i.InvoiceDate.Date == today) 
            .Include(i => i.salesReturnInvoiceDetails)
            .Include(i => i.Customer)
             .AsQueryable();

        var source = invoices.Select(i => new InvoiceDetail
        {
            InvoiceNumber = i.InvoiceNumber,
            CustomerName = i.Customer.Name ,
            Date = i.InvoiceDate,
            TotalAmount = i.TotalAmount,
            Paid = i.Payments.Sum(p => p.Amount),
            Due = i.TotalAmount - i.Payments.Sum(p => p.Amount)
        });
        var paginatedInvoices = await PaginatedList<InvoiceDetail>.CreatePaginationAsync(source, requestFilters.PageNumber, requestFilters.PageSize);

        if (paginatedInvoices is null)
            Result.Failure<PaginatedListResponse<InvoiceDetail>>(InvoiceErrors.InvoiceNotFound);

        return Result.Success(paginatedInvoices.Adapt<PaginatedListResponse<InvoiceDetail>>());
    }

    public async Task<Result<InvoiceSummary>> GetPurchaseReturnSummary()
    {
        var today = DateTime.Today;
        var invoices = await _context.PurchaseReturns
            .Where(i => i.InvoiceDate.Date == today)
            .Include(i => i.Payments)
            .ToListAsync();

        var summary = new InvoiceSummary
        {
            InvoiceCount = invoices.Count,
            TotalAmount = invoices.Sum(x => x.TotalAmount),
            TotalPaid = invoices.Sum(i => i.Payments.Sum(p => p.Amount)),
        };

        return Result.Success(summary);
    }

    public async Task<Result<PaginatedListResponse<InvoiceDetail>>> GetPurchaseReturnDetails(RequestFilters requestFilters)
    {
        var today = DateTime.Today;
        var invoices =  _context.PurchaseReturns
            .Where(i => i.InvoiceDate.Date == today)
            .Include(i => i.PurchaseReturnDetails)
            .Include(i => i.Supplier)
               .AsQueryable();
        var source = invoices.Select(i => new InvoiceDetail
        {
            InvoiceNumber = i.InvoiceNumber,
            CustomerName = i.Supplier.Name,
            Date = i.InvoiceDate,
            TotalAmount = i.TotalAmount,
            Paid = i.Payments.Sum(p => p.Amount),
            Due = i.TotalAmount - i.Payments.Sum(p => p.Amount)
        });

        var paginatedInvoices = await PaginatedList<InvoiceDetail>.CreatePaginationAsync(source, requestFilters.PageNumber, requestFilters.PageSize);

        if (paginatedInvoices is null)
            Result.Failure<PaginatedListResponse<InvoiceDetail>>(InvoiceErrors.InvoiceNotFound);

        return Result.Success(paginatedInvoices.Adapt<PaginatedListResponse<InvoiceDetail>>());
    }


}