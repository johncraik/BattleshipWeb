using System.ComponentModel.DataAnnotations;
using JC.Core.Models.Auditing;

namespace Battleship.Models.Battleship;

public class ShipCell : AuditModel
{
    [Key]
    [MaxLength(38)]
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    
    [Required]
    [MaxLength(38)]
    public string ShipId { get; set; }
    public Ship
    
}