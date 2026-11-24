using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace World.Generator
{
    public sealed partial class WorldGenerator
    {
        private void Render()
        {
            var w = world.WorldConfig.Width;
            var h = world.WorldConfig.Height;
            var blockTiles = new TileBase[w * h];
            var wallTiles  = new TileBase[w * h];

            for (var x = 0; x < w; x++)
            for (var y = 0; y < h; y++)
            {
                blockTiles[x + y * w] = world.BlockConfigs.GetValueOrDefault(world.WorldData.Blocks[x, y])?.BlockTile;
                wallTiles[x + y * w]  = world.WallConfigs.GetValueOrDefault(world.WorldData.Walls[x, y])?.WallTile;
            }

            var bounds = new BoundsInt(0, 0, 0, w, h, 1);

            world.BlockTilemap.ClearAllTiles();
            world.BlockTilemap.SetTilesBlock(bounds, blockTiles);

            world.WallTilemap.ClearAllTiles();
            world.WallTilemap.SetTilesBlock(bounds, wallTiles);
        }
    }
}