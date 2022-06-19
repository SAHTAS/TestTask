using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Core.Exceptions;
using Core.Services;
using DataAccess.Repositories;
using Domain;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Tests
{
    // Тестов маловато и причесать их как следует не успел.
    public class UserServiceTests
    {
        private Mock<IUsersRepository> usersRepositoryMock;
        private Mock<IUserGroupsRepository> userGroupsRepositoryMock;
        private Mock<IUserStatesRepository> userStatesRepositoryMock;
        private readonly IUserService userService;
        private List<User> userTestData;

        public UserServiceTests()
        {
            SetUpTestData();
            SetUpMocks();

            // Actually, should've mocked these two as well and write tests for LockService.
            var passwordHasher = new PasswordHasher<User>();
            var lockService = new LockService();

            userService = new UserService(usersRepositoryMock.Object, userGroupsRepositoryMock.Object,
                userStatesRepositoryMock.Object, passwordHasher, lockService);
        }
        
        [Theory]
        [InlineData(1, 1)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        public async void ShouldBeSuccess_GetUserAsync_WithValidId_UserExists(int userId, int desiredUserId)
        {
            var result = await userService.GetUserAsync(userId);
            Assert.Equal(desiredUserId, result.UserId);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(7)]
        [InlineData(18)]
        public async void ShouldBeFailed_GetUserAsync_WithValidId_UserDoesntExist(int userId)
        {
            await Assert.ThrowsAsync<UserNotFoundException>(async () => await userService.GetUserAsync(userId));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-15)]
        public async void ShouldBeFailed_GetUserAsync_WithInvalidId(int userId)
        {
            await Assert.ThrowsAsync<UserNotFoundException>(async () => await userService.GetUserAsync(userId));
        }

        [Fact]
        private async void ShouldBeSuccess_GetAllUsersAsync()
        {
            var users = await userService.GetAllUsersAsync();
            Assert.Equal(userTestData.Count, users.Count);
        }

        [Fact]
        private async void ShouldBeSuccess_CreateNewUserAsync()
        {
            var userId = await userService.CreateUserAsync("New", "User");
            Assert.Equal(userTestData.Last().UserId, userId);
        }
        
        [Fact]
        private async void ShouldBeFailed_CreateNewUserAsync_ThrowOnTryToCreateTwoUsersWithTheSameLogin()
        {
            var createUserTask = userService.CreateUserAsync("New", "User");
            await Assert.ThrowsAsync<UserAlreadyExistsException>(async () => await userService.CreateUserAsync("New", "User"));
            var userId = await createUserTask;
            Assert.Equal(userTestData.Last().UserId, userId);
        }
        
        [Theory]
        [InlineData("Admin")]
        [InlineData("Guy")]
        [InlineData("Man")]
        [InlineData("NotAdmin")]
        private async void ShouldBeFailed_CreateNewUserAsync_ThrowOnTryToCreateExistingUser(string login)
        {
            usersRepositoryMock.Setup(r => r.DoesLoginExistsAsync(login)).Returns(Task.FromResult(true));
            await Assert.ThrowsAsync<UserAlreadyExistsException>(async () => await userService.CreateUserAsync(login, "User"));
        }
        
        private void SetUpTestData()
        {
            userTestData = new List<User>
            {
                new User
                {
                    UserId = 1,
                    Login = "Admin",
                    CreatedDate = DateTime.UtcNow,
                    UserGroupId = 2,
                    UserStateId = 2,
                    UserGroup = new UserGroup
                    {
                        Code = UserGroupCode.Admin,
                        Description = ""
                    },
                    UserState = new UserState
                    {
                        Code = UserStateCode.Active,
                        Description = ""
                    }
                },
                new User
                {
                    UserId = 2,
                    Login = "Guy",
                    CreatedDate = DateTime.UtcNow,
                    UserGroupId = 1,
                    UserStateId = 2,
                    UserGroup = new UserGroup
                    {
                        Code = UserGroupCode.User,
                        Description = ""
                    },
                    UserState = new UserState
                    {
                        Code = UserStateCode.Active,
                        Description = ""
                    }
                },
                new User
                {
                    UserId = 3,
                    Login = "Man",
                    CreatedDate = DateTime.UtcNow,
                    UserGroupId = 1,
                    UserStateId = 2,
                    UserGroup = new UserGroup
                    {
                        Code = UserGroupCode.User,
                        Description = ""
                    },
                    UserState = new UserState
                    {
                        Code = UserStateCode.Active,
                        Description = ""
                    }
                },
                new User
                {
                    UserId = 4,
                    Login = "NotAdmin",
                    CreatedDate = DateTime.UtcNow,
                    UserGroupId = 1,
                    UserStateId = 2,
                    UserGroup = new UserGroup
                    {
                        Code = UserGroupCode.User,
                        Description = ""
                    },
                    UserState = new UserState
                    {
                        Code = UserStateCode.Active,
                        Description = ""
                    }
                }
            };
        }

        private void SetUpMocks()
        {
            usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock.Setup(r => r.GetAsync(It.IsAny<int>())).Returns<int>(x =>
                Task.FromResult(userTestData.FirstOrDefault(s => s.UserId == x)));
            usersRepositoryMock.Setup(r => r.GetAllAsync()).Returns(Task.FromResult(userTestData));
            usersRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<User>())).Returns<User>(user =>
            {
                user.UserId = userTestData.Count + 1;
                userTestData.Add(user);
                return Task.FromResult(user);
            });

            userGroupsRepositoryMock = new Mock<IUserGroupsRepository>();
            userStatesRepositoryMock = new Mock<IUserStatesRepository>();
        }
    }
}