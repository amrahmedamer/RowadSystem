using Microsoft.AspNetCore.Mvc;
using RowadSystem.Shard.Contract.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RowadSystem.API.Services;

public class ReportService(ApplicationDbContext context) : IReportService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<SalesSummaryDto>> SalesPerDay()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        var dailySales = await _context.SalesInvoices
            .Where(x => x.InvoiceDate >= today && x.InvoiceDate < tomorrow)
            .GroupBy(x => today)
            .Select(g => new SalesSummaryDto
            {
                Date = today,
                TotalSales = g.Sum(x => x.TotalAmount),
                TotalInvoices = g.Count()
            })
            .FirstOrDefaultAsync();


        if (dailySales == null)
            return Result.Failure<SalesSummaryDto>(InvoiceErrors.InvoiceNotFound);

        return Result.Success(dailySales);

    }

    public async Task<Result<SalesSummaryDto>> SalesPerMonth()
    {

        var today = DateTime.Now;
      

        var currentMonth = await _context.SalesInvoices
            .Where(i => i.InvoiceDate.Year == today.Year && i.InvoiceDate.Month == today.Month)
            .GroupBy(i => new { i.InvoiceDate.Year, i.InvoiceDate.Month })
            .Select(g => new SalesSummaryDto
            {
                Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                TotalSales = g.Sum(e => e.TotalAmount),
                TotalInvoices = g.Count()
            })
            .FirstOrDefaultAsync();

        if (currentMonth == null)
            return Result.Failure<SalesSummaryDto>(InvoiceErrors.InvoiceNotFound);


        return Result.Success(currentMonth);
    }
    public async Task<Result<List<TopProductDto>>> GetTopSellingProducts()
    {
        var topProducts = await _context.SalesInvoiceDetails
            .Include(ii => ii.Product)
            .GroupBy(ii => ii.Product.Name)
            .Select(g => new TopProductDto
            {
                ProductName = g.Key,
                TotalQuantity = g.Sum(x => x.Quantity),
                TotalSales = g.Sum(x => x.Quantity * x.Price)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .Take(10)
            .ToListAsync();

        if (topProducts is null)
            return Result.Failure<List<TopProductDto>>(ProductErrors.ProductNotFound);

        return Result.Success(topProducts);
    }
    public async Task<Result<List<LowStockProductDto>>> GetLowStockProductsAsync(int threshold = 10)
    {
        var lowStockProducts = await _context.Products
            .Include(p => p.Inventory)
            .Where(p => p.Inventory != null && p.Inventory.Quantity <= threshold)
            .Select(p => new LowStockProductDto
            {
                ProductId = p.Id,
                Name = p.Name,
                Quantity = p.Inventory.Quantity
            })
            .OrderBy(p => p.Quantity) 
            .ToListAsync();

        return Result.Success(lowStockProducts);
    }

    public async Task<Result<List<SalesSummary>>> GetDailySalesAsync(RangDateDto rangDateDto)
    {
      var  dailySales= await _context.SalesInvoices
            .Where(x => x.InvoiceDate.Date >= rangDateDto.StartDate.Date && x.InvoiceDate.Date <= rangDateDto.EndDate.Date)
            .GroupBy(x => x.InvoiceDate.Date)
            .Select(g => new SalesSummary
            {
                Date = g.Key,
                TotalSales = g.Sum(x => x.TotalAmount)
            })
            .OrderBy(x => x.Date)
            .ToListAsync();

            if (!dailySales.Any())
                return Result.Failure<List<SalesSummary>>(InvoiceErrors.InvoiceNotFound);


        return Result.Success(dailySales);
    }

    public async Task<Result<CustomersDashboardDto>> GetCustomersDashboard()
    {
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);

        var result= new CustomersDashboardDto
        {
            TotalCustomers =await _context.Customers.CountAsync(),
            NewCustomersThisMonth = await _context.Customers.CountAsync(c => c.CreatedAt >= startOfMonth),
            ActiveCustomers = await _context.Customers.CountAsync(c => !c.IsDeleted),
            InActiveCustomers = await _context.Customers .CountAsync(c => c.IsDeleted),
            LatestCustomers = await _context.Customers
                .OrderByDescending(c => c.CreatedAt)
                .Take(5)
                .Select(c => new CustomerSummaryDto
                {
                    CustomerId = c.Id,
                    FullName = c.Name,
                    Email = c.Email,
                    RegisterDate = c.CreatedAt,
                    Status = c.IsDeleted ? "Inactive" : "Active"
                })
                .ToListAsync()
        };

        if (result is null)
            return Result.Failure< CustomersDashboardDto>(CustomerErrors.CustomerNotFound);

        return Result.Success(result);
    }
    public async Task<Result<OrdersDashboardDto>> GetOrdersDashboard()
    {
        var totalOrders = await _context.Orders.CountAsync();
        var pending = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Pending);
        var completed = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Delivered);
        var cancelled = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Canceled);

        var latestOrders = await _context.Orders
            .Include(x=>x.User)
            .OrderByDescending(o => o.OrderDate)
            .Take(5)
            .Select(o => new OrderSummaryDto
            {
                OrderId = o.Id,
                UserName = o.User.FirstName ,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount
            })
            .ToListAsync();

        if (latestOrders is null)
            return Result.Failure<OrdersDashboardDto>(OrderErrors.OrderNotFound);
                
        var dto = new OrdersDashboardDto
        {
            TotalOrders = totalOrders,
            PendingOrders = pending,
            CompletedOrders = completed,
            CancelledOrders = cancelled,
            LatestOrders = latestOrders
        };

        return Result.Success(dto);
    }

}
