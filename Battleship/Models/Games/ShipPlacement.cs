using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Battleship.Models.Battleship;
using Microsoft.EntityFrameworkCore;

namespace Battleship.Models.Games;

[PrimaryKey(nameof(ShipId), nameof(GameId))]
public class ShipPlacement
{
    [Required]
    [MaxLength(38)]
    public string ShipId { get; set; }
    [ForeignKey(nameof(ShipId))]
    public Ship Ship { get; set; }
    
    [Required]
    [MaxLength(38)]
    public string GameId { get; set; }
    [ForeignKey(nameof(GameId))]
    public Game Game { get; set; }
    
    //NOTE: Ideally service layer will ensure ship cells are loaded
    [NotMapped]
    public ShipCellDto SourceCell => Ship.SourceCell();
    
    public ShipRotation Rotation { get; set; } = ShipRotation.SOUTH;
    
    [Range(0, 15)]
    public ushort X { get; set; }
    
    [Range(0, 15)]
    public ushort Y { get; set; }
    
    [NotMapped]
    public string? PlayerId => Game?.Player1CommanderId == Ship?.CommanderId 
        ? Game?.Player1Id 
        : Game?.Player2CommanderId == Ship?.CommanderId 
            ? Game?.Player2Id 
            : null;
}

public enum ShipRotation
{
    /// <summary>
    /// Ship facing North. Cell matrix rotated by 180 degrees
    /// </summary>
    NORTH,
    
    /// <summary>
    /// Ship facing East. Cell matrix rotated 270 degrees
    /// </summary>
    EAST,
    
    /// <summary>
    /// Ship facing South. Cell matrix rotated 0 degrees
    /// </summary>
    SOUTH,
    
    /// <summary>
    /// Ship facing West. Cell matrix rotated 90 degrees
    /// </summary>
    WEST
}