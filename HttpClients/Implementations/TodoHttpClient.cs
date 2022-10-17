using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Domain.DTOs;
using Domain.Models;
using HttpClients.ClientInterfaces;

namespace HttpClients.Implementations;

public class TodoHttpClient : ITodoService
{
    private readonly HttpClient client;

    public TodoHttpClient(HttpClient client)
    {
        this.client = client;
    }

    public async Task CreateAsync(TodoCreationDto dto)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/todos", dto);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(result);
    }

    public async Task<IEnumerable<Todo>> GetAsync(string? userName, int? userId, bool? completedStatus, string? titleContains)
    {
        string query = ConstructQuery(userName, userId, completedStatus, titleContains);
        HttpResponseMessage response = await client.GetAsync("/todos" + query);
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        IEnumerable<Todo> todos = JsonSerializer.Deserialize<IEnumerable<Todo>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        return todos;
    }

    public async Task<TodoDto> GetByIdAsync(int id)
    {
        HttpResponseMessage respone = await client.GetAsync($"/todos/{id}");
        string content = await respone.Content.ReadAsStringAsync();
        if (!respone.IsSuccessStatusCode)
            throw new Exception(content);

        TodoDto todo = JsonSerializer.Deserialize<TodoDto>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        return todo;
    }

    public async Task UpdateAsync(TodoUpdateDto dto)
    {
        string dtoAsJson = JsonSerializer.Serialize(dto);
        StringContent body = new StringContent(dtoAsJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PatchAsync("/todos", body);
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }

    private static string ConstructQuery(string? userName, int? userId, bool? completedStatus, string? titleContains)
    {
        string query = "";
        if (!string.IsNullOrEmpty(userName))
            query += $"?username={userName}";
        if (userId != null)
            query += (string.IsNullOrEmpty(query) ? "?" : "&") + $"userid={userId}";
        if (completedStatus != null)
            query += (string.IsNullOrEmpty(query) ? "?" : "&") + $"completedstatus={completedStatus}";
        if (!string.IsNullOrEmpty(titleContains))
            query += (string.IsNullOrEmpty(query) ? "?" : "&") + $"titlecontains={titleContains}";

        return query;
    }
}