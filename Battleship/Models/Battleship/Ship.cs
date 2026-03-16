using System.ComponentModel.DataAnnotations;
using JC.Core.Models.Auditing;

namespace Battleship.Models.Battleship;

public class Ship : AuditModel
{
    [Key]
    [MaxLength(38)]
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    
    [Required]
    public string Name { get; set; }
    
    public ushort Size { get; set; }
    
    public const ushort MinSize = 2;
    public const ushort MaxSize = 6;
    public const ushort MaxWidth = 3;
    public const ushort MaxHeight = 6;
}