using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace Application.Logic;

public class TodoLogic : ITodoLogic
{
    private readonly ITodoDao todoDao;
    private readonly IUserDao userDao;

    public TodoLogic(ITodoDao todoDao, IUserDao userDao)
    {
        this.todoDao = todoDao;
        this.userDao = userDao;
    }
    
    public async Task<Todo> CreateAsync(TodoCreationDto dto)
    {
        User? owner = await userDao.GetByIdAsync(dto.OwnerId);
        if (owner == null)
            throw new Exception($"User with id {dto.OwnerId} was not found!");
        
        Todo toCreate = new Todo(owner, dto.Title);
        ValidateData(toCreate);
        Todo created = await todoDao.CreateAsync(toCreate);
        return created;
    }

    public Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto searchTodoParameters)
    {
        return todoDao.GetAsync(searchTodoParameters);
    }

    public async Task<TodoDto> GetByIdAsync(int id)
    {
        Todo? todo = await todoDao.GetByIdAsync(id);
        if (todo == null)
        {
            throw new Exception($"Todo with id {id} not found");
        }

        return new TodoDto(todo.Id, todo.Owner.UserName, todo.Title, todo.IsCompleted);
    }

    public async Task UpdateAsync(TodoUpdateDto dto)
    {
        Todo? existing = await todoDao.GetByIdAsync(dto.Id);
        if (existing == null)
            throw new Exception($"Todo with ID {dto.Id} not found!");

        User? user = null;
        if (dto.OwnerId != null)
        {
            user = await userDao.GetByIdAsync((int)(dto.OwnerId));
            if (user == null)
                throw new Exception($"User with id {dto.OwnerId} was not found!");
        }
        if (dto.IsCompleted != null && existing.IsCompleted && !(bool)dto.IsCompleted)
        {
            throw new Exception("Cannot un-complete a completed Todo");
        }

        User userToUse = user ?? existing.Owner;
        string titleToUse = dto.Title ?? existing.Title;
        bool completedToUse = dto.IsCompleted ?? existing.IsCompleted;
        
        Todo updated = new (userToUse, titleToUse)
        {
            IsCompleted = completedToUse,
            Id = existing.Id,
        };
        
        ValidateData(updated);

        await todoDao.UpdateAsync(updated);
    }

    public async Task DeleteAsync(int id)
    {
        Todo? existing = await todoDao.GetByIdAsync(id);
        if (existing == null)
            throw new Exception($"Todo with ID {id} not found!");
        if (!existing.IsCompleted)
            throw new Exception($"Cannot delete uncompleted todo!");
        await todoDao.DeleteAsync(id);
    }

    private void ValidateData(Todo dto)
    {
        if (string.IsNullOrEmpty(dto.Title))
            throw new Exception("Title cannot be empty!");
    }
}