using AutoMapper;
using HyperhireWebAPI.Application.Common.Mapping;
using HyperhireWebAPI.Application.ProductDetailApplication.Dto;
using HyperhireWebAPI.Domain.Entities;

namespace HyperhireWebAPI.Application.OrderApplication.Dto;

public class OrderDto : IMapFrom<OrderDetail>
{
    public Guid Id { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int TotalNights { get; set; }
    public int TotalGuests { get; set; }
    public int TotalInfants { get; set; }
    public int TotalPets { get; set; }
    public string Phone { get; set; }
    public decimal ProductPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string OrderStatus { get; set; }
    public Guid ProductId { get; set; }
    public ProductDetailDto? Product { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<OrderDetail, OrderDto>();
    }
}
