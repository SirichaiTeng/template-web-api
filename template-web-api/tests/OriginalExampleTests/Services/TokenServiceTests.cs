using Moq;
using OriginalExample.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OriginalExample.Services;
using Moq.Protected;
using System.Net.Http;

namespace OriginalExample.Tests.Services;
public class TokenServiceTests
{
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly Mock<HttpMessageHandler> _mockhttpMessage;
    private readonly Mock<IHttpClientFactory> _mockDbConnectFactory;
    private readonly TokenService _service;
    public TokenServiceTests()
    {
        _mockConfig = new Mock<IConfiguration>();
        _mockhttpMessage = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var httpClient = new HttpClient(_mockhttpMessage.Object)
        {
            BaseAddress = new Uri("https://test.com")
        };
        _mockDbConnectFactory = new Mock<IHttpClientFactory>();
        _mockDbConnectFactory
    .Setup(x => x.CreateClient("TokenApi"))
    .Returns(httpClient);
        _service = new TokenService(_mockDbConnectFactory.Object, _mockConfig.Object);
    }

    [Fact]
    public async Task GetTokenAsync_ShouldReturnAccessToken_WhenSuccess()
    {
        // Arrange
        var expectedToken = "abc123";

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new TokenResponse
            {
                AccessToken = expectedToken
            })
        };

        _mockhttpMessage
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
               "SendAsync",
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>()
           )
           .ReturnsAsync(response);

        _mockConfig.Setup(x => x["TokenApi:ClientId"]).Returns("client");
        _mockConfig.Setup(x => x["TokenApi:ClientSecret"]).Returns("secret");


        // Act
        var result = await _service.GetTokenAsync();

        // Assert
        Assert.Equal(expectedToken, result);
    }

    [Fact]
    public async Task GetTokenAsync2_ShouldReturnAccessToken_WhenSuccess()
    {
        // Arrange
        var expectedToken = "abc123";

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new TokenResponse
            {
                AccessToken = expectedToken
            })
        };

        _mockhttpMessage
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
               "SendAsync",
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>()
           )
           .ReturnsAsync(response);


        _mockConfig.Setup(x => x["TokenApi:ClientId"]).Returns("fake-Client");
        _mockConfig.Setup(x => x["TokenApi:ClientSecret"]).Returns("fake-ClientSecret");
        _mockConfig.Setup(x => x["TokenApi:GrantType"]).Returns("fake-GrantType");
        _mockConfig.Setup(x => x["TokenApi:Scope"]).Returns("fake-Scope");

        // Act
        var result = await _service.GetTokenAsync();

        // Assert
        Assert.Equal(expectedToken, result);
    }

    [Fact]
    public async Task GetTokenAsync_ShouldThrow_WhenApiFail()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);

        _mockhttpMessage
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                   "SendAsync",
                   ItExpr.IsAny<HttpRequestMessage>(),
                   ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(response);

        _mockConfig.Setup(x => x["TokenApi:ClientId"]).Returns("client");
        _mockConfig.Setup(x => x["TokenApi:ClientSecret"]).Returns("secret");

        // Assert & Act
        await Assert.ThrowsAsync<Exception>(() => _service.GetTokenAsync());
    }
}
