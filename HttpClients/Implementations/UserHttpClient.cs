using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Domain.DTOs;
using Domain.Models;
using HttpClients.ClientInterfaces;

namespace HttpClients.Implementations;

public class UserHttpClient : IUserService
{
    private readonly HttpClient client;

    public UserHttpClient(HttpClient client)
    {
        this.client = client;
    }

    public async Task<User> CreateAsync(UserCreationDto dto)
    {
        HttpResponseMessage respone = await client.PostAsJsonAsync("/users", dto);
        string result = await respone.Content.ReadAsStringAsync();
        if (!respone.IsSuccessStatusCode)
            throw new Exception(result);
        
        User user = JsonSerializer.Deserialize<User>(result, new JsonSerializerOptions{PropertyNameCaseInsensitive = true})!;
        return user;
    }
}