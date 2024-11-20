using HyperhireWebAPI.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace HyperhireWebAPI.Domain.Entities;

public class Category : AuditableEntity
{
    public string CategoryNameId { get; set; }

    public string Name { get; set; }

    public string Icon { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<ProductDetail>? Products { get; set; } = new List<ProductDetail>();
}
