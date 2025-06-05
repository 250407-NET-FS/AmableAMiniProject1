// UserCommandTests.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using MiniProject.Models;
using MiniProject.Services;
using MiniProject.Services.Users.Commands;
using Xunit;
using AutoMapper;

namespace MiniProject.Tests
{

    public class UserCommandTests
    {

        public class RegisterUserTests
        {
            private readonly Mock<UserManager<User>> _mockUserManager;

            public RegisterUserTests()
            {

                _mockUserManager = new Mock<UserManager<User>>(
                    Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            }

            [Fact]
            public async Task Handle_WhenCreateSucceeds_ReturnsUserIdAsString()
            {
                // Arrange
                var newGuid = Guid.NewGuid();
                var command = new RegisterUser.RegisterCommand
                {
                    Email = "test@example.com",
                    Password = "P@ssw0rd!"
                };

                
                _mockUserManager
                    .Setup(x => x.CreateAsync(It.IsAny<User>(), command.Password))
                    .ReturnsAsync(IdentityResult.Success)
                    .Callback<User, string>((u, pwd) =>
                    {
                        // Ensure email/username match
                        Assert.Equal(command.Email, u.Email);
                        Assert.Equal(command.Email, u.UserName);
                        // Override the handler‐constructed user’s Id
                        u.Id = newGuid;
                    });

                var handler = new RegisterUser.Handler(_mockUserManager.Object);

                // Act
                var result = await handler.Handle(command, CancellationToken.None);

                // Assert
                Assert.Equal(newGuid.ToString(), result);
                _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<User>(), command.Password), Times.Once);
            }

            [Fact]
            public async Task Handle_WhenCreateFails_ThrowsExceptionWithConcatenatedErrors()
            {
                // Arrange
                var command = new RegisterUser.RegisterCommand
                {
                    Email = "bad@example.com",
                    Password = "weakpass"
                };

                var identityError1 = new IdentityError { Description = "Email already taken" };
                var identityError2 = new IdentityError { Description = "Password too weak" };

                _mockUserManager
                    .Setup(x => x.CreateAsync(It.IsAny<User>(), command.Password))
                    .ReturnsAsync(IdentityResult.Failed(identityError1, identityError2));

                var handler = new RegisterUser.Handler(_mockUserManager.Object);

                // Act & Assert
                var ex = await Assert.ThrowsAsync<Exception>(
                    () => handler.Handle(command, CancellationToken.None));

                Assert.Contains("Email already taken", ex.Message);
                Assert.Contains("Password too weak", ex.Message);
                _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<User>(), command.Password), Times.Once);
            }
        }

        public class LoginUserTests
        {
            private readonly Mock<UserManager<User>> _mockUserManager;
            private readonly Mock<IUserService> _mockUserService;

            public LoginUserTests()
            {
                _mockUserManager = new Mock<UserManager<User>>(
                    Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
                _mockUserService = new Mock<IUserService>();
            }

            [Fact]
            public async Task Handle_WhenUserNotFound_ReturnsNull()
            {
                // Arrange
                var command = new LoginUser.Command
                {
                    Email = "nonexistent@example.com",
                    Password = "doesntmatter"
                };

                _mockUserManager
                    .Setup(x => x.FindByEmailAsync(command.Email))
                    .ReturnsAsync((User)null);

                var handler = new LoginUser.Handler(_mockUserManager.Object, _mockUserService.Object);

                // Act
                var token = await handler.Handle(command, CancellationToken.None);

                // Assert
                Assert.Null(token);
                _mockUserManager.Verify(x => x.FindByEmailAsync(command.Email), Times.Once);
                _mockUserManager.Verify(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
                _mockUserService.Verify(x => x.GenerateToken(It.IsAny<User>()), Times.Never);
            }

            [Fact]
            public async Task Handle_WhenPasswordInvalid_ReturnsNull()
            {
                // Arrange
                var command = new LoginUser.Command
                {
                    Email = "test@example.com",
                    Password = "wrongpassword"
                };

                var existingUser = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "testuser",
                    Email = command.Email
                };

                _mockUserManager
                    .Setup(x => x.FindByEmailAsync(command.Email))
                    .ReturnsAsync(existingUser);

                _mockUserManager
                    .Setup(x => x.CheckPasswordAsync(existingUser, command.Password))
                    .ReturnsAsync(false);

                var handler = new LoginUser.Handler(_mockUserManager.Object, _mockUserService.Object);

                // Act
                var token = await handler.Handle(command, CancellationToken.None);

                // Assert
                Assert.Null(token);
                _mockUserManager.Verify(x => x.FindByEmailAsync(command.Email), Times.Once);
                _mockUserManager.Verify(x => x.CheckPasswordAsync(existingUser, command.Password), Times.Once);
                _mockUserService.Verify(x => x.GenerateToken(It.IsAny<User>()), Times.Never);
            }

            [Fact]
            public async Task Handle_WhenCredentialsValid_ReturnsJwtToken()
            {
                // Arrange
                var command = new LoginUser.Command
                {
                    Email = "valid@example.com",
                    Password = "CorrectP@ss"
                };

                var existingUser = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "validuser",
                    Email = command.Email
                };

                _mockUserManager
                    .Setup(x => x.FindByEmailAsync(command.Email))
                    .ReturnsAsync(existingUser);

                _mockUserManager
                    .Setup(x => x.CheckPasswordAsync(existingUser, command.Password))
                    .ReturnsAsync(true);

                var fakeJwt = "eyFakeHeader.FakePayload.FakeSignature";
                _mockUserService
                    .Setup(x => x.GenerateToken(existingUser))
                    .ReturnsAsync(fakeJwt);

                var handler = new LoginUser.Handler(_mockUserManager.Object, _mockUserService.Object);

                // Act
                var token = await handler.Handle(command, CancellationToken.None);

                // Assert
                Assert.Equal(fakeJwt, token);
                _mockUserManager.Verify(x => x.FindByEmailAsync(command.Email), Times.Once);
                _mockUserManager.Verify(x => x.CheckPasswordAsync(existingUser, command.Password), Times.Once);
                _mockUserService.Verify(x => x.GenerateToken(existingUser), Times.Once);
            }
        }
    }
}
