using World.Configs.Block;

namespace World.Generator
{
    public sealed partial class WorldGenerator
    {
        private void RegisterBlockConfigs()
        {
            world.BlockConfigs.Clear();
            world.WallConfigs.Clear();

            Register(world.WorldConfig.Grass);
            Register(world.WorldConfig.Dirt);
            Register(world.WorldConfig.Stone);
            foreach (var ore in world.WorldConfig.Ores)
                Register(ore);
        }

        private void Register(BlockConfig blockConfig)
        {
            // 用索引子 [] 而不是 Add，重複登記時會覆蓋，不會報錯
            world.BlockConfigs[blockConfig.BlockType] = blockConfig;
            world.WallConfigs[blockConfig.WallType]   = blockConfig;
        }
    }
}