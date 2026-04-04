using AutoFixture;
using Moq;
using OriginalExample.Interfaces.IRepositories;
using OriginalExample.Models.Entity;
using OriginalExample.Services;

namespace OriginalExample.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Fixture _fixture;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _fixture = new Fixture();
        _userService = new UserService(_mockUserRepository.Object);
    }

    [Fact]
    public async Task GetUserInfoAsync_ShouldReturnUsers_WhenRepositorySucceeds()
    {
        // Arrange
        var expectedUsers = _fixture.CreateMany<UserInfo>(3);

        _mockUserRepository
            .Setup(r => r.GetUsersAsync())
            .ReturnsAsync(expectedUsers);

        // Act
        var result = await _userService.GetUserInfoAsync();

        // Assert
        Assert.Equal(expectedUsers, result);
    }

    [Fact]
    public async Task GetUserInfoAsync_ShouldReturnEmptyList_WhenRepositoryThrowsException()
    {
        // Arrange
        _mockUserRepository
            .Setup(r => r.GetUsersAsync())
            .ThrowsAsync(new Exception("Database connection error"));

        // Act
        var result = await _userService.GetUserInfoAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUserInfoAsync_ShouldReturnEmptyList_WhenRepositoryReturnsEmpty()
    {
        // Arrange
        _mockUserRepository
            .Setup(r => r.GetUsersAsync())
            .ReturnsAsync(Enumerable.Empty<UserInfo>());

        // Act
        var result = await _userService.GetUserInfoAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUserInfoAsync_ShouldCallRepository_ExactlyOnce()
    {
        // Arrange
        _mockUserRepository
            .Setup(r => r.GetUsersAsync())
            .ReturnsAsync(Enumerable.Empty<UserInfo>());

        // Act
        await _userService.GetUserInfoAsync();

        // Assert
        _mockUserRepository.Verify(r => r.GetUsersAsync(), Times.Once);
    }

    [Fact]
    public async Task GetUserInfoAsync_ShouldReturnCorrectCount_WhenRepositoryReturnsMultipleUsers()
    {
        // Arrange
        var expectedCount = 5;
        var expectedUsers = _fixture.CreateMany<UserInfo>(expectedCount);

        _mockUserRepository
            .Setup(r => r.GetUsersAsync())
            .ReturnsAsync(expectedUsers);

        // Act
        var result = await _userService.GetUserInfoAsync();

        // Assert
        Assert.Equal(expectedCount, result.Count());
    }

    [Fact]
    public async Task GetUserInfoAsync_ShouldReturnEmptyList_WhenExceptionTypeIsInvalidOperation()
    {
        // Arrange
        _mockUserRepository
            .Setup(r => r.GetUsersAsync())
            .ThrowsAsync(new InvalidOperationException("Invalid operation"));

        // Act
        var result = await _userService.GetUserInfoAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
