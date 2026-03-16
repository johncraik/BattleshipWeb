using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Battleship.Models.Battleship;
using Battleship.Models.Identity;
using JC.Core.Models.Auditing;

namespace Battleship.Models.Games;

public class Game : AuditModel
{
    [Key]
    [MaxLength(38)]
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    
    [Required]
    [MaxLength(38)]
    public string Player1Id { get; set; }
    [ForeignKey(nameof(Player1Id))]
    public ApplicationUser Player1 { get; set; }
    
    [Required]
    [MaxLength(38)]
    public string Player1CommanderId { get; set; }
    [ForeignKey(nameof(Player1CommanderId))]
    public Commander Player1Commander { get; set; }
    
    
    [Required]
    [MaxLength(38)]
    public string Player2Id { get; set; }
    [ForeignKey(nameof(Player2Id))]
    public ApplicationUser Player2 { get; set; }
    
    [Required]
    [MaxLength(38)]   
    public string Player2CommanderId { get; set; }
    [ForeignKey(nameof(Player2CommanderId))]
    public Commander Player2Commander { get; set; }
    
    

    [NotMapped]
    public DateTime StartTime => CreatedUtc;
    
    public DateTime? TimeFinished { get; private set; }
    
    [NotMapped]
    public bool IsFinished => GameWinner == GameWinner.None || TimeFinished != null;
    
    public bool IsPlayer1Turn { get; private set; }

    public GameWinner GameWinner { get; private set; } = GameWinner.None;


    public void Start(bool player1Starts = true)
    {
        if(TimeFinished.HasValue)
            return;
        
        IsPlayer1Turn = player1Starts;
        GameWinner = GameWinner.None;
        TimeFinished = null;
    }

    public void SwitchTurn()
    {
        if(TimeFinished.HasValue)
            return;
        
        IsPlayer1Turn = !IsPlayer1Turn;
    }
    
    public void Finish(bool player1Won)
    {
        if(TimeFinished.HasValue)
            return;
        
        TimeFinished = DateTime.UtcNow;
        GameWinner = player1Won ? GameWinner.Player1 : GameWinner.Player2;
    }

    public void Draw()
    {
        if(TimeFinished.HasValue)
            return;
        
        TimeFinished = DateTime.UtcNow;
        GameWinner = GameWinner.Draw;
    }
}

public enum GameWinner
{
    None,
    Draw,
    Player1,
    Player2
}