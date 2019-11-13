﻿using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.Core;

namespace Middleware_Tool
{
    public interface IJWTTokenService
    {
        string GetToken(JWTUserModel user);
    }

    public class JWTTokenService : IJWTTokenService
    {
        private readonly ServerJwtSetting _jwtSetting;

        public JWTTokenService()
        {
            _jwtSetting = new ServerJwtSetting
            {
                SecurityKey = "d0ecd23c-dvdb-5005-a2ka-0fea210c858a", // 密钥
                Issuer = "jwtIssuertest", // 颁发者
                Audience = "jwtAudiencetest", // 接收者
                ExpireSeconds = 86400 // 1t 过期时间
            };
        }

        public string GetToken(JWTUserModel user)
        {
            // 创建用户身份标识
            var claims = new Claim[]
            {
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString()),
                new Claim(JwtClaimTypes.Id, user.UserID.ToString(), ClaimValueTypes.Integer32),
                new Claim(JwtClaimTypes.Name, user.Name, ClaimValueTypes.String),
                new Claim(JwtClaimTypes.Role, user.Power_ID.ToString(), ClaimValueTypes.Integer32)
            };

            // 创建令牌
            var token = new JwtSecurityToken(
                    issuer: _jwtSetting.Issuer,
                    audience: _jwtSetting.Audience,
                    signingCredentials: _jwtSetting.Credentials,
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddSeconds(_jwtSetting.ExpireSeconds)
                );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}