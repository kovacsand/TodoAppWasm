using Application.DaoInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace FileData.DAOs;

public class TodoFileDao : ITodoDao
{
    private readonly FileContext context;

    public TodoFileDao(FileContext context)
    {
        this.context = context;
    }
    
    public Task<Todo> CreateAsync(Todo todo)
    {
        int todoId = 1;
        if (context.Todos.Any())
        {
            todoId = context.Todos.Max(todo => todo.Id) + 1;
        }

        todo.Id = todoId;
        context.Todos.Add(todo);
        context.SaveChanges();

        return Task.FromResult(todo);
    }

    public Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto searchTodoParameters)
    {
        IEnumerable<Todo> todos = context.Todos.AsEnumerable();
        if (!string.IsNullOrEmpty(searchTodoParameters.UserName))
            todos = todos.Where(todo => todo.Owner.UserName.Equals(searchTodoParameters.UserName, StringComparison.OrdinalIgnoreCase));
        if (searchTodoParameters.UserId != null)
            todos = todos.Where(todo => todo.Owner.Id.Equals(searchTodoParameters.UserId));
        if (searchTodoParameters.CompletedStatus != null)
            todos = todos.Where(todo => todo.IsCompleted == searchTodoParameters.CompletedStatus);
        if (!string.IsNullOrEmpty(searchTodoParameters.TitleContains))
            todos = todos.Where(todo => todo.Title.Contains(searchTodoParameters.TitleContains, StringComparison.OrdinalIgnoreCase));
        
        return Task.FromResult(todos);
    }   
}