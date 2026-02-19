using AutoFixture;
using DocumentFormat.OpenXml.Office2010.Excel;
using Moq;
using OriginalExample.Common.Constants;
using OriginalExample.Interfaces.IData;
using OriginalExample.Models.Entity;
using OriginalExample.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginalExample.Tests.Repositories;

public class UserRepositoryTests
{
    private readonly Mock<IDbConnectionFactory> _dbconnectionFactory;
    private readonly Mock<IDbConnection> _dbconnection;
    private readonly Mock<IDapperWarpper> _mockDapperWarpprt;
    private readonly Fixture _fixture;
    private readonly UserRepository _userRepository;
    public UserRepositoryTests()
    {
        _dbconnectionFactory = new Mock<IDbConnectionFactory>();
        _dbconnection = new Mock<IDbConnection>();
        _mockDapperWarpprt = new Mock<IDapperWarpper>();
        _fixture = new Fixture();
        _userRepository = new UserRepository(_mockDapperWarpprt.Object, _dbconnectionFactory.Object);
    }
    [Fact]
    public async Task GetUsersAsync_ShouldSuccess()
    {
        // Arrange
        var expectResult = _fixture.CreateMany<UserInfo>(2);

        _dbconnectionFactory.Setup(db => db.GetDatabaseInstance(DatabaseProvider.MicrosoftSql)).Returns(_dbconnection.Object);

        _mockDapperWarpprt.Setup(f => f.QueryAsync<UserInfo>(_dbconnection.Object,It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(expectResult);

        // Act
        var results = await _userRepository.GetUsersAsync();

        // Assert
        Assert.Equal(expectResult, (results));
    }  

    [Fact]
    public async Task GetUserByIdAsync_ShouldSuccess()
    {
        // Arrange

        // ตัวอย่างอยาก mock ค่าเอง
        //var user = _fixture.Build<UserInfo>()
        //.With(x => x.UserName, "Mr.Sun")
        //.With(x => x.UserEmail, "abc@Necro")
        //.Create();

        var sql = @"SELECT Id, Username, Email, CreatedAt, IsActive
                FROM Users
                WHERE Id = @Id";

        var expectResult = _fixture.Create<UserInfo>();

        _dbconnectionFactory.Setup(db => db.GetDatabaseInstance(DatabaseProvider.MicrosoftSql)).Returns(_dbconnection.Object);

        _mockDapperWarpprt.Setup(f => f.QueryFirstOrDefaultAsync<UserInfo>(_dbconnection.Object, sql, It.IsAny<object>())).ReturnsAsync(expectResult);

        // Act
        var results = await _userRepository.GetUserByIdAsync(1);

        // Assert
        Assert.Equal(expectResult.UserName, results?.UserName);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnId()
    {
        var expectId = _fixture.Create<int>();

        _dbconnectionFactory
            .Setup(x => x.GetDatabaseInstance(It.IsAny<DatabaseProvider>()))
            .Returns(_dbconnection.Object);

        _mockDapperWarpprt
            .Setup(x => x.QueryFirstOrDefaultAsync<int>(
                _dbconnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>()))
            .ReturnsAsync(expectId);

        var result = await _userRepository.CreateUserAsync("user", "mail@test.com");

        Assert.Equal(expectId, result);
    }

    [Fact]
    public async Task IsEmailExistsAsync_ShouldReturnTrue()
    {
        // Arrange
        var sql = @"SELECT 1 FROM Users WHERE Email = @Email";
        _dbconnectionFactory
            .Setup(x => x.GetDatabaseInstance(It.IsAny<DatabaseProvider>()))
            .Returns(_dbconnection.Object);

        _mockDapperWarpprt
            .Setup(x => x.QueryFirstOrDefaultAsync<int?>(
                _dbconnection.Object,
                sql,
                It.IsAny<object>()))
            .ReturnsAsync(1);

        // Act
        var result = await _userRepository.IsEmailExistsAsync("test@test.com");


        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetUsersPagedAsync_ShouldReturnList()
    {
        var expectList = _fixture.CreateMany<UserInfo>(5);

        _dbconnectionFactory
            .Setup(x => x.GetDatabaseInstance(It.IsAny<DatabaseProvider>()))
            .Returns(_dbconnection.Object);

        _mockDapperWarpprt
            .Setup(x => x.QueryAsync<UserInfo>(
                _dbconnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>()))
            .ReturnsAsync(expectList);

        var result = await _userRepository.GetUsersPagedAsync(1, 10);

        Assert.NotNull(result);
        Assert.Equal(5, result.Count());
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnTrue_WhenAffectedRow()
    {
        _dbconnectionFactory
            .Setup(x => x.GetDatabaseInstance(It.IsAny<DatabaseProvider>()))
            .Returns(_dbconnection.Object);

        _mockDapperWarpprt
            .Setup(x => x.ExecuteAsync(
                _dbconnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>()))
            .ReturnsAsync(1);

        var result = await _userRepository.UpdateUserAsync(1, "email", true);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldReturnTrue()
    {
        _dbconnectionFactory
            .Setup(x => x.GetDatabaseInstance(It.IsAny<DatabaseProvider>()))
            .Returns(_dbconnection.Object);

        _mockDapperWarpprt
            .Setup(x => x.ExecuteAsync(
                _dbconnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>()))
            .ReturnsAsync(1);

        var result = await _userRepository.DeleteUserAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task CreateUserPostgresqlAsync_ShouldReturnId()
    {
        // Arrange
        _dbconnectionFactory
            .Setup(x => x.GetDatabaseInstance(It.IsAny<DatabaseProvider>()))
            .Returns(_dbconnection.Object);

        _mockDapperWarpprt
            .Setup(x => x.QueryFirstOrDefaultAsync<int>(
                _dbconnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>()))
            .ReturnsAsync(1);

        // Act
        var result = await _userRepository.CreateUserPostgresqlAsync("email", "user");

        // Assert
        Assert.Equal(result,1);
    }
    
    [Fact]
    public async Task IsEmailExistsPostgresqlAsync_ShouldReturnTrue()
    {
        // Arrange
        _dbconnectionFactory
            .Setup(x => x.GetDatabaseInstance(It.IsAny<DatabaseProvider>()))
            .Returns(_dbconnection.Object);

        _mockDapperWarpprt
            .Setup(x => x.QueryFirstOrDefaultAsync<bool>(
                _dbconnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>()))
            .ReturnsAsync(true);

        // Act
        var result = await _userRepository.IsEmailExistsPostgresqlAsync("email");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetUsersPagedPostgresqlAsync_ShouldReturnPagedResult()
    {
        // Arrange
        var page = 2;
        var pageSize = 10;
        var expectedOffset = (page - 1) * pageSize;

        var expectedUsers = _fixture.CreateMany<UserInfo>(5).ToList();

        _dbconnectionFactory
            .Setup(x => x.GetDatabaseInstance(DatabaseProvider.PostgreSql))
            .Returns(_dbconnection.Object);

        _mockDapperWarpprt
            .Setup(x => x.QueryAsync<UserInfo>(
                _dbconnection.Object,
                It.IsAny<string>(),
                It.Is<object>(o =>
                    (int)o.GetType().GetProperty("PageSize")!.GetValue(o)! == pageSize &&
                    (int)o.GetType().GetProperty("Offset")!.GetValue(o)! == expectedOffset
                )))
            .ReturnsAsync(expectedUsers);

        // Act
        var result = await _userRepository
            .GetUsersPagedPostgresqlAsync(page, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUsers.Count, result.Count());
    }
}
