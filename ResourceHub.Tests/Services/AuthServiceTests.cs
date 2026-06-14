using Microsoft.Extensions.Configuration;
using Moq;
using ResourceHub.Core.Entities;
using ResourceHub.Core.Interfaces;
using ResourceHub.Infrastructure.Services;

namespace ResourceHub.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _configMock = new Mock<IConfiguration>();

            _configMock.Setup(c => c["Jwt:Key"])
                .Returns("SuperSecretJwtKey12345678901234567890");

            _configMock.Setup(c => c["Jwt:Issuer"])
                .Returns("ResourceHub");

            _configMock.Setup(c => c["Jwt:Audience"])
                .Returns("ResourceHubUsers");

            _service = new AuthService(
                _userRepositoryMock.Object,
                _configMock.Object);
        }


        // Test that registering a user with a new email creates the user and returns a token
        [Fact]
        public async Task RegisterAsync_ShouldCreateUser_WhenEmailDoesNotExist()
        {

            // Arrange
            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync("test@test.com"))
                .ReturnsAsync((User?)null);


            // Act
            var token = await _service.RegisterAsync(
                "nompumelelo",
                "test@test.com",
                "Password123");


            // Assert
            Assert.False(string.IsNullOrWhiteSpace(token));

            _userRepositoryMock.Verify(
                r => r.AddAsync(It.IsAny<User>()),
                Times.Once);

            _userRepositoryMock.Verify(
                r => r.SaveChangesAsync(),
                Times.Once);
        }


        // Test that registering a user with an existing email throws an ArgumentException
        [Fact]
        public async Task RegisterAsync_ShouldThrow_WhenEmailAlreadyExists()
        {
            // Arrange
            var existingUser = new User
            {
                Email = "test@test.com"
            };

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync("test@test.com"))
                .ReturnsAsync(existingUser);


            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.RegisterAsync(
                    "nompumelelo",
                    "test@test.com",
                    "Password123"));
        }


        // Test that logging in with valid credentials returns a token
        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var user = new User
            {
                Username = "nompumelelo",
                Email = "test@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123"),
                Role = "User"
            };

            _userRepositoryMock
                .Setup(r => r.GetByUsernameOrEmailAsync("nompumelelo"))
                .ReturnsAsync(user);


            // Act
            var token = await _service.LoginAsync(
                "nompumelelo",
                "Password123");


            // Assert
            Assert.False(string.IsNullOrWhiteSpace(token));
        }


        // Test that logging in with an invalid username or email throws 
        // an UnauthorizedAccessException
        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepositoryMock
                .Setup(r => r.GetByUsernameOrEmailAsync("unknown"))
                .ReturnsAsync((User?)null);


            // Act + Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.LoginAsync(
                    "unknown",
                    "Password123"));
        }


        // Test that looging in with an incorrect passwoword throws an UnathorizedAccessException
        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenPasswordIsIncorrect()
        {
            // Arrange
            var user = new User
            {
                Username = "nompumelelo",
                Email = "test@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword")
            };

            _userRepositoryMock
                .Setup(r => r.GetByUsernameOrEmailAsync("nompumelelo"))
                .ReturnsAsync(user);


            // Act + Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.LoginAsync(
                    "nompumelelo",
                    "WrongPassword"));
        }
    }
}