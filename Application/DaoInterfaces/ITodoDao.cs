using Domain.DTOs;
using Domain.Models;

namespace Application.DaoInterfaces;

public interface ITodoDao
{
    Task<Todo> CreateAsync(Todo todo);

    Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto searchTodoParameters);

    Task<Todo> GetByIdAsync(int id);
    
    Task UpdateAsync(Todo todo);
}