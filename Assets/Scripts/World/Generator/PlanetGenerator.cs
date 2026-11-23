using UnityEngine;
using UnityEngine.Tilemaps;
using World.Generator.Config.Planet;
using World.Generator.Data;

namespace World.Generator
{
    public enum Block : byte { Air, Dirt, Stone, Copper, Iron, Silver }
    public enum Wall  : byte { None, Dirt, Stone, Copper, Iron, Silver }

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

        public WorldData WorldData { get; private set; }

        [ContextMenu("預先生成地形 ( 測試用 )")]
        private void Start()
        {
            WorldData = new WorldData(config.Width, config.Height);
            
            // 步驟 1：用雜訊算出每一欄的地表高度
            GenerateSurface(new System.Random(seed));
            
            // 步驟 2：地表以下，上面幾格放泥土，再往下放岩石
            FillLayers();
            
            // 步驟 3：用 2D 雜訊挖出洞穴
            CarveCaves(new System.Random(seed + 1));
            
            // 步驟 4：挖空隨機個洞穴的入口
            CarveCaveEntrances(new System.Random(seed + 2));
            
            // 步驟 5：在岩石層放礦脈（方塊和背景牆一起換）
            PlaceOres(new System.Random(seed + 3));

            // 步驟 6：把資料轉成 Tile，一次打進 Tilemap
            Render();
            
            // 最後步驟：放置玩家的出生點
            SpawnPlayer(config.Width / 2);
        }
    }
}