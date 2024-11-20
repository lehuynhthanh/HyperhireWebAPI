using AutoMapper;
using HyperhireWebAPI.Application.Common.Mapping;
using HyperhireWebAPI.Domain.Entities;

namespace HyperhireWebAPI.Application.CategoryApplication.Dto;

public class CategoryDto : IMapFrom<Category>
{
    public Guid Id { get; set; }

    public string CategoryNameId { get; set; }

    public string Name { get; set; }

    public string Icon { get; set; }

    public bool IsActive { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Category, CategoryDto>();
    }
}
