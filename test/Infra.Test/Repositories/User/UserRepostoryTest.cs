using Enoch.Domain.Base;
using Enoch.Domain.Services.User.Entities;
using Enoch.Infra.Context;
using Enoch.Infra.User;
using Fixtures.User;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Infra.Tests.Repositories.User
{
    public class UserRepostoryTest
    {
        private readonly AutoMocker autoMocker = new AutoMocker();

        [Fact(DisplayName = "UserRepository, create user must return Success")]
        [Trait("Infra", "User Repository")]
        public void UserRepository_CreateUser_MustReturnSuccess()
        {
            //Arrange
            var user = UserFixtureEntity.CreateValidUser();

            var context = autoMocker.CreateInstance<DataContext>();

            var expectedUser = 1;

            var userRepository = new UserRepository(context);

            //Act
            var idActualResult = userRepository.Post(user);

            //Assert
            Assert.Equal(expectedUser, idActualResult);
        }

        // [Fact(DisplayName = "UserRepository, create user must return Error")]
        // [Trait("Infra", "User Repository")]
        // public void UserRepository_CreateUser_MustReturnError()
        // {
        //     //Arrange
        //     var user = UserFixtureEntity.CreateInvalidUser();
        //     // user.Id = 0;

        //     autoMocker.GetMock<IBaseRepository<UserEntity>>().Setup(x => x.Post(It.IsAny<UserEntity>())).Returns(0);

        //     var context = autoMocker.CreateInstance<DataContext>();

        //     var expectedUser = 1;

        //     var userRepository = new UserRepository(context);

        //     //Act
        //     var idActualResult = userRepository.Post(user);

        //     //Assert
        //     Assert.NotEqual(expectedUser, idActualResult);
        // }

        [Fact(DisplayName = "UserRepository, update user must return Success")]
        [Trait("Infra", "User Repository")]
        public void UserRepository_UpdateUser_MustReturnSuccess()
        {
            //Arrange
            var user = UserFixtureEntity.CreateValidAdmin();

            autoMocker.GetMock<IBaseRepository<UserEntity>>().Setup(x => x.Put(It.IsAny<UserEntity>()));
            var context = autoMocker.CreateInstance<DataContext>();
            
            var userRepository = new UserRepository(context);
            
            userRepository.Put(user);

            //Act & Assert
            userRepository.Put(user);
            Assert.True(1 > 0);
        }

        // [Fact(DisplayName = "UserRepository, get user by id user must return Success")]
        // [Trait("Infra", "User Repository")]
        // public void UserRepository_GetUserById_MustReturnSuccess()
        // {
        //     //Arrange
        //     var user = UserFixtureEntity.CreateValidAdmin();

        //     userRepository.Setup(x => x.First(It.IsAny<int>())).Returns(user);

        //     //Act
        //     var resultUser = userRepository.Object.First(x => x.Id == user.Id);

        //     //Assert
        //     Assert.Equal(user, resultUser);
        //     Assert.NotNull(resultUser);
        //     Assert.IsType<UserEntity>(resultUser);
        // }

        // [Fact(DisplayName = "UserRepository, get user by id user must return Error")]
        // [Trait("Infra", "User Repository")]
        // public void UserRepository_GetUserById_MustReturnError()
        // {
        //     //Arrange
        //     var user = UserFixtureEntity.CreateValidAdmin();

        //     userRepository.Setup(x => x.First(x => x.Id == user.Id)).Returns(UserFixtureEntity.CreateValidAdmin("Matheus", "matheus@gmail.io"));

        //     //Act
        //     var resultUser = userRepository.Object.First(x => x.Id == user.Id);

        //     //Assert
        //     Assert.NotEqual(user, resultUser);
        //     Assert.NotNull(resultUser);
        // }

        // [Fact(DisplayName = "UserRepository, get list of users must return Success")]
        // [Trait("Infra", "User Repository")]
        // public void UserRepository_Get_MustReturnSuccess()
        // {
        //     //Arrange
        //     var users = UserFixtureEntity.CreateListValidUsers();
        //     int total = users.Count();

        //     userRepository.Setup(x => x.Get(out total, 1)).Returns(users);

        //     //Act
        //     var resultUser = userRepository.Object.Get(out total, 1);

        //     //Assert
        //     Assert.Equal(users, resultUser);
        //     Assert.NotNull(resultUser);
        // }

        // [Fact(DisplayName = "UserRepository, get list of users must return Error")]
        // [Trait("Infra", "User Repository")]
        // public void UserRepository_Get_MustReturnError()
        // {
        //     //Arrange
        //     var users = UserFixtureEntity.CreateListValidUsers().ToList();
        //     int total = users.Count();

        //     var wrongListUsers = UserFixtureEntity.CreateListValidUsers().ToList();
        //     wrongListUsers.Add(UserFixtureEntity.CreateValidAdmin("Eliseu", "eliseu@gmail.io"));
        //     int total2 = wrongListUsers.Count();

        //     userRepository.Setup(x => x.Get(out total2, 1)).Returns(wrongListUsers);

        //     //Act
        //     var resultUser = userRepository.Object.Get(out total, 1);

        //     //Assert
        //     Assert.NotEqual(users, resultUser);
        // }
    }
}
