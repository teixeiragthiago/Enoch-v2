﻿using AutoMapper;
using Enoch.CrossCutting.Notification;
using Enoch.Domain.Services.User;
using Enoch.Domain.Services.User.Dto;
using Enoch.Domain.Services.User.Entities;
using Fixtures.Mapper;
using Fixtures.User;
using Moq;
using MoqExpression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Domain.Tests.User
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
        private readonly Mock<INotification> notification = new Mock<INotification>();
        private readonly Mock<IUserFactory> userFactory = new Mock<IUserFactory>();
        private readonly Mock<IMapper> mapper = new Mock<IMapper>();

        [Fact(DisplayName = "UserService, create user must return Success")]
        [Trait("Domain", "User Service")]
        public void UserService_Post_MustReturnSuccess()
        {
            //Arrange
            var user = UserFixtureDto.CreateValidAdmin();

            var userEntity = DynamicMapper.MapTo<UserEntity>(user);

            userFactory.Setup(x => x.VerifyPassword(It.IsAny<string>())).Returns(true);
            userRepository.Setup(x => x.Post(It.IsAny<UserEntity>())).Returns(user.Id);

            var userService = new UserService(userRepository.Object, notification.Object, userFactory.Object);

            //Act
            var response = userService.Post(user);

            //Assert
            Assert.True(response > 0);
            Assert.True(user.IsValid(notification.Object));
            Assert.IsType<UserDto>(user);
        }

        [Fact(DisplayName = "UserService, create user must return Error")]
        [Trait("Domain", "User Service")]
        public void UserService_Post_MustReturnError()
        {
            //Arrange
            var invalidUser = UserFixtureDto.CreateInvalidAdmin();
            int idResult = 1;

            var userEntity = DynamicMapper.MapTo<UserEntity>(invalidUser);

            userRepository.Setup(x => x.Post(UserFixtureEntity.CreateInvalidAdmin())).Returns(idResult);
            userFactory.Setup(x => x.VerifyPassword(It.IsAny<string>())).Returns(false);

            var userService = new UserService(userRepository.Object, notification.Object, userFactory.Object);

            //Act
            var response = userService.Post(invalidUser);

            //Assert
            Assert.True(response <= 0);
            Assert.False(invalidUser.IsValid(notification.Object));
            Assert.IsType<UserDto>(invalidUser);
        }

        [Fact(DisplayName = "UserService, update user must return Success")]
        [Trait("Domain", "User Service")]
        public void UserService_Put_MustReturnSuccess()
        {
            //Arrange

            var user = UserFixtureDto.CreateValidAdmin();

            var userEntity = DynamicMapper.MapTo<UserEntity>(user);

            userRepository.Setup(x => x.Put(userEntity));
            userRepository.Setup(x => x.First(x => x.Id == user.Id)).Returns(userEntity);
            userFactory.Setup(x => x.VerifyPassword(It.IsAny<string>())).Returns(true);

            var userService = new UserService(userRepository.Object, notification.Object, userFactory.Object);

            //Act
            var response = userService.Put(user);

            //Assert
            Assert.True(response);
            Assert.True(user.IsValidUpdate(notification.Object));
            Assert.IsType<UserDto>(user);
        }

        [Fact(DisplayName = "UserService, update user must return Error")]
        [Trait("Domain", "User Service")]
        public void UserService_Put_MustReturnError()
        {
            //Arrange
            var user = UserFixtureDto.CreateInvalidAdmin();

            var userEntity = DynamicMapper.MapTo<UserEntity>(user);

            userRepository.Setup(x => x.Put(userEntity));
            userRepository.Setup(x => x.First(x => x.Id == user.Id)).Returns(userEntity);
            userFactory.Setup(x => x.VerifyPassword("senha")).Returns(false);

            var userService = new UserService(userRepository.Object, notification.Object, userFactory.Object);

            //Act
            var response = userService.Put(user);

            //Assert
            Assert.False(response);
            Assert.False(user.IsValidUpdate(notification.Object));
            Assert.IsType<UserDto>(user);
        }

        [Fact(DisplayName = "UserService, delete user must return Success")]
        [Trait("Domain", "User Service")]
        public void UserService_Delete_MustReturnSuccess()
        {
            //Arrange
            var user = UserFixtureDto.CreateInvalidAdmin();
            var userEntity = DynamicMapper.MapTo<UserEntity>(user);

            userRepository.Setup(x => x.First(MoqHelper.IsExpression<UserEntity>(x => x.Id == user.Id))).Returns(userEntity);
            userRepository.Setup(x => x.Delete(It.IsAny<UserEntity>()));

            var userService = new UserService(userRepository.Object, notification.Object, userFactory.Object);

            //Act
            var response = userService.Delete(user.Id);

            //Assert
            Assert.True(response);
        }
    }
}
