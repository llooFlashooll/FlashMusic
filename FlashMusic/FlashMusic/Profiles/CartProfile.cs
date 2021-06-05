using AutoMapper;
using FlashMusic.Dtos;
using FlashMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartAddDto, Cart>();
            CreateMap<CartDelDto, Cart>();
        }
    }
}
