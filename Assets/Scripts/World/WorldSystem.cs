using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using World.Configs.Block;
using World.Configs.World;
using World.Data;
using World.Generator;

namespace World
{
    [RequireComponent(typeof(WorldGenerator))]
    [RequireComponent(typeof(WorldDamageSystem))]
    [RequireComponent(typeof(WorldItemDropSystem))]
    public sealed class WorldSystem : MonoBehaviour
    {
        [field: Header("自身系統")]
        [field: Tooltip("世界生成器")]
        [field: SerializeField] private WorldGenerator worldGenerator;
                                public WorldGenerator WorldGenerator => worldGenerator;
                                
        [field: Tooltip("挖掘方塊系統")]
        [field: SerializeField] private WorldDamageSystem worldDamageSystem;
                                public WorldDamageSystem WorldDamageSystem => worldDamageSystem;
        
        [field: Tooltip("掉落物系統")]
        [field: SerializeField] private WorldItemDropSystem worldItemDropSystem;
                                public WorldItemDropSystem WorldItemDropSystem => worldItemDropSystem;
        
        // ================================
        
        [field: Header("世界參數")]
        [field: SerializeField] private WorldConfig worldConfig;
                                public WorldConfig WorldConfig => worldConfig;
                                
        // ================================
                                
        [field: Header("圖層設定")]
        [field: SerializeField] private Tilemap blockTilemap;
                                public Tilemap BlockTilemap => blockTilemap;
        [field: SerializeField] private Tilemap wallTilemap;
                                public Tilemap WallTilemap => wallTilemap;
        
        // ================================
                                
        [field: Header("世界種子碼")]
        [field: SerializeField] private int worldSeed;
                                public int WorldSeed => worldSeed;
        
        // ================================
                                
        [field: Header("玩家設定")]
        [field: SerializeField] private Transform player;
                                public Transform Player => player;
        
        // ================================
                                
        [field: Header("方塊裂痕設定")]
        [field: SerializeField] private Tilemap blockCrackTilemap;
        [field: SerializeField] private Tilemap wallCrackTilemap;
        
        [field: Tooltip("裂痕的紋理素材")]
        [field: SerializeField] private TileBase[] crackTiles;
                                public TileBase[] CrackTiles => crackTiles;
        
        [field: Tooltip("方塊多久會自己回到完整的狀態")]
        [field: SerializeField] private float recoverDelay = 3;
                                public float RecoverDelay => recoverDelay;
        
        public Tilemap GetCrackTilemap(WorldLayer layer)
        {
            return layer == WorldLayer.Block ? blockCrackTilemap : wallCrackTilemap;
        }
        
        // ================================
        
        [field: Header("放置設定")]
        [field: Tooltip("那些圖層會阻擋玩家的放置")]
        [field: SerializeField] private LayerMask placeBlockingLayer;
                                
        public WorldData WorldData { get; private set; }

        public readonly Dictionary<BlockType, BlockConfig> BlockConfigs = new();
        public readonly Dictionary<WallType, BlockConfig> WallConfigs = new();

        public event Action<Vector3Int, BlockConfig> OnBlockBroken;
        public event Action<Vector3Int, BlockConfig> OnBlockPlaced;

        public event Action<Vector3Int, BlockConfig> OnWallBroken;
        public event Action<Vector3Int, BlockConfig> OnWallPlaced;
        
        private void Start()
        {
            WorldData = new WorldData(WorldConfig.Width, WorldConfig.Height);
            WorldGenerator.Generator();
        }
        
        /// <summary>
        /// 用來查詢這一格是什麼方塊
        /// </summary>
        /// <param name="cell"> 要被查詢的位置 </param>
        /// <returns> 回傳該位置的方塊資料，如果是空氣或是地圖外，將會回傳 null </returns>
        public BlockConfig GetBlockConfig(Vector3Int cell)
        {
            if (!WorldData.InBounds(cell.x, cell.y)) return null;
            return BlockConfigs.GetValueOrDefault(WorldData.Blocks[cell.x, cell.y]);
        }

        /// <summary>
        /// 用來查詢這一格的背景牆是什麼
        /// </summary>
        /// <param name="cell"> 要被查詢的位置 </param>
        /// <returns> 回傳該位置的背景牆資料，如果是空氣或是地圖外，將會回傳 null </returns>
        public BlockConfig GetWallConfig(Vector3Int cell)
        {
            if (!WorldData.InBounds(cell.x, cell.y)) return null;
            return WallConfigs.GetValueOrDefault(WorldData.Walls[cell.x, cell.y]);
        }

        /// <summary>
        /// 專門負責處理要被破壞掉的方塊
        /// </summary>
        /// <param name="cell"> 要被破壞的位置 </param>
        /// <returns> 回傳是否成功破壞 </returns>
        public bool TryBreakBlock(Vector3Int cell)
        {
            var config = GetBlockConfig(cell);
            if (!config) return false;
            
            WorldData.Blocks[cell.x, cell.y] = BlockType.Air;
            blockTilemap.SetTile(cell, null);
            
            OnBlockBroken?.Invoke(cell, config);
            return true;
        }

        /// <summary>
        /// 專門負責處理要被破壞掉的背景牆
        /// </summary>
        /// <param name="cell"> 要被破壞的位置 </param>
        /// <returns> 回傳是否成功破壞 </returns>
        public bool TryBreakWall(Vector3Int cell)
        {
            var config = GetWallConfig(cell);
            if (!config) return false;
            if (IsSolid(cell)) return false;

            WorldData.Walls[cell.x, cell.y] = WallType.Air;
            wallTilemap.SetTile(cell, null);
            
            OnWallBroken?.Invoke(cell, config);
            return true;
        }

        /// <summary>
        /// 專門負責處理要被放置的方塊
        /// </summary>
        /// <param name="cell"> 要被放置的位置 </param>
        /// <param name="blockConfig"> 要被放置的方塊 </param>
        /// <returns> 回傳是否放置成功 </returns>
        public bool TryPlaceBlock(Vector3Int cell, BlockConfig blockConfig)
        {
            if (!blockConfig) return false;
            if (!WorldData.InBounds(cell.x, cell.y)) return false;
            if (WorldData.Blocks[cell.x, cell.y] != BlockType.Air) return false;
            if (!HasSupport(cell)) return false;
            if (IsOccupied(cell)) return false;

            WorldData.Blocks[cell.x, cell.y] = blockConfig.BlockType;
            blockTilemap.SetTile(cell, blockConfig.BlockTile);
            
            CoverGrass(cell + Vector3Int.down);
            
            OnBlockPlaced?.Invoke(cell, blockConfig);
            return true;
        }

        /// <summary>
        /// 專門負責處理要被放置的背景牆
        /// </summary>
        /// <param name="cell"> 要被放置的位置 </param>
        /// <param name="blockConfig"> 要被放置的背景牆 </param>
        /// <returns> 回傳是否放置成功 </returns>
        public bool TryPlaceWall(Vector3Int cell, BlockConfig blockConfig)
        {
            if (!blockConfig) return false;
            if (blockConfig.WallType == WallType.Air) return false;
            if (!blockConfig.WallTile) return false;
            if (!WorldData.InBounds(cell.x, cell.y)) return false;
            if (WorldData.Walls[cell.x, cell.y] != WallType.Air) return false;
            if (IsSolid(cell)) return false;
            if (!HasWallSupport(cell)) return false;

            WorldData.Walls[cell.x, cell.y] = blockConfig.WallType;
            wallTilemap.SetTile(cell, blockConfig.WallTile);

            OnWallPlaced?.Invoke(cell, blockConfig);
            return true;
        }

        /// <summary>
        /// 用於檢查格子的周圍以及背景是否有支撐點
        /// </summary>
        /// <param name="cell"> 要被檢查的位置 </param>
        /// <returns> 回傳是否有支撐點 </returns>
        private bool HasSupport(Vector3Int cell)
        {
            if (WorldData.Walls[cell.x, cell.y] != WallType.Air) return true;
            if (IsSolid(cell + Vector3Int.up)) return true;
            if (IsSolid(cell + Vector3Int.down)) return true;
            if (IsSolid(cell + Vector3Int.left)) return true;
            if (IsSolid(cell + Vector3Int.right)) return true;
            return false;
        }

        /// <summary>
        /// 用於檢查格子與背景牆周圍是否有支撐點
        /// </summary>
        /// <param name="cell"> 要被檢查的位置 </param>
        /// <returns> 回傳是否有支撐點 </returns>
        private bool HasWallSupport(Vector3Int cell)
        {
            foreach (var dir in new[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right })
            {
                var neighbor = cell + dir;
                if (IsSolid(neighbor) || HasWall(neighbor)) return true;
            }
            return false;
        }

        /// <summary>
        /// 檢查格子是否已經有方塊了
        /// </summary>
        /// <param name="cell"> 要被檢查的格子 </param>
        /// <returns> 回傳是否有方塊 </returns>
        private bool IsSolid(Vector3Int cell)
        {
            if (!WorldData.InBounds(cell.x, cell.y)) return false;
            if (WorldData.Blocks[cell.x, cell.y] == BlockType.Air) return false;
            return true;
        }

        /// <summary>
        /// 檢查格子是否已經有背景牆了
        /// </summary>
        /// <param name="cell"> 要被檢查的格子 </param>
        /// <returns> 回傳是否有背景牆 </returns>
        private bool HasWall(Vector3Int cell)
        {
            if (!WorldData.InBounds(cell.x, cell.y)) return false;
            return WorldData.Walls[cell.x, cell.y] != WallType.Air;
        }

        /// <summary>
        /// 檢查格子是否有實體擋住
        /// </summary>
        /// <param name="cell"> 要被檢查的格子 </param>
        /// <returns> 回傳是否被擋住 </returns>
        private bool IsOccupied(Vector3Int cell)
        {
            var center = blockTilemap.GetCellCenterWorld(cell);
            if (Physics2D.OverlapBox(center, Vector2.one * .9f, 0, placeBlockingLayer)) return true;
            return false;
        }

        /// <summary>
        /// 檢查是否有草地，如果有，就把草地變成泥土方塊
        /// </summary>
        /// <param name="cell"> 要被檢查的格子 </param>
        private void CoverGrass(Vector3Int cell)
        {
            if (!WorldData.InBounds(cell.x, cell.y)) return;
            if (WorldData.Blocks[cell.x, cell.y] != BlockType.Grass) return;
            WorldData.Blocks[cell.x, cell.y] = BlockType.Dirt;
            blockTilemap.SetTile(cell, BlockConfigs[BlockType.Dirt].BlockTile);
        }
    }
}