using OriginalExample.Models.Entity;

namespace OriginalExample.Interfaces.IRepositories;

public interface IUserRepository
{
    Task<IEnumerable<UserInfo>> GetUsersAsync();
}
