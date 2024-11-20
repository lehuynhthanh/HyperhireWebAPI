using AutoMapper;
using HyperhireWebAPI.Application.Common.Mapping;
using HyperhireWebAPI.Domain.Entities;

namespace HyperhireWebAPI.Application.ProductDetailApplication.Dto;

public class ProductDetailDto : IMapFrom<ProductDetail>
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public Guid CategoryId { get; set; }

    public decimal OriginalPrice { get; set; }

    public decimal Price { get; set; }

    public int MaxNight { get; set; }

    public int MaxGuests { get; set; }

    public List<string> ImgUrl { get; set; }

    public string Location { get; set; }

    public string Decription { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProductDetail, ProductDetailDto>();
    }
}