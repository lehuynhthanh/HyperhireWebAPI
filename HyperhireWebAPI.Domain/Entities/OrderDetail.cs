using HyperhireWebAPI.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace HyperhireWebAPI.Domain.Entities;

public class OrderDetail : AuditableEntity
{
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
    [ForeignKey(nameof(ProductId))]
    public virtual ProductDetail Products { get; set; }
    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }
}
