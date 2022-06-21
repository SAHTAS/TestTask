using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Services;
using DataAccess.Repositories;
using Domain;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository usersRepository;
        private readonly IUserGroupsRepository userGroupsRepository;
        private readonly IUserStatesRepository userStatesRepository;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly ILockService lockService;

        public UserService(IUsersRepository usersRepository, 
            IUserGroupsRepository userGroupsRepository,
            IUserStatesRepository userStatesRepository, 
            IPasswordHasher<User> passwordHasher,
            ILockService lockService)
        {
            this.usersRepository = usersRepository;
            this.userGroupsRepository = userGroupsRepository;
            this.userStatesRepository = userStatesRepository;
            this.passwordHasher = passwordHasher;
            this.lockService = lockService;
        }

        public async Task<User> GetUserAsync(int userId)
        {
            if (userId <= 0)
                ThrowInvalidUserId(userId);

            var user =  await usersRepository.GetAsync(userId);
            if (user == null)
                ThrowUserNotFoundException(userId);

            return user;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await usersRepository.GetAllAsync();
        }

        public async Task<int> CreateUserAsync(string login, string password)
        {
            if (!lockService.TryLock(login, out var locker))
                ThrowUserAlreadyExistsException(login);

            using (locker)
            {
                var user = await usersRepository.GetByLoginIncludeBlockedAsync(login);
                if (user != null && user.UserState.Code == UserStateCode.Active)
                    ThrowUserAlreadyExistsException(login);

                await Task.Delay(TimeSpan.FromSeconds(5));

                var userGroupId = await userGroupsRepository.GetUserGroupIdAsync();
                var userStateId = await userStatesRepository.GetActiveStateIdAsync();

                var now = DateTime.UtcNow;
                var newUser = new User
                {
                    UserId = user?.UserId ?? 0,
                    Login = login,
                    CreatedDate = user?.CreatedDate ?? now,
                    BlockedDate = user?.BlockedDate,
                    LastUpdate = now,
                    UserGroupId = userGroupId,
                    UserStateId = userStateId
                };
                newUser.Password = passwordHasher.HashPassword(newUser, password);

                await usersRepository.CreateOrUpdateAsync(newUser);

                return newUser.UserId;
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            if (userId <= 1)
                ThrowInvalidUserId(userId);

            var user = await usersRepository.GetAsync(userId);
            if (user == null)
                ThrowUserNotFoundException(userId);

            var blockedStateId = await userStatesRepository.GetBlockedStateIdAsync();
            await usersRepository.DeleteAsync(userId, blockedStateId, DateTime.UtcNow);
        }

        private static void ThrowUserAlreadyExistsException(string login) =>
            throw new UserAlreadyExistsException($"User with login '{login}' already exists.");

        private static void ThrowInvalidUserId(int userId) =>
            throw new UserNotFoundException($"Invalid userId '{userId}'");

        private static void ThrowUserNotFoundException(int userId) =>
            throw new UserNotFoundException($"User with id '{userId}' is not found");
    }
}