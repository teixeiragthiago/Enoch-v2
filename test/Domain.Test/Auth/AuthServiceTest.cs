using Enoch.CrossCutting;
using Enoch.CrossCutting.Notification;
using Enoch.Domain.Services.User;
using Enoch.Domain.Services.User.Entities;
using Fixtures.User;
using Microsoft.Extensions.Configuration;
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


        [Fact(DisplayName = "AuthService, GenerateToken must return Token with Success")]
        [Trait("Domain", "User Service")]
        public void AuthService_GenerateToken_MustReturnTokenWithSuccess()
        {
            //Arrange

            userRepository.Setup(x => x.First(It.IsAny<Expression<Func<UserEntity, bool>>>())).Returns(UserFixtureEntity.CreateValidUser());

            //Act

            //Assert
        }
    }
}
