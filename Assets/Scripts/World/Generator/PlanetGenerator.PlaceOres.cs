using UnityEngine;

namespace World.Generator
{
    public sealed partial class PlanetGenerator
    {
        private void PlaceOres(System.Random rng)
        {
            foreach (var ore in config.Ores)
            {
                var offsetX = (float)(rng.NextDouble() * 10000);
                var offsetY = (float)(rng.NextDouble() * 10000);

                for (var x = 0; x < config.Width; x++)
                for (var y = 0; y < WorldData.Surface[x]; y++)
                {
                    // 只看原本是岩石層的地方（用牆判斷，挖空的洞穴也算）
                    if (WorldData.Walls[x, y] != Wall.Stone)
                        continue;

                    var depth = WorldData.Surface[x] - 1 - y;
                    if (depth < ore.MinDepth || depth > ore.MaxDepth)
                        continue;

                    var noise = Mathf.PerlinNoise(offsetX + x * ore.Scale, offsetY + y * ore.Scale);
                    if (noise < ore.Threshold)
                        continue;

                    WorldData.Walls[x, y] = ore.Wall;                // 背景一定換
                    if (WorldData.Blocks[x, y] == Block.Stone)
                        WorldData.Blocks[x, y] = ore.Block;          // 前景還在才換
                }
            }
        }
    }
}