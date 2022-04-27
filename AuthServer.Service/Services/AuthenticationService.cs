using AuthServer.Core.Configuration;
using AuthServer.Core.DTOS;
using AuthServer.Core.Model;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class AuthenticationService : IAutentiocationService
    {

        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepostiory<UserRefreshToken> _refreshToken;



        public AuthenticationService(IOptions<List<Client>> clientOptions, ITokenService tokenService, UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IGenericRepostiory<UserRefreshToken> genericRepostiory)
        {
            _clients = clientOptions.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _refreshToken = genericRepostiory;

        }

        public async Task<Response<TokenDto>> CreateAccessToken(LoginDto loginDto)
        {
            if (loginDto == null) throw new ArgumentException(nameof(loginDto));

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Response<TokenDto>.Failed("Email or Password is Wrong", 400, true);

            if (await _userManager.CheckPasswordAsync(user, loginDto.Password) == false)
            {
                return Response<TokenDto>.Failed("Email or Password is Wrong", 400, true);
            }
            var token = _tokenService.CreateToken(user);

            var userRefreshToken = await _refreshToken.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

            if (userRefreshToken == null)
            {
                await _refreshToken.AddAsync(new UserRefreshToken { UserId = user.Id, COde = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
            }
            else
            {
                userRefreshToken.COde = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration; 
            }

            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(token, 200);

        }

        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client = _clients.SingleOrDefault(x=>x.ClientId==clientLoginDto.ClientId && x.Secret==clientLoginDto.ClientSecret);

            if (client==null)
            {
                return Response<ClientTokenDto>.Failed("Client not Found", 404,true);
            }

            var token = _tokenService.CreateTokenByClient(client);

            return Response<ClientTokenDto>.Success(token,200);
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _refreshToken.Where(x => x.COde == refreshToken).SingleOrDefaultAsync();

            if (existRefreshToken==null)
            {
                return Response<TokenDto>.Failed("RefreshToken is not found", 404,true);
            }

            var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);

            if (user==null)
            {
                return Response<TokenDto>.Failed("RefreshToken is not found", 404, true);
            }

            var tokenDto = _tokenService.CreateToken(user);

            existRefreshToken.COde = tokenDto.RefreshToken;
            existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(tokenDto, 200);
        }

        public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            var existRefreshToken = await  _refreshToken.Where(x=>x.COde==refreshToken).SingleOrDefaultAsync();

            if (existRefreshToken==null)
            {
                return Response<NoDataDto>.Failed("RefreshToken is not found", 404, true);
            }

            _refreshToken.Remove(existRefreshToken);

            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success(200);
        }
    }
}
