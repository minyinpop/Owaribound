using World.Data;

namespace World.Generator
{
    public sealed partial class WorldGenerator
    {
        private void FillLayers()
        {
            for (var x = 0; x < world.WorldConfig.Width; x++)
            for (var y = 0; y < world.WorldData.Surface[x]; y++)
            {
                var depth = world.WorldData.Surface[x] - 1 - y; // 距離地表幾格
                var isSoil = depth < world.WorldConfig.DirtDepth;
                
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