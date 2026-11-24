using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using World.Configs.Block;
using World.Configs.Planet;
using World.Data;

namespace World.Generator
{
    public sealed partial class PlanetGenerator : MonoBehaviour
    {
        [field: Header("世界設定")]
        [field: SerializeField] private PlanetConfig config;
        [field: SerializeField] private Tilemap blockTilemap;
                                public Tilemap BlockTilemap => blockTilemap;
        [field: SerializeField] private Tilemap wallTilemap;
        [field: SerializeField] private int seed;
        
        [field: Header("玩家設定")]
        [field: SerializeField] private Transform player;

        private WorldData WorldData { get; set; }

        private readonly Dictionary<BlockType, BlockConfig> blockConfigs = new();
        private readonly Dictionary<WallType, BlockConfig> wallConfigs = new();

        [ContextMenu("預先生成地形 ( 測試用 )")]
        private void Start()
        {
            RegisterBlockConfigs();
            
            WorldData = new WorldData(config.Width, config.Height);
            
            GenerateSurface(new System.Random(seed));
            
            FillLayers();
            
            CarveCaves(new System.Random(seed + 1));
            
            CarveCaveEntrances(new System.Random(seed + 2));
            
            PlaceOres(new System.Random(seed + 3));

            Render();
            
            SpawnPlayer(config.Width / 2);
        }
    }
}