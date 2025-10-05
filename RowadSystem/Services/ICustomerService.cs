using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Customers;
using RowadSystem.Shard.Contract.Helpers;

namespace RowadSystem.Services;

public interface ICustomerService
{
    Task<Result<int>> AddCustomerAsync(string createdByUserId, CustomerRequest request);
    Task<Result<PaginatedListResponse<CustomerResponse>>> GetAllCustomersAsync(RequestFilters requestFilters);
    Task<Result<CustomerResponse>> GetCustomerByIdAsync(int id);
    Task<Result<List<CustomerResponseDto>>> GetAllCustomerWithoutFilterAsync();
    Task<Result<PaginatedListResponse<AccountStatementDto>>> GetAccountStatementAsync(int customerId, RequestFilters requestFilters);


}
