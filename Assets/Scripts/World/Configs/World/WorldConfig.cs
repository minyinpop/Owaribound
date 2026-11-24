using UnityEngine;
using UnityEngine.Tilemaps;
using World.Configs.Block;
using World.Configs.Block.Grass;
using World.Configs.Block.Ore;
using World.Configs.Block.Stone;

namespace World.Configs.Planet
{
    [CreateAssetMenu(menuName = "Owaribound/World/Generator/Planet Configs", fileName = "New Planet Config")]
    public sealed class WorldConfig : ScriptableObject
    {
        [field: Header("世界大小")]
        [field: SerializeField] private int width = 200;
                                public int Width => width;
        [field: SerializeField] private int height = 100;
                                public int Height => height;

        [field: Header("地表")]
        [field: SerializeField] private int baseHeight = 60;         // 地表平均高度
                                public int BaseHeight => baseHeight;
        [field: SerializeField] private int amplitude = 6;           // 起伏幅度（上下各幾格）
                                public int Amplitude => amplitude;
        [field: SerializeField] private float frequency = .03f;      // 越小越平緩
                                public float Frequency => frequency;
        [field: SerializeField] private int grassDepth = 5;          // 泥土層厚度
                                public int GrassDepth => grassDepth;
                                
        [field: Header("玩家出生點")]
        [field: SerializeField] private int spawnFlatRadius = 12;               // 中間多寬是平的（左右各幾格）
                                public int SpawnFlatRadius => spawnFlatRadius;
        [field: SerializeField] private int spawnBlendWidth = 20;               // 從平地過渡到正常地形的寬度
                                public int SpawnBlendWidth => spawnBlendWidth;
        [field: SerializeField, Range(0, 1)] private float spawnFlatness = .1f; // 平地區保留多少起伏（0 = 完全平）
                                public float SpawnFlatness => spawnFlatness;

        [field: Header("方塊資料")]
        [field: SerializeField] private GrassConfig grass;
                                public GrassConfig Grass => grass;
        [field: SerializeField] private StoneConfig stone;
                                public StoneConfig Stone => stone;
        [field: SerializeField] private OreConfig[] ores;
                                public OreConfig[] Ores => ores;
                                
        [field: Header("洞穴入口")]
        [field: SerializeField] private int entranceCount = 3;
                                public int EntranceCount => entranceCount;
        [field: SerializeField] private int entranceLength = 40;
                                public int EntranceLength => entranceLength;
        [field: SerializeField] private float entranceRadius = 1.5f;
                                public float EntranceRadius => entranceRadius;
        [field: SerializeField] private Vector2 entranceSlope = new(15f, 75f); // 往下傾斜的角度範圍（最緩, 最陡）
                                public Vector2 EntranceSlope => entranceSlope;
        [field: SerializeField] private float entranceWobble = 15f;            // 擺動幅度
                                public float EntranceWobble => entranceWobble;
        
        [field: Header("洞穴")]
        [field: SerializeField] private float caveScale = .08f;                           // 越小洞越大
                                public float CaveScale => caveScale;
        [field: SerializeField, Range(0, 1)] private float caveThreshold = .65f;          // 越低洞越多
                                             public float CaveThreshold => caveThreshold;
        [field: SerializeField] private int caveStartDepth = 8;                           // 地表往下幾格才開始挖，避免地表被挖穿
                                public int CaveStartDepth => caveStartDepth;
    }
}