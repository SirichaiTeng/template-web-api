using System.Data;

namespace OriginalExample.Interfaces.IData;

public interface IDapperWarpper
{
    Task<IEnumerable<T>> QuaryAsync<T>(IDbConnection connection, string sql, object? param = null);
    Task<T?> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string sql, object? param = null);
    Task<int> ExecuteAsync(IDbConnection connection, string sql, object? param = null);
}
