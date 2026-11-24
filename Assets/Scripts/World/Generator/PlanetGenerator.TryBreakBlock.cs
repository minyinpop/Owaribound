using UnityEngine;
using World.Generator.Data;

namespace World.Generator
{
    public sealed partial class PlanetGenerator
    {
        public bool TryBreakBlock(Vector3Int cell)
        {
            if (!WorldData.InBounds(cell.x, cell.y))
                return false;

            if (WorldData.Blocks[cell.x, cell.y] == Block.Air)
                return false;

            WorldData.Blocks[cell.x, cell.y] = Block.Air;
            blockTilemap.SetTile(cell, null);

            return true;
        }
    }
}