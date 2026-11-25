using World.Data;

namespace World.Generator
{
    public sealed partial class WorldGenerator
    {
        private void GrowGrass()
        {
            for (var x = 0; x < world.WorldConfig.Width; x++)
            for (var y = world.WorldData.Surface[x] - 1; y >= 0; y--)
            {
                var block = world.WorldData.Blocks[x, y];
                
                if (block == BlockType.Air) continue;

                if (block == BlockType.Dirt)
                    world.WorldData.Blocks[x, y] = BlockType.Grass;
                break;
            }
        }
    }
}