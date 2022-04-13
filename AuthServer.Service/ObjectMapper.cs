using AutoMapper;
using System;

namespace AuthServer.Service
{
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        {

            var config = new MapperConfiguration(cfg=>
                cfg.AddProfile<DtoMapper>()
            );

            return config.CreateMapper(); 

            // lazy method Projet run olunanda Memorye yazilmir.
        });

        public static IMapper Mapper => lazy.Value;
    }
}
