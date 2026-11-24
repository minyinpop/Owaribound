using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using World.Configs.Block;
using World.Configs.Planet;
using World.Damage;
using World.Data;
using World.Generator;

namespace World
{
    [RequireComponent(typeof(WorldGenerator))]
    [RequireComponent(typeof(WorldDamageSystem))]
    public sealed class WorldSystem : MonoBehaviour
    {
        [field: Header("自身系統")]
        [field: SerializeField] private WorldGenerator worldGenerator;
                                public WorldGenerator WorldGenerator => worldGenerator;
        [field: SerializeField] private WorldDamageSystem worldDamageSystem;
                                public WorldDamageSystem WorldDamageSystem => worldDamageSystem;
        
        [field: Header("世界設定")]
        [field: SerializeField] private WorldConfig worldConfig;
                                public WorldConfig WorldConfig => worldConfig;
                                
        [field: Header("Rename")]
        [field: SerializeField] private Tilemap blockTilemap;
                                public Tilemap BlockTilemap => blockTilemap;
        [field: SerializeField] private Tilemap wallTilemap;
                                public Tilemap WallTilemap => wallTilemap;
        
        [field: Header("世界種子碼")]
        [field: SerializeField] private int worldSeed;
                                public int WorldSeed => worldSeed;
        
        [field: Header("玩家設定")]
        [field: SerializeField] private Transform player;
                                public Transform Player => player;
        
        [field: Header("裂痕設定")]
        [field: SerializeField] private Tilemap crackTilemap;
                                public Tilemap CrackTilemap => crackTilemap;
        [field: SerializeField] private TileBase[] crackTiles;
                                public TileBase[] CrackTiles => crackTiles;
        [field: SerializeField] private float recoverDelay = 3;
                                public float RecoverDelay => recoverDelay;
                                
        public WorldData WorldData { get; private set; }

        public readonly Dictionary<BlockType, BlockConfig> BlockConfigs = new();
        public readonly Dictionary<WallType, BlockConfig> WallConfigs = new();
        
        private void Start()
        {
            WorldData = new WorldData(WorldConfig.Width, WorldConfig.Height);
            
            WorldGenerator.Generator();
        }
        
        // 查詢：某一格是什麼方塊，空氣或地圖外回傳 null
        public BlockConfig GetBlockConfig(Vector3Int cell)
        {
            if (!WorldData.InBounds(cell.x, cell.y))
                return null;

            return BlockConfigs.GetValueOrDefault(WorldData.Blocks[cell.x, cell.y]);
        }

        // 修改：挖掉一格方塊（所有挖掉方塊的方式都走這裡）
        public bool TryBreakBlock(Vector3Int cell)
        {
            if (!GetBlockConfig(cell))
                return false;

            WorldData.Blocks[cell.x, cell.y] = BlockType.Air;
            blockTilemap.SetTile(cell, null);
            return true;
        }
    }
}