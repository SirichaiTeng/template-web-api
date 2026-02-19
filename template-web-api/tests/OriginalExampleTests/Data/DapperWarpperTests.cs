using Dapper;
using Moq;
using Moq.Dapper;
using OriginalExample.Data;
using OriginalExample.Interfaces.IData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginalExample.Tests.Data;
public class DapperWarpperTests
{
    private readonly Mock<IDbConnection> _connectionMock;
    private readonly DapperWarpper _dapperWarpper;

    public DapperWarpperTests()
    {
        _dapperWarpper = new DapperWarpper();
        _connectionMock = new Mock<IDbConnection>();
    }

    [Fact]
    public async Task QueryAsync_ShouldReturnResults_WithMoqDapper()
    {
        // Arrange
        const string sql = @"select * from Table";
        var expectedResult = new List<string> { "Result1", "Result2" };

        _connectionMock.SetupDapperAsync(f => f.QueryAsync<string>(sql,null,null,null,null)).ReturnsAsync(expectedResult);

        // Act
        var result = await _dapperWarpper.QuaryAsync<string>(_connectionMock.Object,sql);

        // Assert
        Assert.Equal(expectedResult,result);
    } 
    
    [Fact]
    public async Task QueryFirstOrDefaultAsync_ShouldReturnResults_WithMoqDapper()
    {
        // Arrange
        const string sql = @"select * from Table";
        var expectedResult = "Result1";

        _connectionMock.SetupDapperAsync(f => f.QueryFirstOrDefaultAsync<string>(sql,null,null,null,null)).ReturnsAsync(expectedResult);

        // Act
        var result = await _dapperWarpper.QueryFirstOrDefaultAsync<string>(_connectionMock.Object,sql);

        // Assert
        Assert.Equal(expectedResult,result);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnResults_WithMoqDapper()
    {
        // Arrange
        const string sql = @"select * from Table";
        var expectedResult = 1;

        _connectionMock.SetupDapperAsync(f => f.ExecuteAsync(sql, null, null, null, null)).ReturnsAsync(expectedResult);

        // Act
        var result = await _dapperWarpper.ExecuteAsync(_connectionMock.Object, sql);

        // Assert
        Assert.Equal(expectedResult, result);
    }
}
