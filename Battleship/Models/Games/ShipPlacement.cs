using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Battleship.Models.Battleship;
using Battleship.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace Battleship.Models.Games;

public class ShipPlacement
{
    [Key]
    [MaxLength(38)]
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    
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
    public CellCoordinate SourceCell => Ship.SourceCell();
    
    public ShipRotation Rotation { get; set; } = ShipRotation.SOUTH;
    
    [Range(0, BoardDictionary.Width0Based)]
    public ushort X { get; set; }
    
    [Range(0, BoardDictionary.Height0Based)]
    public ushort Y { get; set; }
    
    [NotMapped]
    public CellCoordinate SourcePlacement => new (X, Y);
    
    [Required]
    [MaxLength(38)]
    public string PlayerId { get; set; }
    [ForeignKey(nameof(PlayerId))]
    public ApplicationUser Player { get; set; }
    
    public ICollection<ShipHit> Hits { get; set; }
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