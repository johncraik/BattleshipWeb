using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Battleship.Models.Battleship;
using Battleship.Models.Identity;
using JC.Core.Models.Auditing;

namespace Battleship.Models.Games;

public class BoardCellAction : AuditModel
{
    [Key]
    [MaxLength(38)]
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    
    [Required]
    [MaxLength(38)]
    public string PlayerId { get; set; }
    [ForeignKey(nameof(PlayerId))]
    public ApplicationUser Player { get; set; }
    
    [Required]
    [MaxLength(38)]
    public string GameId { get; set; }
    [ForeignKey(nameof(GameId))]
    public Game Game { get; set; }
    
    public BoardCellActionType ActionType { get; set; } = BoardCellActionType.Shot;
    
    [Range(0, BoardDictionary.Width0Based)]
    public ushort X { get; set; }
    
    [Range(0, BoardDictionary.Height0Based)]
    public ushort Y { get; set; }
    
    [NotMapped]
    public CellCoordinate ShotCoordinate => new(X, Y);
}

public enum BoardCellActionType
{
    Shot,
    Radar,
    Mine
}