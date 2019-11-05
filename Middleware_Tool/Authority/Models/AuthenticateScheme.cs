﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.Auth.Models
{
    public class AuthenticateScheme
    {
        /// <summary>
        /// 客户端携带的 Token 不是有效的 Jwt 令牌，将不能被解析
        /// </summary>
        public string TokenEbnormal { get; set; } = "登录验证失败!";

        /// <summary>
        /// 令牌解码后，issuer 或 audience不正确
        /// </summary>
        public string TokenIssued { get; set; } = "登录验证失败!";

        /// <summary>
        /// 用户所属的角色中，均无访问API的权限，即无访问此API的权限
        /// </summary>
        public string NoPermissions { get; set; } = "登录验证失败!";
    }
}