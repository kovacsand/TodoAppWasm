using Domain.Models;

namespace HttpClients.ClientInterfaces;

public interface ITodoService
{
    Task<IEnumerable<Todo>> GetAsync(string? userName, int? userId, bool? completedStatus, string? titleContains);
}