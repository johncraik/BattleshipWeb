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

    [NotMapped]
    public CellCoordinate HitCell => new (X(), Y());
    
    private ushort X()
    {
        if(ShipCell == null! || ShipPlacement == null! 
            || ShipPlacement.Ship == null!)
            throw new InvalidOperationException("ShipCell, ShipPlacement, or Ship is null");
        
        var diff = (ushort)(ShipCell.X - ShipPlacement.SourceCell.X);
        return ShipPlacement.Rotation switch
        {
            ShipRotation.NORTH or ShipRotation.WEST => (ushort)(ShipPlacement.X - diff),
            ShipRotation.EAST or ShipRotation.SOUTH => (ushort)(ShipPlacement.X + diff),
            _ => throw new InvalidOperationException("Invalid ship rotation")
        };
    }

    private ushort Y()
    {
        if(ShipCell == null! || ShipPlacement == null! 
                             || ShipPlacement.Ship == null!)
            throw new InvalidOperationException("ShipCell, ShipPlacement, or Ship is null");
        
        var diff = (ushort)(ShipCell.Y - ShipPlacement.SourceCell.Y);
        return ShipPlacement.Rotation switch
        {
            ShipRotation.NORTH or ShipRotation.EAST => (ushort)(ShipPlacement.Y - diff),
            ShipRotation.WEST or ShipRotation.SOUTH => (ushort)(ShipPlacement.Y + diff),
            _ => throw new InvalidOperationException("Invalid ship rotation")
        };
    }
}