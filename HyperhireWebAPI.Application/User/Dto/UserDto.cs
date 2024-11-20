using AutoMapper;
using HyperhireWebAPI.Application.Common.Mapping;
using HyperhireWebAPI.Domain.Entities;

namespace HyperhireWebAPI.Application.User.Dto;

public class UserDto : IMapFrom<HyperhireWebAPI.Domain.Entities.User>
{
    public string UserName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<HyperhireWebAPI.Domain.Entities.User, UserDto>();
    }
}