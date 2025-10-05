using RowadSystem.API.Helpers;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Suppliers;

namespace RowadSystem.Services;

public interface ISupplierService
{
    Task<Result<int>> AddSupplierAsync(string createdByUserId, SupplierRequest request);
    Task<Result<PaginatedListResponse<SupplierResponse>>> GetAllSuppliersAsync(RequestFilters requestFilters);
    Task<Result<SupplierResponse>> GetSupplierByIdAsync(int id);
    Task<Result<List<SupplierResponseDto>>> GetAllSuppliersWithoutFilterAsync();
    Task<Result<PaginatedListResponse<AccountStatementDto>>> GetAccountStatementAsync(int supplierId, RequestFilters requestFilters);
}
