using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using World.Data;

namespace World.Generator
{
    public sealed partial class PlanetGenerator
    {
        private void Render()
        {
            var blockLookup = new Dictionary<BlockType, TileBase>
            {
                { BlockType.Grass,  config.Grass.BlockTile },
                { BlockType.Stone, config.Stone.BlockTile },
            };
            var wallLookup = new Dictionary<WallType, TileBase>
            {
                { WallType.Grass,  config.Grass.WallTile },
                { WallType.Stone, config.Stone.WallTile },
            };
            foreach (var ore in config.Ores)
            {
                blockLookup[ore.BlockType] = ore.BlockTile;
                wallLookup[ore.WallType]   = ore.WallTile;
            }
            
            var w = config.Width;
            var h = config.Height;
            var blockTiles = new TileBase[w * h];
            var wallTiles = new TileBase[w * h];

            for (var x = 0; x < w; x++)
            for (var y = 0; y < h; y++)
            {
                blockTiles[x + y * w] = blockLookup.GetValueOrDefault(WorldData.Blocks[x, y]);
                wallTiles[x + y * w]  = wallLookup.GetValueOrDefault(WorldData.Walls[x, y]);
            }

            var bounds = new BoundsInt(0, 0, 0, w, h, 1);

            blockTilemap.ClearAllTiles();
            blockTilemap.SetTilesBlock(bounds, blockTiles);
                    
            wallTilemap.ClearAllTiles();
            wallTilemap.SetTilesBlock(bounds, wallTiles);
        }
    }
}