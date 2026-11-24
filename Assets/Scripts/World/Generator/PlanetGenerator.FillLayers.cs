using World.Data;

namespace World.Generator
{
    public sealed partial class PlanetGenerator
    {
        private void FillLayers()
        {
            for (var x = 0; x < config.Width; x++)
            for (var y = 0; y < WorldData.Surface[x]; y++)
            {
                var depth = WorldData.Surface[x] - 1 - y; // 距離地表幾格
                var isGrass = depth < config.GrassDepth;
                        
                WorldData.Blocks[x, y] = isGrass ? BlockType.Grass : BlockType.Stone;
                WorldData.Walls[x, y] = isGrass ? WallType.Grass : WallType.Stone;
            }
        }
    }
}