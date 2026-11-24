using System.Collections.Generic;
using UnityEngine;
using World.Configs.Block;
using World.Data;

namespace World.Generator
{
    public sealed partial class PlanetGenerator
    {
        private void RegisterBlockConfigs()
        {
            blockConfigs.Clear();
            wallConfigs.Clear();

            Register(config.Grass);
            Register(config.Stone);
            foreach (var ore in config.Ores)
                Register(ore);
        }

        private void Register(BlockConfig blockConfig)
        {
            // 用索引子 [] 而不是 Add，重複登記時會覆蓋，不會報錯
            blockConfigs[blockConfig.BlockType] = blockConfig;
            wallConfigs[blockConfig.WallType]   = blockConfig;
        }

        public BlockConfig GetBlockConfig(Vector3Int cell)
        {
            if (!WorldData.InBounds(cell.x, cell.y))
                return null;

            return blockConfigs.GetValueOrDefault(WorldData.Blocks[cell.x, cell.y]);
        }
    }
}