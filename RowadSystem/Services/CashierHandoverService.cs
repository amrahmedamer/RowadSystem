using RowadSystem.API.Entity;
using RowadSystem.API.Errors;

namespace RowadSystem.API.Services;

public class CashierHandoverService(ApplicationDbContext context) : ICashierHandoverService
{
    private readonly ApplicationDbContext _context = context;


    //public async Task<Result> HandoverCashierAsync(string cashierId, decimal amountInDrawer, DateTime? startDate = null, DateTime? endDate = null, bool isLate = false)
    //{
    //    startDate ??= DateTime.Today.Date;  
    //    endDate ??= DateTime.Today.Date.AddDays(1).AddTicks(-1);


    //    var totalSalesAmount = await _context.SalesInvoices
    //        .Where(i => i.CreatedBy == cashierId 
    //        && (i.CreatedAt.Date >= startDate  && i.CreatedAt.Date <= endDate)
    //        && ( i.PaymentStatus == PaymentStatus.Paid || i.PaymentStatus == PaymentStatus.PartiallyPaid))
    //        .SumAsync(i => i.TotalAmount );


    //    var totalReturnAmount = await _context.SalesReturnInvoices
    //         .Where(i => i.CreatedBy == cashierId
    //        && (i.CreatedAt.Date >= startDate && i.CreatedAt.Date <= endDate)
    //        && (i.PaymentStatus == PaymentStatus.Paid || i.PaymentStatus == PaymentStatus.PartiallyPaid))
    //        .SumAsync(i => i.TotalAmount);

    //    var totalPurchaseAmount = await _context.PurchaseInvoices
    //         .Where(i => i.CreatedBy == cashierId
    //        && (i.CreatedAt.Date >= startDate && i.CreatedAt.Date <= endDate)
    //        && (i.PaymentStatus == PaymentStatus.Paid || i.PaymentStatus == PaymentStatus.PartiallyPaid))
    //        .SumAsync(i => i.TotalAmount);

    //    var totalPurchaseReturnAmount = await _context.PurchaseReturns
    //     .Where(i => i.CreatedBy == cashierId
    //        && (i.CreatedAt.Date >= startDate && i.CreatedAt.Date <= endDate)
    //        && (i.PaymentStatus == PaymentStatus.Paid || i.PaymentStatus == PaymentStatus.PartiallyPaid))
    //        .SumAsync(i => i.TotalAmount);

    //    var totalAmount = totalSalesAmount - totalReturnAmount + totalPurchaseAmount - totalPurchaseReturnAmount;
    //    var difference = amountInDrawer - totalAmount;

    //    string notes = difference == 0 ? "تسليم صحيح" : "يوجد عجز أو فائض";

    //    if (isLate)
    //    {
    //        notes += " (تسليم متأخر)";
    //    }

    //    if (difference == 0)
    //    {
    //        Result.Failure(CashierHandoverErrors.CashierHandoverWithDifference);
    //    }

    //    var handover = new CashierHandover
    //    {
    //        CashierId = cashierId,
    //        AmountInDrawer = amountInDrawer,
    //        TotalSalesAmount = totalSalesAmount,
    //        TotalReturnAmount = totalReturnAmount,
    //        Difference = difference,
    //        HandoverDate = DateTime.Now,
    //        IsLateHandover = isLate,
    //        HandoverStartDate = startDate,
    //        HandoverEndDate = endDate,
    //        Notes = notes
    //    };

    //    _context.CashierHandovers.Add(handover);
    //    await _context.SaveChangesAsync();

    //    return Result.Success();  
    //}
    public async Task<Result> HandoverCashierAsync(string cashierId, decimal amountInDrawer)
    {
        DateTime startDate;
        DateTime endDate = DateTime.Today.AddDays(1).AddTicks(-1);

        var lastHandover = await _context.CashierHandovers
            .Where(h => h.CashierId == cashierId )
            .OrderByDescending(h => h.HandoverDate)
            .FirstOrDefaultAsync();

        if (lastHandover != null)
            startDate = lastHandover.HandoverDate;
        else
            startDate = DateTime.Today;

        var existingHandover = await _context.CashierHandovers
            .Where(h => h.CashierId == cashierId && h.HandoverStartDate >= startDate && h.HandoverEndDate <= endDate)
            .FirstOrDefaultAsync();

        if (existingHandover != null)
        {
            return Result.Failure(CashierHandoverErrors.DuplicateHandoverInSameDay);
        }


        var totalSalesAmount = await _context.SalesInvoices
            .Where(i => i.CreatedBy == cashierId
            && (i.CreatedAt >= startDate && i.CreatedAt <= endDate)
            && (i.PaymentStatus == PaymentStatus.Paid || i.PaymentStatus == PaymentStatus.PartiallyPaid))
            .SumAsync(i => i.NetAmount);


        var totalReturnAmount = await _context.SalesReturnInvoices
             .Where(i => i.CreatedBy == cashierId
            && (i.CreatedAt >= startDate && i.CreatedAt <= endDate)
            && (i.PaymentStatus == PaymentStatus.Paid || i.PaymentStatus == PaymentStatus.PartiallyPaid))
            .SumAsync(i => i.TotalAmount);

        var totalPurchaseAmount = await _context.PurchaseInvoices
             .Where(i => i.CreatedBy == cashierId
            && (i.CreatedAt >= startDate && i.CreatedAt <= endDate)
            && (i.PaymentStatus == PaymentStatus.Paid || i.PaymentStatus == PaymentStatus.PartiallyPaid))
            .SumAsync(i => i.TotalAmount);

        var totalPurchaseReturnAmount = await _context.PurchaseReturns
         .Where(i => i.CreatedBy == cashierId
            && (i.CreatedAt  >= startDate && i.CreatedAt <= endDate)
            && (i.PaymentStatus == PaymentStatus.Paid || i.PaymentStatus == PaymentStatus.PartiallyPaid))
            .SumAsync(i => i.TotalAmount);

        var totalAmount = totalSalesAmount - totalReturnAmount + totalPurchaseAmount - totalPurchaseReturnAmount;
        var difference = amountInDrawer - totalAmount;

        string notes = difference == 0 ? "تسليم صحيح" : "يوجد عجز أو فائض";

        if (difference != 0)
        {
          return  Result.Failure(CashierHandoverErrors.CashierHandoverWithDifference);
        }

        var handover = new CashierHandover
        {
            CashierId = cashierId,
            AmountInDrawer = amountInDrawer,
            TotalSalesAmount = totalSalesAmount,
            TotalReturnAmount = totalReturnAmount,
            Difference = difference,
            HandoverDate = DateTime.Today.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute),
            HandoverStartDate = startDate,
            HandoverEndDate = endDate,
            Notes = notes
        };

        _context.CashierHandovers.Add(handover);
        await _context.SaveChangesAsync();

        return Result.Success();
    }
}
