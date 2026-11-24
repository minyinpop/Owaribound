using UnityEngine;
using World.Data;

namespace World.Generator
{
    public sealed partial class WorldGenerator
    {
        private void PlaceOres(System.Random rng)
        {
            foreach (var ore in world.WorldConfig.Ores)
            {
                var offsetX = (float)(rng.NextDouble() * 10000);
                var offsetY = (float)(rng.NextDouble() * 10000);

                for (var x = 0; x < world.WorldConfig.Width; x++)
                for (var y = 0; y < world.WorldData.Surface[x]; y++)
                {
                    // 只看原本是岩石層的地方（用牆判斷，挖空的洞穴也算）
                    if (world.WorldData.Walls[x, y] != WallType.Stone)
                        continue;

                    var depth = world.WorldData.Surface[x] - 1 - y;
                    if (depth < ore.MinDepth || depth > ore.MaxDepth)
                        continue;

                    var noise = Mathf.PerlinNoise(offsetX + x * ore.Scale, offsetY + y * ore.Scale);
                    if (noise < ore.Threshold)
                        continue;

                    world.WorldData.Walls[x, y] = ore.WallType;                // 背景一定換
                    if (world.WorldData.Blocks[x, y] == BlockType.Stone)
                        world.WorldData.Blocks[x, y] = ore.BlockType;          // 前景還在才換
                }
            }
        }
    }
}