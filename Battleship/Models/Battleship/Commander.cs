using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Battleship.Models.Games;
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
    
    [NotMapped]
    public bool IsValid => (Ships?.Count ?? 0) == ShipCount;
    
    public ICollection<Ship> Ships { get; set; }
    
    [InverseProperty(nameof(Game.Player1Commander))]
    public ICollection<Game> Player1Games { get; set; }
    
    [InverseProperty(nameof(Game.Player2Commander))]
    public ICollection<Game> Player2Games { get; set; }

    public const ushort ShipCount = 6;
}