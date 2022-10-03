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

    public Task<Todo?> GetByIdAsync(int id)
    {
        Todo? existing = context.Todos.FirstOrDefault(todo => todo.Id == id);
        return Task.FromResult(existing);
    }

    public Task UpdateAsync(Todo todo)
    {
        Todo? existing = context.Todos.FirstOrDefault(foundTodo => foundTodo.Id == todo.Id);
        if (existing == null)
            throw new Exception($"Todo with id {todo.Id} not found!");
        
        context.Todos.Remove(existing);
        context.Todos.Add(todo);
        context.SaveChanges();
        
        return Task.CompletedTask; 
    }

    public Task DeleteAsync(int id)
    {
        Todo? existing = context.Todos.FirstOrDefault(foundTodo => foundTodo.Id == id);
        if (existing == null)
            throw new Exception($"Todo with id {id} not found!");
        
        context.Todos.Remove(existing);
        context.SaveChanges();
        
        return Task.CompletedTask;
    }
}