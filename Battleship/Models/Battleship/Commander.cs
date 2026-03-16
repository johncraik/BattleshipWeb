using System.ComponentModel.DataAnnotations;
using JC.Core.Models.Auditing;

namespace Battleship.Models.Battleship;

public class Commander : AuditModel
{
    [Key]
    [MaxLength(38)]
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    
    public string? ImgPath { get; set; }
    
    public bool IsEnabled { get; set; }
    
    public ICollection<Ship> Ships { get; set; }

    public const ushort ShipCount = 6;
}