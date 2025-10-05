
using RowadSystem.API.Helpers;
using RowadSystem.Shard.Abstractions;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Users;
using System.Net.Http.Json;

namespace RowadSystem.UI.Features.Users;

public class UserService(IHttpClientFactory httpClient) : IUserService
{
    private readonly HttpClient _httpClient = httpClient.CreateClient("AuthorizedClient");


    public async Task<Result<HttpResponseMessage>> AddUser(UserRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Users", request);

        if (response.IsSuccessStatusCode)
            return  Result.Success(response);
        else
            return await PorblemDetailsExtensions.HandleErrorResponse<HttpResponseMessage>(response);
    }


    public async Task<PaginatedListResponse<UserResponse>> GetAllUsers(RequestFilters filters)
    {
        var response = await _httpClient.GetAsync($"/api/Users?pageNumber={filters.PageNumber}&pageSize={filters.PageSize}");
        var users = await response.Content.ReadFromJsonAsync<PaginatedListResponse<UserResponse>>();

        return users ?? new PaginatedListResponse<UserResponse>();
    }
    public async Task<List<string>> GetAllRoles()
    {
        var response = await _httpClient.GetAsync("/api/Roles/without-filter");
        var roles = await response.Content.ReadFromJsonAsync<List<string>>();
      
        return roles ?? new List<string>();
    }
}
