using OriginalExample.Constants;
using OriginalExample.Interfaces.IData;
using OriginalExample.Interfaces.IRepositories;
using OriginalExample.Models.Entity;

namespace OriginalExample.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly IDapperWarpper _dapperWarpper;
    public UserRepository(IDapperWarpper dapperWarpper, IDbConnectionFactory dbConnectionFactory)
    {
        _connectionFactory = dbConnectionFactory;
        _dapperWarpper = dapperWarpper;
    }

    public async Task<IEnumerable<UserInfo>> GetUsersAsync()
    {
        var connection = _connectionFactory.GetDatebaseInstance(DatabaseProvider.MicrosoftSql);
        var sql =  "";
        return await _dapperWarpper.QuaryAsync<UserInfo>(connection,sql, null); 
    }
}
