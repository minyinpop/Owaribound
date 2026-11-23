using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace World.Generator
{
    public sealed partial class PlanetGenerator
    {
        private void Render()
        {
            var blockLookup = new Dictionary<Block, TileBase>
            {
                { Block.Dirt,  config.GrassBlockTile },
                { Block.Stone, config.StoneBlockTile },
            };
            var wallLookup = new Dictionary<Wall, TileBase>
            {
                { Wall.Dirt,  config.GrassWallTile },
                { Wall.Stone, config.StoneWallTile },
            };
            foreach (var ore in config.Ores)
            {
                blockLookup[ore.Block] = ore.BlockTile;
                wallLookup[ore.Wall]   = ore.WallTile;
            }
                    
            var w = config.Width;
            var h = config.Height;
            var blockTiles = new TileBase[w * h];
            var wallTiles = new TileBase[w * h];

            for (var x = 0; x < w; x++)
            for (var y = 0; y < h; y++)
            {
                blockTiles[x + y * w] = blockLookup.GetValueOrDefault(blocks[x, y]);
                wallTiles[x + y * w]  = wallLookup.GetValueOrDefault(walls[x, y]);
            }

            var bounds = new BoundsInt(0, 0, 0, w, h, 1);

            blockTilemap.ClearAllTiles();
            blockTilemap.SetTilesBlock(bounds, blockTiles);
                    
            wallTilemap.ClearAllTiles();
            wallTilemap.SetTilesBlock(bounds, wallTiles);
        }
    }
}