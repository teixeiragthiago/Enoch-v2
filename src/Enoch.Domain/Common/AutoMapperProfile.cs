using AutoMapper;
using Enoch.Domain.Services.User.Dto;
using Enoch.Domain.Services.User.Entities;

namespace Enoch.Domain.Common
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserEntity, UserDto>();
            CreateMap<UserDto, UserEntity>();
        }
    }
}
