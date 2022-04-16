﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public static class SignService
    {
        public static SecurityKey GetSecurityKey(string securityKet)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKet));
        }
    }
}
