
using OriginalExample.Interfaces.IRepositories;
using OriginalExample.Interfaces.IServices;
using OriginalExample.Models.Entity;

namespace OriginalExample.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<IEnumerable<UserInfo>> GetUserInfoAsync()
    {
        try
        {
            var result = await _userRepository.GetUsersAsync();
            return result;
        }
        catch (Exception ex)
        {
            return new List<UserInfo>();
        }
    }
}
