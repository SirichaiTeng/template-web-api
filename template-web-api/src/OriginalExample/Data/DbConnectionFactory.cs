using Microsoft.Data.SqlClient;
using Npgsql;
using OriginalExample.Constants;
using OriginalExample.Interfaces.IData;
using System.Data;

namespace OriginalExample.Data;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _configuration;

    public DbConnectionFactory(IConfiguration configuration )
    {
        _configuration = configuration ?? throw new ArgumentNullException( nameof( configuration ) );
    }
    public IDbConnection GetDatebaseInstance(DatabaseProvider databaseProvider)
    {
        var (connStrName, ctor) = databaseProvider switch
        {
            DatabaseProvider.MicrosoftSql => ("ConnectionStrings:MsSql",
            (Func<string, IDbConnection>)(cs => new SqlConnection(cs))),

            DatabaseProvider.PostgreSql => ("ConnectionStrings:PostgreSql",
            cs => new NpgsqlConnection(cs)),

            _ => throw new NotSupportedException($"Database provider '{databaseProvider}' is not support.")
        };

        var connectionString = _configuration.GetSection(connStrName).Value ?? "";
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException($"Connection string '{connStrName}' is missing or empty");
        }

        return ctor(connectionString);
    }
}
