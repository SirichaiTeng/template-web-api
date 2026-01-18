using OriginalExample.Models.Entity;

namespace OriginalExample.Interfaces.IServices;

public interface IUserService
{
    public Task<IEnumerable<UserInfo>> GetUserInfoAsync();
}
