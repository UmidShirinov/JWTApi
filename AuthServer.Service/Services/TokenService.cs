using AuthServer.Core.Configuration;
using AuthServer.Core.DTOS;
using AuthServer.Core.Model;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class TokenService : ITokenService
    {
        public UserManager<UserApp> _userManager { get; set; }
        public CustomTokenOption _tokenOption { get; set; }



        public TokenService(UserManager<UserApp> _userManager, IOptions<CustomTokenOption> _tokenOption)
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

        //register olanda bu claimler
        private IEnumerable<Claim> GetClaims(UserApp userApp, List<string> audiences)
        {
            var userlist = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,userApp.Id),
                new Claim(JwtRegisteredClaimNames.Email ,userApp.Email),
                new Claim(ClaimTypes.Name,userApp.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            //userlist.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            foreach (var item in audiences)
            {
                userlist.Add(new Claim(JwtRegisteredClaimNames.Aud, item));
            }

            return userlist;
        }

        //register olmuyanda bu claim isdifade olunacaq
        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>();
            foreach (var item in client.Audience)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Aud, item));

            }
            claims.Add (new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, client.ClientId.ToString()));


            return claims;

        }
         
        public TokenDto CreateToken(UserApp userApp)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.RefreshTokenExpiration);

            var securityKey = SignService.GetSecurityKey(_tokenOption.SecurityKey);

            SigningCredentials signingCredential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenOption.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                signingCredentials: signingCredential,
                claims:GetClaims(userApp,_tokenOption.Audience));

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessTo = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };

            return tokenDto;

        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);

            var securityKey = SignService.GetSecurityKey(_tokenOption.SecurityKey);

            SigningCredentials signingCredential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenOption.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                signingCredentials: signingCredential,
                claims: GetClaimsByClient(client));

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            var clientTokenDto = new ClientTokenDto
            {
                AccessToken = token,
                AccessTokenExpiration = accessTokenExpiration,
            };

            return clientTokenDto;
        }
    }
}
