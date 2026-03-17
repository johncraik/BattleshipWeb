using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Battleship.Models.Games;
using JC.Core.Models.Auditing;

namespace Battleship.Models.Battleship;

public class Ship : AuditModel
{
    [Key]
    [MaxLength(38)]
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    
    [Required]
    [MaxLength(38)]
    public string CommanderId { get; set; }
    [ForeignKey(nameof(CommanderId))]
    public Commander Commander { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Range(MinSize, MaxSize)]
    public ushort Size { get; set; }
    
    public ICollection<ShipCell> Cells { get; set; }
    public ICollection<ShipPlacement> GamePlacements { get; set; }

    public bool[][] CellMatrix()
    {
        var matrix = new bool[MaxHeight][];
        for (var i = 0; i < MaxHeight; i++)
        {
            matrix[i] = new bool[MaxWidth];
        }
        
        foreach (var cell in Cells)
        {
            matrix[cell.Y][cell.X] = true;
        }
        
        return matrix;
    }

    public CellCoordinate SourceCell()
    {
        var matrix = CellMatrix();
        
        for (ushort y = 0; y < matrix.Length; y++)
        {
            var row = matrix[y];
            for (ushort x = 0; x < row.Length; x++)
            {
                var cell = row[x];
                if (!cell) continue;
                
                return new CellCoordinate(x, y);
            }
        }
        
        throw new InvalidOperationException("No source cell found");
    }
    
    public const ushort MinSize = 2;
    public const ushort MaxSize = 6;
    public const ushort MaxWidth = 3;
    public const ushort MaxHeight = 6;
}