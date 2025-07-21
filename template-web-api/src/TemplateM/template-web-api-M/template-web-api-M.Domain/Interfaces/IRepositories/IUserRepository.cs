using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using template_web_api_M.Domain.Entites;

namespace template_web_api_M.Domain.Interfaces.IRepositories;
public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllAsync();
    Task<IEnumerable<User>> GetActiveUsersAsync();
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(string username, string email);
    Task<User> AuthenticateAsync(string username, string password);
    Task UpdateLastLoginAsync(int userId);
    Task<bool> UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime expiry);
}
