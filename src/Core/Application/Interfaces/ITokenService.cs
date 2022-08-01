﻿using Application.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> EncodeStringAsync(string stringToEncode);
        Task<string> DecodeStringAsync(string stringToDecode);

        Task<AuthenticationModel> GeneratePasswordResetTokenAsync(string email);
        Task<JwtSecurityToken> GenerateJWToken(AppUser appUser);
        Task<UserToken> GenerateRefreshTokenAsync(string ipAddress, string userId);
        Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token);
        Task<RefreshTokens> RefreshAsync(string accessToken, string refreshToken, string ipAddress);


        Task<UserToken> CommitAsync(UserToken refreshToken);
        Task<UserToken> GetByIdAsync(Guid Id);
        Task<UserToken> GetByUserIdAsync(string userId);

        Task CreateAsync(UserToken refreshToken);
        Task UpdateAsync(UserToken refreshToken);
        Task DeleteAsync(UserToken refreshToken);

        Task SaveAsync();
    }
}
