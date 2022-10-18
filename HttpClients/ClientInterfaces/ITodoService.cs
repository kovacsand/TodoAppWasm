using Domain.DTOs;
using Domain.Models;

namespace HttpClients.ClientInterfaces;

public interface ITodoService
{
    Task CreateAsync(TodoCreationDto dto);
    Task<IEnumerable<Todo>> GetAsync(string? userName, int? userId, bool? completedStatus, string? titleContains);
    Task<TodoDto> GetByIdAsync(int id);
    Task UpdateAsync(TodoUpdateDto dto);
    Task DeleteAsync(int id);
}