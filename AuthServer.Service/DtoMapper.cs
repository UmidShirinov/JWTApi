using AuthServer.Core.DTOS;
using AuthServer.Core.Model;
using AutoMapper;

namespace AuthServer.Service
{
    public class DtoMapper:Profile
    {
        public DtoMapper()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<UserAppDto, UserApp>().ReverseMap();
        }

    }
}
