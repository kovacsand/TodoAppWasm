using Application.DaoInterfaces;
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
}