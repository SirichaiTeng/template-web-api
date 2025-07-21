using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template_web_api_M.Common.ConnectionFactory;
public class DbContext
{
    private readonly IConfiguration _configuration;
    public DbContext(IConfiguration configuretion)
    {
        _configuration = configuretion ?? throw new ArgumentException(nameof(configuretion));
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
    public IDbConnection CreateConnection()
    {
        var connectionString = _configuration.GetConnectionString("sqlserver") ?? "";
        return new SqlConnection(connectionString);
    }
}
