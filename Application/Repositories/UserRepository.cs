using SV.Demo.Application.Interfaces;
using SV.Demo.Application.Models;

namespace SV.Demo.Application.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IApplicationContext _context;

    public UserRepository(IApplicationContext context)
    {
        _context = context;
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        _context.Users.Remove(user);

        await _context.SaveChangesAsync();
    }

    public async Task InsertAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task<User> GetByIdAsync(int id) => await _context.Users.FindAsync(id);

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        
        await _context.SaveChangesAsync();
    }
}
