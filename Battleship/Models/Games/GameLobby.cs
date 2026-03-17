using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Battleship.Models.Identity;
using JC.Core.Models.Auditing;

namespace Battleship.Models.Games;

public class GameLobby : AuditModel
{
    [Key]
    [MaxLength(38)]
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    
    [MaxLength(38)]
    public string? GameId { get; set; }
    [ForeignKey(nameof(GameId))]
    public Game? Game { get; set; }
    
    [Required]
    [MaxLength(38)]
    public string PlayerId { get; set; }
    [ForeignKey(nameof(PlayerId))]
    public ApplicationUser Player { get; set; }
    
    [Required]
    [MaxLength(38)]   
    public string InvitedPlayerId { get; set; }
    [ForeignKey(nameof(InvitedPlayerId))]
    public ApplicationUser InvitedPlayer { get; set; }


    public bool RandomPlayerStart { get; set; } = true;
    public bool NoMines { get; set; }
    public ushort? GameTimerSeconds { get; set; } = 120;
}