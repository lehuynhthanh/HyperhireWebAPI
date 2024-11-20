using HyperhireWebAPI.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace HyperhireWebAPI.Domain.Entities;

public class User : AuditableEntity
{
    public string UserName { get; set; }

    public string Password { get; set; }
}
