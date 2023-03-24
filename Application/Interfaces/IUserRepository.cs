using SV.Demo.Application.Models;

namespace SV.Demo.Application.Interfaces;

public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
    Task InsertAsync(User user);
}
