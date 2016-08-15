﻿using System;
using Microsoft.AspNetCore.Builder;
using zxm.AspNetCore.Authentication.Hmac.Identity;

namespace zxm.AspNetCore.Authentication.Hmac
{
    public class HmacOptions : AuthenticationOptions
    {
        public HmacOptions()
        {
            AuthenticationScheme = HmacDefaults.AuthenticationScheme;
            AutomaticAuthenticate = true;
            AutomaticChallenge = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public int RequestTimeInterval { get; set; } = 10;

        public Func<string, string> TryGetClientSecret { get; set; }
        
        public Func<string, HmacIdentity> VerifyUserAccessToken { get; set; }
    }
}
