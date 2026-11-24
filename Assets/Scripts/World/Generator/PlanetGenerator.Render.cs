using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace World.Generator
{
    public sealed partial class PlanetGenerator
    {
        private void Render()
        {
            var w = config.Width;
            var h = config.Height;
            var blockTiles = new TileBase[w * h];
            var wallTiles  = new TileBase[w * h];

            for (var x = 0; x < w; x++)
            for (var y = 0; y < h; y++)
            {
                blockTiles[x + y * w] = blockConfigs.GetValueOrDefault(WorldData.Blocks[x, y])?.BlockTile;
                wallTiles[x + y * w]  = wallConfigs.GetValueOrDefault(WorldData.Walls[x, y])?.WallTile;
            }

            var bounds = new BoundsInt(0, 0, 0, w, h, 1);

            blockTilemap.ClearAllTiles();
            blockTilemap.SetTilesBlock(bounds, blockTiles);

            wallTilemap.ClearAllTiles();
            wallTilemap.SetTilesBlock(bounds, wallTiles);
        }
    }
}