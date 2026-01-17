using Dapper;
using OriginalExample.Interfaces.IData;
using System.Data;

namespace OriginalExample.Data;

public class DapperWarpper : IDapperWarpper
{
    public async Task<int> ExecuteAsync(IDbConnection connection, string sql, object? param = null)
    {
       return await connection.ExecuteAsync(sql, param);
    }

    public async Task<IEnumerable<T>> QuaryAsync<T>(IDbConnection connection, string sql, object? param = null)
    {
       return await connection.QueryAsync<T>(sql, param);
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string sql, object? param = null)
    {
        return await connection.QueryFirstOrDefaultAsync<T>(sql, param);
    }

}
