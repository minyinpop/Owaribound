using UnityEngine;

namespace World.Generator
{
    public sealed partial class WorldGenerator
    {
        private void SpawnPlayer(int x)
        {
            // surface[x] 是這一欄的高度，最上面那格實心方塊的頂部就在 y = surface[x]
            var cell = new Vector3Int(x, world.WorldData.Surface[x], 0);
            var pos = world.BlockTilemap.CellToWorld(cell) + new Vector3(world.BlockTilemap.cellSize.x / 2f, 0, 0);

            world.Player.position = pos;
        }
    }
}