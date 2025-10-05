using RowadSystem.API.Helpers;
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Invoices;
using RowadSystem.Shard.Contract.Suppliers;

namespace RowadSystem.UI.Features.Suppliers;

public interface ISupplierService
{
    Task<PaginatedListResponse<SupplierResponse>> GetAllSupplier(RequestFilters filters);
    Task<Result<List<SupplierResponse>>> GetSuppliersAsync();
    Task<HttpResponseMessage> AddSupplier(SupplierRequest request);
    Task<Result<PaginatedListResponse<AccountStatementDto>>> GetAccountStatement(int id, RequestFilters filters);
}
