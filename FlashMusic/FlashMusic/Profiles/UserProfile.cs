using AutoMapper;
using FlashMusic.Models;
using FlashMusic.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserAddDto, User>()
                .AfterMap((src, dest) =>
                {
                    dest.Avatar = Constant.DefaultPicUrl;
                });
        }
    }
}
