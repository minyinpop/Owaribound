using UnityEngine;
using World.Data;

namespace World.Generator
{
    public sealed partial class WorldGenerator
    {
        private void CarveCaves(System.Random rng)
        {
            var offsetX = (float)(rng.NextDouble() * 10000);
            var offsetY = (float)(rng.NextDouble() * 10000);

            for (var x = 0; x < world.WorldConfig.Width; x++)
            for (var y = 0; y < world.WorldData.Surface[x]; y++)
            {
                var depth = world.WorldData.Surface[x] - 1 - y;
                if (depth < world.WorldConfig.CaveStartDepth)
                    continue; // 太靠近地表，不挖

                var noise = Mathf.PerlinNoise(offsetX + x * world.WorldConfig.CaveScale,
                    offsetY + y * world.WorldConfig.CaveScale);

                if (noise > world.WorldConfig.CaveThreshold)
                    world.WorldData.Blocks[x, y] = BlockType.Air;
            }
        }
    }
}