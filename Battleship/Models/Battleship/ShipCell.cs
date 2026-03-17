using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Battleship.Models.Games;
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
    [ForeignKey(nameof(ShipId))]
    public Ship Ship { get; set; }

    [Range(0, Ship.MaxWidth - 1)]
    public ushort X { get; set; }
    
    [Range(0, Ship.MaxHeight - 1)]
    public ushort Y { get; set; }
    
    public ICollection<ShipHit> GameHits { get; set; }
}

public record CellCoordinate(ushort X, ushort Y);