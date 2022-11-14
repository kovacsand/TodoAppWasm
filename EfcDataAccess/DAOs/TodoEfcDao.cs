using Application.DaoInterfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfcDataAccess.DAOs;

public class TodoEfcDao : ITodoDao
{
    private readonly TodoContext context;

    public TodoEfcDao(TodoContext context)
    {
        this.context = context;
    }
    
    public async Task<Todo> CreateAsync(Todo todo)
    {
        EntityEntry<Todo> newTodo = await context.Todos.AddAsync(todo);
        await context.SaveChangesAsync();
        return newTodo.Entity;
    }

    public async Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto searchTodoParameters)
    {
        IQueryable<Todo> query = context.Todos.Include(todo => todo.Owner).AsQueryable();
    
        if (!string.IsNullOrEmpty(searchTodoParameters.UserName))
            query = query.Where(todo => todo.Owner.UserName.ToLower().Equals(searchTodoParameters.UserName.ToLower()));

        if (searchTodoParameters.UserId != null)
            query = query.Where(todo => todo.Owner.Id == searchTodoParameters.UserId);

        if (searchTodoParameters.CompletedStatus != null)
            query = query.Where(todo => todo.IsCompleted == searchTodoParameters.CompletedStatus);

        if (!string.IsNullOrEmpty(searchTodoParameters.TitleContains))
            query = query.Where(todo => todo.Title.ToLower().Contains(searchTodoParameters.TitleContains.ToLower()));

        List<Todo> result = await query.ToListAsync();
        return result;
    }

    public async Task<Todo> GetByIdAsync(int id)
    {
        Todo? existing = await context.Todos.FindAsync(id);
        return existing;
    }

    public async Task UpdateAsync(Todo todo)
    {
        context.ChangeTracker.Clear();
        context.Todos.Update(todo);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Todo? existing = await GetByIdAsync(id);
        if (existing == null)
            throw new Exception($"Todo with id {id} not found!");
        
        context.Todos.Remove(existing);
        await context.SaveChangesAsync();
    }
}