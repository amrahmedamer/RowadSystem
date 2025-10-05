using RowadSystem.API.Helpers;
using RowadSystem.Entity;
using RowadSystem.Shard.Contract.Address;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Phones;
using RowadSystem.Shard.Contract.Suppliers;

namespace RowadSystem.Services;

public class SupplierService(ApplicationDbContext context) : ISupplierService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<int>> AddSupplierAsync(string createdByUserId, SupplierRequest request)
    {

        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            if (await _context.Suppliers.AnyAsync(x => x.Email == request.Email))
                return Result.Failure<int>(SupplierErrors.SupplierAlreadyExists);

            var supplier = request.Adapt<Supplier>();
            supplier.Address = request.Address.Adapt<Address>();

            if (supplier.Address is null)
                return Result.Failure<int>(CustomerErrors.InvalidAddress);

            var phones = request.PhoneNumbers.Adapt<List<ContactNumber>>();

            if (phones is null || !phones.Any())
                return Result.Failure<int>(CustomerErrors.InvalidPhoneNumbers);

            foreach (var phone in phones)
                supplier.ContactNumbers!.Add(phone);

            supplier.CreatedBy = createdByUserId;

            await _context.AddAsync(supplier);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return Result.Success(supplier.Id);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }

    }

    public async Task<Result<SupplierResponse>> GetSupplierByIdAsync(int id)
    {
        var supplier = await _context.Suppliers
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new SupplierResponse
            {

                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                //Addresses = x.Address.Adapt<AddressRequest>(),
                PhoneNumbers = x.ContactNumbers!.Select(p => new PhoneNumberRequest { Number = p.Number, IsPrimary = p.IsPrimary }).ToList()
            }).SingleOrDefaultAsync();
        if (supplier is null)
            return Result.Failure<SupplierResponse>(SupplierErrors.SupplierNotFound);
        return Result.Success(supplier);
    }
    public async Task<Result<PaginatedListResponse<SupplierResponse>>> GetAllSuppliersAsync(RequestFilters requestFilters)
    {
        var suppliers = _context.Suppliers
            .AsNoTracking()
            .Select(x => new SupplierResponse
            {

                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Addresses = x.Address.Adapt<AddressRequest>(),
                PhoneNumbers = x.ContactNumbers!.Select(p => new PhoneNumberRequest { Number = p.Number, IsPrimary = p.IsPrimary }).ToList()
            });

        var result = await PaginatedList<SupplierResponse>.CreatePaginationAsync(suppliers, requestFilters.PageNumber, requestFilters.PageSize);

        if (result is null)
            return Result.Failure<PaginatedListResponse<SupplierResponse>>(SupplierErrors.SupplierNotFound);

        return Result.Success(result.Adapt<PaginatedListResponse<SupplierResponse>>());
    }
    public async Task<Result<List<SupplierResponseDto>>> GetAllSuppliersWithoutFilterAsync()
    {
        var suppliers = _context.Suppliers
            .AsNoTracking()
            .Select(x => new SupplierResponseDto
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();

     
        if (suppliers is null)
            return Result.Failure<List<SupplierResponseDto>>(SupplierErrors.SupplierNotFound);

        return Result.Success(suppliers);
    }

  
    public async Task<Result<PaginatedListResponse<AccountStatementDto>>> GetAccountStatementAsync(int supplierId, RequestFilters requestFilters)
    {
        var result = _context.PurchaseInvoices
            .Where(i => i.SupplierId == supplierId)
            .Select(i => new AccountStatementDto
            {
                InvoiceNumber = i.InvoiceNumber,
                InvoiceDate = i.InvoiceDate,
                Amount = i.TotalAmount,
                Payments = _context.Payments
                    .Where(p => p.InvoiceId == i.Id)
                    .Sum(p => p.Amount),
                BalanceDue = i.TotalAmount - _context.Payments
                    .Where(p => p.InvoiceId == i.Id)
                    .Sum(p => p.Amount)
            });

        var response = await PaginatedList<AccountStatementDto>.CreatePaginationAsync(result, requestFilters.PageNumber, requestFilters.PageSize);


        if (!response.Items.Any())
            return Result.Failure<PaginatedListResponse<AccountStatementDto>>(SupplierErrors.SupplierNotFound);

        return Result.Success(response.Adapt<PaginatedListResponse<AccountStatementDto>>());
    }


}
