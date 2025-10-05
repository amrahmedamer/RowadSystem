using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Address;
using RowadSystem.Shard.Contract.Customers;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Phones;

namespace RowadSystem.Services;

public class CustomerService(ApplicationDbContext context) : ICustomerService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<int>> AddCustomerAsync(string createdByUserId, CustomerRequest request)
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            if (await _context.Customers.AnyAsync(x => x.Email == request.Email))
                return Result.Failure<int>(CustomerErrors.CustomerAlreadyExists);

            var customer = request.Adapt<Customer>();

            customer.Address = request.Address.Adapt<Address>();

            if (customer.Address is null)
                return Result.Failure<int>(CustomerErrors.InvalidAddress);

            var phones = request.PhoneNumbers.Adapt<List<ContactNumber>>();

            if (phones is null || !phones.Any())
                return Result.Failure<int>(CustomerErrors.InvalidPhoneNumbers);

            foreach (var phone in phones)
                customer.ContactNumbers!.Add(phone);

            customer.CreatedBy = createdByUserId;

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return Result.Success(customer.Id);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Result<CustomerResponse>> GetCustomerByIdAsync(int id)
    {
        var customers = await _context.Customers
          .AsNoTracking()
          .Where(x => x.Id == id)
          .Select(x => new CustomerResponse
          {
              Id = x.Id,
              Name = x.Name,
              Email = x.Email,
              Addresses = x.Address.Adapt<AddressRequest>(),
              PhoneNumbers = x.ContactNumbers!.Select(p => new PhoneNumberRequest { Number = p.Number, IsPrimary = p.IsPrimary }).ToList()
          }).SingleOrDefaultAsync();

        if (customers is null)
            return Result.Failure<CustomerResponse>(CustomerErrors.CustomerNotFound);

        return Result.Success(customers);
    }

    public async Task<Result<PaginatedListResponse<CustomerResponse>>> GetAllCustomersAsync(RequestFilters requestFilters)
    {
        var customers = _context.Customers
            .AsNoTracking()
            .Select(x => new CustomerResponse
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Addresses = x.Address.Adapt<AddressRequest>(),
                PhoneNumbers = x.ContactNumbers!.Select(p => new PhoneNumberRequest { Number = p.Number, IsPrimary = p.IsPrimary }).ToList()
            });

        var response = await PaginatedList<CustomerResponse>.CreatePaginationAsync(customers, requestFilters.PageNumber, requestFilters.PageSize);

        if (response is null)
            return Result.Failure<PaginatedListResponse<CustomerResponse>>(CustomerErrors.CustomerNotFound);

        return Result.Success(response.Adapt<PaginatedListResponse<CustomerResponse>>());
    }

    public async Task<Result<List<CustomerResponseDto>>> GetAllCustomerWithoutFilterAsync()
    {
        var Customer = _context.Customers
            .AsNoTracking()
            .Select(x => new CustomerResponseDto
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();


        if (Customer is null)
            return Result.Failure<List<CustomerResponseDto>>(SupplierErrors.SupplierNotFound);

        return Result.Success(Customer);
    }

    public async Task<Result<PaginatedListResponse<AccountStatementDto>>> GetAccountStatementAsync(int customerId, RequestFilters requestFilters)
    {
        var result = _context.SalesInvoices
            .Where(i => i.CustomerId == customerId)
            .Select(i => new AccountStatementDto
            {
                InvoiceId=i.Id,
                 InvoiceNumber = i.InvoiceNumber,
                InvoiceDate = i.InvoiceDate,
                Amount = i.NetAmount,
                Payments = _context.Payments
                    .Where(p => p.InvoiceId == i.Id)
                    .Sum(p => p.Amount),
                BalanceDue = i.NetAmount - _context.Payments
                    .Where(p => p.InvoiceId == i.Id)
                    .Sum(p => p.Amount)
            });

        var response = await PaginatedList<AccountStatementDto>.CreatePaginationAsync(result, requestFilters.PageNumber, requestFilters.PageSize);


        if (!response.Items.Any())
            return Result.Failure<PaginatedListResponse<AccountStatementDto>>(CustomerErrors.CustomerNotFound);

        return Result.Success(response.Adapt<PaginatedListResponse<AccountStatementDto>>());
    }


}
