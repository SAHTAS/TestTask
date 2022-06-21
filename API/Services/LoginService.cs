using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Helpers;
using Core.Exceptions;
using DataAccess.Repositories;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUsersRepository usersRepository;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly JwtOptions jwtOptions;

        public LoginService(IUsersRepository usersRepository, IPasswordHasher<User> passwordHasher, IOptions<JwtOptions> jwtOptions)
        {
            this.usersRepository = usersRepository;
            this.passwordHasher = passwordHasher;
            this.jwtOptions = jwtOptions.Value;
        }
        
        public async Task<string> GenerateTokenAsync(string login, string password)
        {
            var user = await usersRepository.GetByLoginAsync(login);
            if (user == null)
                throw new AuthenticationException($"User '{login}' not found");

            var checkPasswordResult = passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (checkPasswordResult == PasswordVerificationResult.Failed)
                throw new AuthenticationException("Invalid password");

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserGroup.Code.ToString())
            };

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(jwtOptions.ExpirationInMinutes)),
                signingCredentials: jwtOptions.SigningCredentials,
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            return accessToken;
        }
    }
}