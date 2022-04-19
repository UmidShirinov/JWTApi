using AuthServer.Core.Configuration;
using AuthServer.Core.DTOS;
using AuthServer.Core.Model;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SharedLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class TokenService : ITokenService
    {
        public UserManager<UserApp> _userManager { get; set; }
        public CustomTokenOption _tokenOption { get; set; }



        public TokenService(UserManager<UserApp> _userManager,IOptions<CustomTokenOption> _tokenOption)
        {
            this._tokenOption = _tokenOption.Value;
            this._userManager = _userManager;

        }



        // Random  stringin alinmasi.
        private string CreateRefreshToken()
        {
            var numberByte = new Byte[32];

            var rndm = RandomNumberGenerator.Create();

            rndm.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);

        }
        public TokenDto CreateToken(UserApp userApp)
        {
            throw new NotImplementedException();

            
        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            throw new NotImplementedException();
        }
    }
}
