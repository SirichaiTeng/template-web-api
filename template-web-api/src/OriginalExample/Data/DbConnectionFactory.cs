using Microsoft.Data.SqlClient;
using Npgsql;
using OriginalExample.Common.Constants;
using OriginalExample.Interfaces.IData;
using System.Data;

namespace OriginalExample.Data;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _configuration;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException( nameof( configuration ) );
    }
    public IDbConnection GetDatabaseInstance(DatabaseProvider provider)
    {
        var connectionString = GetConnectionString(provider);

        return provider switch
        {
            DatabaseProvider.MicrosoftSql => new SqlConnection(connectionString),
            DatabaseProvider.PostgreSql => new NpgsqlConnection(connectionString),
            _ => throw new NotSupportedException( $"Database provider '{provider}' is not supported.")
        };
    }

    private string GetConnectionString(DatabaseProvider provider)
    {
        var key = $"ConnectionStrings:{provider}";

        var connectionString = _configuration[key];

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                $"Connection string '{provider}' is missing or empty.");

        return connectionString;
    }
}
