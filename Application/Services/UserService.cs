using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Services;
using DataAccess.Repositories;
using Domain;
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
                if (await usersRepository.DoesLoginExistsAsync(login))
                    ThrowUserAlreadyExistsException(login);

                await Task.Delay(TimeSpan.FromSeconds(5));

                var userGroupId = await userGroupsRepository.GetUserGroupIdAsync();
                var userStateId = await userStatesRepository.GetActiveStateIdAsync();

                var user = new User
                {
                    Login = login,
                    CreatedDate = DateTime.UtcNow,
                    UserGroupId = userGroupId,
                    UserStateId = userStateId
                };
                user.Password = passwordHasher.HashPassword(user, password);

                await usersRepository.CreateAsync(user);
                
                return user.UserId;
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
            await usersRepository.DeleteAsync(userId, blockedStateId);
        }

        private static void ThrowUserAlreadyExistsException(string login) =>
            throw new UserAlreadyExistsException($"User with login '{login}' already exists.");

        private static void ThrowInvalidUserId(int userId) =>
            throw new UserNotFoundException($"Invalid userId '{userId}'");

        private static void ThrowUserNotFoundException(int userId) =>
            throw new UserNotFoundException($"User with id '{userId}' is not found");
    }
}