using AutoMapper;
using OnlineStore.Core.File;
using OnlineStore.Core.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Bll.Mapper
{
    public static class MapperConfig
    {
        public static IMapper ConfigureAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Dal.Entities.File, FileModel>();
                cfg.CreateMap<Dal.Entities.User, UserModel>();
            });

            config.AssertConfigurationIsValid();

            return config.CreateMapper();
        }
    }
}
