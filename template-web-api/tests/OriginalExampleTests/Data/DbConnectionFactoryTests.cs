using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Moq;
using OriginalExample.Common.Constants;
using OriginalExample.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginalExample.Tests.Data;
public class DbConnectionFactoryTests
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly DbConnectionFactory _dbConnectionFactory;

    public DbConnectionFactoryTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        _dbConnectionFactory = new DbConnectionFactory(_configurationMock.Object);
    }

    [Fact]
    public void CreateNpgsqlConnection_ShouldRetuenNpgsqlConnection_WhenConnectionStringExists()
    {
        // Arrange
        var mockConnectionString = @"Server=myServaerAddress;Database=myDatabase;User Id=myId;Password=myPassword";
        _configurationMock.Setup(config => config["ConnectionStrings:PostgreSql"]).Returns(mockConnectionString);

        // Act
        var connection = _dbConnectionFactory.GetDatabaseInstance(DatabaseProvider.PostgreSql);

        // Assert
        Assert.IsType<Npgsql.NpgsqlConnection>(connection);
    } 

    [Fact]
    public void CreateConnection_ShouldThrowInvalidOperationException_WhenConnectionEmpty()
    {
        
        // Act
        var ex = Assert.Throws<InvalidOperationException>(()=>
        _dbConnectionFactory.GetDatabaseInstance(DatabaseProvider.PostgreSql));

        // Assert
        Assert.Contains("missing or empty", ex.Message,StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CreateSqlConnection_ShouldRetuenSqlConnection_WhenConnectionStringExists()
    {
        // Arrange
        var mockConnectionString = @"Server=myServaerAddress;Database=myDatabase;User Id=myId;Password=myPassword";
        _configurationMock.Setup(config => config["ConnectionStrings:MicrosoftSql"]).Returns(mockConnectionString);

        // Act
        var connection = _dbConnectionFactory.GetDatabaseInstance(DatabaseProvider.MicrosoftSql);

        // Assert
        Assert.IsType<SqlConnection>(connection);
    }
    [Fact]
    public void GetDatabaseInstance_WithUnSupportProvider_ShouldThrowNotSupportedException()
    {
        // Arrange
        var inavlidProvider = (DatabaseProvider)123;
        var mockConnectionString = @"Server=myServaerAddress;Database=myDatabase;User Id=myId;Password=myPassword";
        _configurationMock.Setup(config => config["ConnectionStrings:123"]).Returns(mockConnectionString);

        // Act & Assert
        var ex = Assert.Throws<NotSupportedException>(() =>
        _dbConnectionFactory.GetDatabaseInstance(inavlidProvider));

        Assert.Contains("not supported", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}
