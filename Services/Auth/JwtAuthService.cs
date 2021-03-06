using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using BackendServiceStarter.Models.Options;
using BackendServiceStarter.Services.Auth.Exceptions;
using BackendServiceStarter.Services.Crypto;
using BackendServiceStarter.Services.Models;
using Microsoft.IdentityModel.Tokens;

namespace BackendServiceStarter.Services.Auth
{
    public class JwtAuthService : IAuthService
    {
        private readonly UserRepository _userRepository;
        private readonly IHashService _hashService;
        private readonly JwtAuthOptions _jwtAuthOptions;
        
        public JwtAuthService(UserRepository userRepository, IHashService hashService, JwtAuthOptions jwtAuthOptions)
        {
            _userRepository = userRepository;
            _hashService = hashService;
            _jwtAuthOptions = jwtAuthOptions;
        }
        
        public string GenerateToken(ClaimsIdentity identity)
        {
            var jwt = new JwtSecurityToken
            (
                issuer: _jwtAuthOptions.Issuer,
                audience: _jwtAuthOptions.Audience,
                notBefore: DateTime.Now,
                claims: identity.Claims,
                expires: DateTime.Now.AddMinutes(_jwtAuthOptions.Lifetime),
                signingCredentials: new SigningCredentials
                (
                    _jwtAuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha512
                )
            );

            var jwtHandler = new JwtSecurityTokenHandler();

            return jwtHandler.WriteToken(jwt);
        }

        public async Task<ClaimsIdentity> GetIdentity(string email, string password)
        {
            var user = await _userRepository.FindByEmail(email);
            
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            if (!_hashService.Verify(password, user.Password))
            {
                throw new UserHashVerifyException();
            }

            return new ClaimsIdentity
            (
                new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                },
                "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
            );
        }
    }
}