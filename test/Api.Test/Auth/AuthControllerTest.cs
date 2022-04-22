using Enoch.Api.Controllers;
using Enoch.CrossCutting.Notification;
using Enoch.Domain.Services.Auth;
using Enoch.Domain.Services.Auth.Dto;
using Fixtures.Auth;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Api.Tests.Auth
{
    public  class AuthControllerTest
    {
        private readonly Mock<IAuthService> authService = new Mock<IAuthService>();
        private readonly Mock<INotification> notification = new Mock<INotification>();

        [Fact(DisplayName = "AuthController, Authenticate user must return Token with Success")]
        [Trait("Api", "User Controller")]
        public void UserController_Post_MustReturnOk()
        {
            //Arrange
            var authData = AuthFixtureDto.CreateAuthData();

            authService.Setup(x => x.GenerateToken(It.IsAny<AuthDto>())).Returns(AuthFixtureDto.CreateValidToken());
            var authController = new AuthController(authService.Object, notification.Object);

            //Act
            var token = authController.Authenticate(authData) as OkObjectResult;

            //Assert
            Assert.Equal(((int)HttpStatusCode.OK), token.StatusCode);
        }

        [Fact(DisplayName = "AuthController, Authenticate user must return Error")]
        [Trait("Api", "User Controller")]
        public void UserController_Post_MustReturnBadRequest()
        {
            //Arrange
            var authData = AuthFixtureDto.CreateInvalidAuthData();

            authService.Setup(x => x.GenerateToken(It.IsAny<AuthDto>())).Returns(value: null);
            var authController = new AuthController(authService.Object, notification.Object);

            //Act
            var token = authController.Authenticate(authData) as BadRequestObjectResult;

            //Assert
            Assert.Equal(((int)HttpStatusCode.BadRequest), token.StatusCode);
        }
    }
}
