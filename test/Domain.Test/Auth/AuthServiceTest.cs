using Enoch.CrossCutting;
using Enoch.CrossCutting.Notification;
using Enoch.Domain.Services.Auth;
using Enoch.Domain.Services.User;
using Enoch.Domain.Services.User.Entities;
using Fixtures.Auth;
using Fixtures.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain.Tests.Auth
{
    public class AuthServiceTest
    {
        private readonly Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
        private readonly Mock<INotification> notification = new Mock<INotification>();
        private readonly Mock<IConfiguration> configuration = new Mock<IConfiguration>();
        private readonly Mock<IConfigurationSection> configurationSection = new Mock<IConfigurationSection>();


        [Fact(DisplayName = "AuthService, GenerateToken must return Token with Success")]
        [Trait("Domain", "Auth Service")]
        public void AuthService_GenerateToken_MustReturnTokenWithSuccess()
        {
            //Arrange
            var authDto = AuthFixtureDto.CreateAuthData();

            configurationSection.Setup(x => x.Value).Returns("SECRETKEYVALUETESTECOMVALORESALEATORIOS");
            configuration.Setup(x => x.GetSection("AppSettings:Secret")).Returns(configurationSection.Object);
            userRepository.Setup(x => x.First(It.IsAny<Expression<Func<UserEntity, bool>>>())).Returns(UserFixtureEntity.CreateValidUser());
            notification.Setup(x => x.Any()).Returns(false);

            var authService = new AuthService(userRepository.Object, notification.Object, configuration.Object);

            //Act
            var token = authService.GenerateToken(authDto);

            //Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token.Token);
            Assert.True(authDto.IsValid(notification.Object));
            userRepository.Verify(r => r.First(It.IsAny<Expression<Func<UserEntity, bool>>>()), Times.Once);
        }

        [Fact(DisplayName = "AuthService, GenerateToken must not return Token when e-mail is invalid")]
        [Trait("Domain", "Auth Service")]
        public void AuthService_GenerateToken_MustNotReturnTokenWhenEmailIsInvalid()
        {
            //Arrange
            var authDto = AuthFixtureDto.CreateInvalidAuthData();

            configurationSection.Setup(x => x.Value).Returns("SECRETKEYVALUETESTECOMVALORESALEATORIOS");
            configuration.Setup(x => x.GetSection("AppSettings:Secret")).Returns(configurationSection.Object);
            userRepository.Setup(x => x.First(It.IsAny<Expression<Func<UserEntity, bool>>>())).Returns(UserFixtureEntity.CreateValidUser());
            notification.Setup(x => x.Any()).Returns(true);

            var authService = new AuthService(userRepository.Object, notification.Object, configuration.Object);

            //Act
            var token = authService.GenerateToken(authDto);

            //Assert
            Assert.Null(token);
            Assert.Empty(authDto.Email);
            Assert.False(authDto.IsValid(notification.Object));
            userRepository.Verify(r => r.First(It.IsAny<Expression<Func<UserEntity, bool>>>()), Times.Once);
        }

        [Fact(DisplayName = "AuthService, GenerateToken must not return Token when user is null")]
        [Trait("Domain", "Auth Service")]
        public void AuthService_GenerateToken_MustNotReturnTokenWhenUserIsNull()
        {
            //Arrange
            var authDto = AuthFixtureDto.CreateAuthData();

            configurationSection.Setup(x => x.Value).Returns("SECRETKEYVALUETESTECOMVALORESALEATORIOS");
            configuration.Setup(x => x.GetSection("AppSettings:Secret")).Returns(configurationSection.Object);

            var authService = new AuthService(userRepository.Object, notification.Object, configuration.Object);

            //Act
            var token = authService.GenerateToken(authDto);

            //Assert
            Assert.Null(token);
        }

        [Fact(DisplayName = "AuthService, GenerateToken must not return Token when secret is invalid")]
        [Trait("Domain", "Auth Service")]
        public void AuthService_GenerateToken_MustNotReturnTokenWhenSecretIsInvalid()
        {
            //Arrange
            var authDto = AuthFixtureDto.CreateAuthData();

            configurationSection.Setup(x => x.Value).Returns(string.Empty);
            configuration.Setup(x => x.GetSection("AppSettings:Secret")).Returns(configurationSection.Object);

            userRepository.Setup(x => x.First(It.IsAny<Expression<Func<UserEntity, bool>>>())).Returns(UserFixtureEntity.CreateValidUser());

            var authService = new AuthService(userRepository.Object, notification.Object, configuration.Object);

            //Act
            var token = Assert.Throws<ArgumentException>(() => authService.GenerateToken(authDto));

            //Assert
            userRepository.Verify(r => r.First(It.IsAny<Expression<Func<UserEntity, bool>>>()), Times.Once);
            Assert.NotNull(token);
            Assert.NotEmpty(token.Message);
            Assert.Equal("IDX10703: Cannot create a 'System.RuntimeType', key length is zero.", token.Message);
        }
    }
}
