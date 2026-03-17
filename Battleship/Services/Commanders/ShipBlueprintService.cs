using Battleship.Models.Battleship;
using JC.Core.Enums;
using JC.Core.Extensions;
using JC.Core.Services.DataRepositories;
using JC.Web.UI.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Battleship.Services.Commanders;

public class ShipBlueprintService
{
    private readonly IRepositoryManager _repos;

    public ShipBlueprintService(IRepositoryManager repos)
    {
        _repos = repos;
    }

    private async Task ValidateShipBlueprint(Ship ship, List<ShipCell> shipCells, ModelStateWrapper modelState)
    {
        //Setup:
        ship.Size = (ushort)shipCells.Count;
        
        var result = await _repos.GetRepository<Commander>()
            .AsQueryable().FilterDeleted(DeletedQueryType.OnlyActive)
            .AnyAsync(c => c.Id == ship.CommanderId);
        
        if (!result)
            modelState.AddModelError(nameof(ship.CommanderId), "Invalid commander");
        
        if(string.IsNullOrWhiteSpace(ship.Name))
            modelState.AddModelError(nameof(ship.Name), "Invalid ship name");
        
        var validatedCells = new HashSet<ShipCell>();
        foreach (var shipCell in shipCells)
        {
            if (validatedCells.Contains(shipCell))
                modelState.AddModelError(nameof(shipCell), $"Duplicate ship cell {shipCell.X}, {shipCell.Y}");
            
            if(shipCell.X >= Ship.MaxWidth || shipCell.Y >= Ship.MaxHeight)
                modelState.AddModelError(nameof(shipCell), $"Invalid ship cell {shipCell.X}, {shipCell.Y}");
            
            //Setup:
            shipCell.ShipId = ship.Id;
            
            validatedCells.Add(shipCell);
        }

        if (shipCells.Count < Ship.MaxSize || shipCells.Count > Ship.MaxWidth)
            modelState.AddModelError(nameof(shipCells), "Invalid ship size");

        var matrix = ShipCell.CreateMatrix(shipCells);
        if (!IsContiguous(matrix))
            modelState.AddModelError(nameof(shipCells), "Ship cells must be connected with no gaps");
    }
    

    private static bool IsContiguous(bool[][] matrix)
    {
        // Find the first filled cell
        ushort startX = 0, startY = 0;
        var found = false;
        for (ushort y = 0; y < matrix.Length && !found; y++)
        {
            for (ushort x = 0; x < matrix[y].Length && !found; x++)
            {
                if (!matrix[y][x]) continue;
                startX = x;
                startY = y;
                found = true;
            }
        }

        if (!found)
            return false;

        // BFS from the first cell
        var visited = new bool[matrix.Length][];
        for (var i = 0; i < matrix.Length; i++)
            visited[i] = new bool[matrix[i].Length];

        var queue = new Queue<(ushort X, ushort Y)>();
        queue.Enqueue((startX, startY));
        visited[startY][startX] = true;

        while (queue.Count > 0)
        {
            var (cx, cy) = queue.Dequeue();
            foreach (var (dx, dy) in new[] { (0, -1), (0, 1), (-1, 0), (1, 0) })
            {
                var nx = cx + dx;
                var ny = cy + dy;
                if (ny < 0 || ny >= matrix.Length || nx < 0 || nx >= matrix[ny].Length)
                    continue;
                if (visited[ny][nx] || !matrix[ny][nx])
                    continue;

                visited[ny][nx] = true;
                queue.Enqueue(((ushort)nx, (ushort)ny));
            }
        }

        // Check all filled cells were reached
        for (var y = 0; y < matrix.Length; y++)
        {
            for (var x = 0; x < matrix[y].Length; x++)
            {
                if (matrix[y][x] && !visited[y][x])
                    return false;
            }
        }

        return true;
    }

    public async Task<bool> TryCreateShipBlueprint(Ship ship, List<ShipCell> shipCells, ModelStateWrapper modelState)
    {
        await ValidateShipBlueprint(ship, shipCells, modelState);
        if(!modelState.IsValid) return false;
    }
}