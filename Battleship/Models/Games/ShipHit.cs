using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Battleship.Models.Battleship;
using JC.Core.Models.Auditing;
using Microsoft.EntityFrameworkCore;

namespace Battleship.Models.Games;

[PrimaryKey(nameof(ShipPlacementId), nameof(ShipCellId))]
public class ShipHit : AuditModel
{
    [Required]
    [MaxLength(38)]
    public string ShipPlacementId { get; set; }
    [ForeignKey(nameof(ShipPlacementId))]
    public ShipPlacement ShipPlacement { get; set; }
    
    [Required]
    [MaxLength(38)]
    public string ShipCellId { get; set; }
    [ForeignKey(nameof(ShipCellId))]
    public ShipCell ShipCell { get; set; }
    
    [NotMapped]
    public string GameId => ShipPlacement?.GameId ?? string.Empty;
}