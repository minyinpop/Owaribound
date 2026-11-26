using UnityEngine;
using World.Data;

namespace World.Generator
{
    public sealed partial class WorldGenerator
    {
        private void FillLayers(System.Random random)
        {
            var offset = (float)(random.NextDouble() * 10000);

            for (var x = 0; x < world.WorldConfig.Width; x++)
            {
                var noise = Mathf.PerlinNoise(offset + x * world.WorldConfig.DirtFrequency, 0);
                var dirtDepth = Mathf.RoundToInt(Mathf.Lerp(world.WorldConfig.DirtDepthRange.x,
                                                                 world.WorldConfig.DirtDepthRange.y, noise));
                
                for (var y = 0; y < world.WorldData.Surface[x]; y++)
                {
                    var depth = world.WorldData.Surface[x] - 1 - y;
                    var isSoil = depth < dirtDepth;
                    
                    world.WorldData.Blocks[x, y] = isSoil ? BlockType.Dirt : BlockType.Stone;
                    if (depth == 0)
                    {
                        world.WorldData.Walls[x, y] = WallType.Grass;
                    }
                    else
                    {
                        if (isSoil)
                            world.WorldData.Walls[x, y] = WallType.Dirt;
                        else
                            world.WorldData.Walls[x, y] = WallType.Stone;
                    }
                }
            }
        }
    }
}