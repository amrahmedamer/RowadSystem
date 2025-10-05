using RowadSystem.API.Helpers;
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Customers;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Invoices;

namespace RowadSystem.UI.Features.Customers;

public interface ICustomerService
{
    Task<PaginatedListResponse<CustomerResponse>> GetAllCustomer(RequestFilters filters);
    Task<HttpResponseMessage> AddCustomer(CustomerRequest request);
    Task<Result<PaginatedListResponse<AccountStatementDto>>> GetAccountStatement(int id, RequestFilters filters);
    Task<Result<List<CustomerResponseDto>>> GetCustomersAsync();
}
