using Application.DaoInterfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfcDataAccess.DAOs;

public class UserEfcDao : IUserDao
{
    private readonly TodoContext context;

    public UserEfcDao(TodoContext context)
    {
        this.context = context;
    }
    
    public async Task<User> CreateAsync(User user)
    {
        EntityEntry<User> newUser = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return newUser.Entity;
    }

    public Task<User?> GetByUsernameAsync(string userName)
    {
        User? existing = context.Users.FirstOrDefault(u => u.UserName.ToLower().Equals(userName.ToLower()));
        return Task.FromResult(existing);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        User? existing = await context.Users.FindAsync(id);
        return existing;
    }

    public async Task<IEnumerable<User>> GetAsync(SearchUserParametersDto dto)
    {
        IQueryable<User> usersQuery = context.Users.AsQueryable();
        if (dto.UserNameContains != null)
            usersQuery = usersQuery.Where(u => u.UserName.ToLower().Contains(dto.UserNameContains.ToLower()));

        IEnumerable<User> result = await usersQuery.ToListAsync();
        return result;
    }
}