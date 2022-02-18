using System.Net;
using Enoch.Api.Controllers;
using Enoch.CrossCutting.Notification;
using Enoch.Domain.Services.User;
using Fixtures.User;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Tests.User
{
    public class UserControllerTest
    {
        private readonly Mock<IUserService> userService = new Mock<IUserService>();
        private readonly Mock<INotification> notification = new Mock<INotification>();

        [Fact(DisplayName = "UserController, create user must return Ok")]
        [Trait("Api", "User Controller")]
        public void UserController_Post_MustReturnOk()
        {
           //Arrange
           var user = UserFixtureDto.CreateValidAdmin();

           userService.Setup(x => x.Post(user)).Returns(user.Id);
           var userController = new UserController(userService.Object, notification.Object);

           //Act
           var response = userController.Post(user) as OkObjectResult;

           //Assert
           Assert.Equal(((int)HttpStatusCode.OK), response.StatusCode);
        }

        [Fact(DisplayName = "UserController, create user must return Bad Request")]
        [Trait("Api", "User Controller")]
        public void UserController_Post_MustReturnBadRequest()
        {
           //Arrange
           var user = UserFixtureDto.CreateInvalidAdmin();
        
           userService.Setup(x => x.Post(user)).Returns(0);
           var userController = new UserController(userService.Object, notification.Object);

           //Act
           var response = userController.Post(user) as BadRequestObjectResult;
            
           //Assert
           Assert.Equal(((int)HttpStatusCode.BadRequest), response.StatusCode);
        }

        //[Fact(DisplayName = "UserController, update user must return Ok")]
        //[Trait("Api", "User Controller")]
        //public void UserController_Put_MustReturnOk()
        //{
        //    //Arrange
        //    var user = UserFixtureDto.CreateValidAdmin();

        //    userService.Setup(x => x.Post(user)).Returns(true);
        //    var userController = new UserController(userService.Object, notification.Object);

        //    //Act
        //    var response = userController.Post(user) as OkResult;

        //    //Assert
        //    Assert.Equal(((int)HttpStatusCode.OK), response.StatusCode);
        //}

        //[Fact(DisplayName = "UserController, update user must return Bad Request")]
        //[Trait("Api", "User Controller")]
        //public void UserController_Put_MustReturnBadRequest()
        //{
        //    //Arrange
        //    var user = UserFixtureDto.CreateInvalidAdmin();

        //    userService.Setup(x => x.Put(user)).Returns(false);
        //    var userController = new UserController(userService.Object, notification.Object);

        //    //Act
        //    var response = userController.Put(user) as BadRequestObjectResult;

        //    //Assert
        //    Assert.Equal(((int)HttpStatusCode.BadRequest), response.StatusCode);
        //}

        //[Fact(DisplayName = "UserController, delete user must return Ok")]
        //[Trait("Api", "User Controller")]
        //public void UserController_Delete_MustReturnOk()
        //{
        //    //Arrange
        //    var user = UserFixtureDto.CreateValidAdmin();

        //    userService.Setup(x => x.Delete(user.Id)).Returns(user.Id);
        //    var userController = new UserController(userService.Object, notification.Object);

        //    //Act
        //    var response = userController.Delete(user.Id) as OkResult;

        //    //Assert
        //    Assert.Equal(((int)HttpStatusCode.OK), response.StatusCode);
        //}

        //[Fact(DisplayName = "UserController, delete user must return Bad Request")]
        //[Trait("Api", "User Controller")]
        //public void UserController_Delete_MustReturnBadRequest()
        //{
        //    //Arrange
        //    var user = UserFixtureDto.CreateInvalidAdmin();

        //    userService.Setup(x => x.Delete(0)).Returns(0);
        //    var userController = new UserController(userService.Object, notification.Object);

        //    //Act
        //    var response = userController.Delete(0) as BadRequestObjectResult;

        //    //Assert
        //    Assert.Equal(((int)HttpStatusCode.BadRequest), response.StatusCode);
        //}
    }
}
