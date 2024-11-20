using HyperhireWebAPI.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace HyperhireWebAPI.Domain.Entities;

public class ProductDetail : AuditableEntity
{
    public string Name { get; set; }

    public Guid CategoryId { get; set; }

    public decimal OriginalPrice { get; set; }

    public decimal Price { get; set; }

    public int MaxNight { get; set; }

    public int MaxGuests { get; set; }

    public List<string>? ImgUrl { get; set; }

    public string? Location { get; set; }

    public string? Decription { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public virtual Category Category { get; set; } = null!;
}
