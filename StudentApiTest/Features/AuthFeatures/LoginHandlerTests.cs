using Microsoft.Extensions.Configuration;
using NSubstitute;
using Shouldly;
using StudentApi.Features.AuthFeatures.Login;
using Xunit;

namespace StudentApiTest.Features.AuthFeatures
{
    public class LoginHandlerTests
    {
        private readonly IConfiguration _configuration;
        private readonly LoginHandler _handler;

        public LoginHandlerTests()
        {
            _configuration = Substitute.For<IConfiguration>();
            _handler = new LoginHandler(_configuration);
        }

        [Fact]
        public async Task Handle_InvalidCredentials_ShouldReturn401()
        {
            // Arrange
            var request = new LoginRequestModel { Username = "wrong", Password = "user" };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.status.ShouldBe(401);
            result.message.ShouldBe("Invalid username or password.");
        }

        [Fact]
        public async Task Handle_ValidCredentials_ShouldReturnToken()
        {
            // Arrange
            var request = new LoginRequestModel { Username = "admin", Password = "password123" };
            
            _configuration["Jwt:Issuer"].Returns("https://studenttest.com");
            _configuration["Jwt:audience"].Returns("https://studenttest.com");
            _configuration["Jwt:Key"].Returns("rtoiete4564rtfdoodsjfs4kwrewkere324");
            _configuration["Jwt:MobileAppExpiryMinutes"].Returns("60");

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.status.ShouldBe(200);
            result.message.ShouldBe("Login successful.");
            result.Token.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async Task Handle_MissingJwtKey_ShouldReturn500()
        {
            // Arrange
            var request = new LoginRequestModel { Username = "admin", Password = "password123" };
            _configuration["Jwt:Key"].Returns((string)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.status.ShouldBe(500);
            result.message.ShouldBe("JWT Key is not configured properly.");
        }
    }
}
