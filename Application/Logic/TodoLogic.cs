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

        ValidateData(dto);
        Todo toCreate = new Todo(owner, dto.Title);
        Todo created = await todoDao.CreateAsync(toCreate);
        return created;
    }

    public Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto searchTodoParameters)
    {
        return todoDao.GetAsync(searchTodoParameters);
    }

    private void ValidateData(TodoCreationDto dto)
    {
        if (string.IsNullOrEmpty(dto.Title))
            throw new Exception("Title cannot be empty!");
    }
}