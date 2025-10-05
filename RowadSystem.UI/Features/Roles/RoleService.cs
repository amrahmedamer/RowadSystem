using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Roles;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.Roles;

public class RoleService(IHttpClientFactory httpClient) : IRoleService
{
    private readonly HttpClient _httpClient = httpClient.CreateClient("AuthorizedClient");

    public async Task<Result<RoleRespons>> GetRoleByIdAsync(string roleId)
    {
        var response = await _httpClient.GetAsync($"/api/Roles/{roleId}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<RoleRespons>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<RoleRespons>(response);
        }
    }

    public async Task<Result<List<RoleRespons>>> GetRolesAsync()
    {
        var response = await _httpClient.GetAsync("/api/Roles");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<List<RoleRespons>>();
            return Result.Success(result);
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<List<RoleRespons>>(response);
        }

    }

    public async Task<Result> UpdateRoleAysnc(string roleId, RoleRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/Roles/{roleId}", request);
        if (response.IsSuccessStatusCode)
        {
            return Result.Success();
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<List<RoleRespons>>(response);
        }

    }
    public async Task<Result> UpdateRolePermissionsAysnc(string roleId, PermissionRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/Roles/{roleId}/permissions", request);
        if (response.IsSuccessStatusCode)
        {
            return Result.Success();
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<Result>(response);
        }

    }
    public async Task<Result> AddRoleAysnc( RoleRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Roles/", request);
        if (response.IsSuccessStatusCode)
        {
            return Result.Success();
        }
        else
        {
            return await PorblemDetailsExtensions.HandleErrorResponse<List<RoleRespons>>(response);
        }

    }
}
