﻿using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;
using Application.Helpers;
using Microsoft.IdentityModel.Tokens;
using Domain.Settings;
using Microsoft.Extensions.Options;

namespace Application.Features.Account.Commands.Authenticate
{
    public class AuthenticationCommand : IRequest<AuthenticationNewModel>
    {
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string IpAddress { get; set; }
    }


    internal class AuthenticationRequestCommandHandler : IRequestHandler<AuthenticationCommand, AuthenticationNewModel>
    {
        private readonly ILogger<AuthenticationRequestCommandHandler> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IRepositoryWrapper _repository;
        private readonly JWTSettings _jwtSettings;
        private readonly IMapper _mapper;


        public AuthenticationRequestCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<AuthenticationRequestCommandHandler> logger, UserManager<AppUser> userManager, IOptions<JWTSettings> jwtSettings)
        {
            _userManager = userManager;
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
        }


        public async Task<AuthenticationNewModel> Handle(AuthenticationCommand command, CancellationToken cancellationToken)
        {
            var loginModel = _mapper.Map<LoginModel>(command);

            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            if (user == null) throw new ApiException($"No Accounts Registered with {loginModel.Email}.");

            var authenticationSucceeded = await _userManager.CheckPasswordAsync(user, command.Password);

            if (!authenticationSucceeded) throw new ApiException($"Invalid Credentials for '{loginModel.Email}'.");

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            if (!isEmailConfirmed) throw new ApiException($"Account Not Confirmed for '{loginModel.Email}'.");


            var jwtSecurityToken = await GenerateJWToken(user);
            var authenticationModel = new AuthenticationNewModel
            {
                Id = user.Id,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName
            };

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            authenticationModel.Roles = rolesList.ToList();
            authenticationModel.IsVerified = user.EmailConfirmed;
            var refreshToken = GenerateRefreshToken(command.IpAddress);
            authenticationModel.RefreshToken = refreshToken.Token;

            return authenticationModel;
        }




        private async Task<JwtSecurityToken> GenerateJWToken(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }


        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
    }


}