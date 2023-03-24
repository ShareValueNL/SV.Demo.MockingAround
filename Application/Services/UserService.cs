using SV.Demo.Application.Interfaces;
using SV.Demo.Application.Models;

namespace SV.Demo.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
	{
        _userRepository = userRepository;
    }

    public async Task<User> GetUserById(int userId) => await _userRepository.GetByIdAsync(userId);

    public async Task UpdateUser(User user) => await _userRepository.UpdateAsync(user);

    public async Task DeleteUser(int userId) => await _userRepository.DeleteAsync(userId);
    
    public async Task InsertUser(User user) => await _userRepository.InsertAsync(user);
}
