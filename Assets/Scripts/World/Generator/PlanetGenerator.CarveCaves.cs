using UnityEngine;

namespace World.Generator
{
    public sealed partial class PlanetGenerator
    {
        private void CarveCaves(System.Random rng)
        {
            var offsetX = (float)(rng.NextDouble() * 10000);
            var offsetY = (float)(rng.NextDouble() * 10000);

            for (var x = 0; x < config.Width; x++)
            for (var y = 0; y < surface[x]; y++)
            {
                var depth = surface[x] - 1 - y;
                if (depth < config.CaveStartDepth)
                    continue; // 太靠近地表，不挖

                var noise = Mathf.PerlinNoise(offsetX + x * config.CaveScale,
                    offsetY + y * config.CaveScale);

                if (noise > config.CaveThreshold)
                    blocks[x, y] = Block.Air;
            }
        }
    }
}