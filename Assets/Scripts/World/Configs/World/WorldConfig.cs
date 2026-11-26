using UnityEngine;
using World.Configs.Block.Dirt;
using World.Configs.Block.Grass;
using World.Configs.Block.Ore;
using World.Configs.Block.Stone;

namespace World.Configs.World
{
    [CreateAssetMenu(menuName = "Owaribound/World/Generator/Planet Configs", fileName = "New Planet Config")]
    public sealed class WorldConfig : ScriptableObject
    {
        [field: Header("世界生成參數")]
        [field: Tooltip("世界的總寬度")]
        [field: SerializeField] private int width = 300;
                                public int Width => width;
                                
        [field: Tooltip("世界的總高度")]
        [field: SerializeField] private int height = 200;
                                public int Height => height;
        
        [field: Tooltip("世界地表平均高度")]
        [field: SerializeField] private int baseHeight = 60;
                                public int BaseHeight => baseHeight;
        
        [field: Tooltip("世界地表起伏幅度")]
        [field: SerializeField] private int amplitude = 20;
                                public int Amplitude => amplitude;
                                
        [field: Tooltip("世界地表起伏頻率")]
        [field: SerializeField] private float frequency = .05f;
                                public float Frequency => frequency;
                                
        // ================================

        [field: Header("地表參數")]
        [field: Tooltip("泥土層厚度範圍")]
        [field: SerializeField] private Vector2Int dirtDepthRange = new(3, 10);
                                public Vector2Int DirtDepthRange => dirtDepthRange;
        
        [field: Tooltip("泥土層厚度頻率")]
        [field: SerializeField] private float dirtFrequency = .1f;
                                public float DirtFrequency => dirtFrequency;
        
        // ================================
        
        [field: Header("玩家出生點地形參數")]
        [field: Tooltip("以中間項兩側外計算多少格是平緩的")]
        [field: SerializeField] private int spawnFlatRadius = 12;
                                public int SpawnFlatRadius => spawnFlatRadius;
                                
        [field: Tooltip("從平緩地形過渡到正常地形的寬度")]
        [field: SerializeField] private int spawnBlendWidth = 20;
                                public int SpawnBlendWidth => spawnBlendWidth;
                                
        [field: Range(0, 1)]
        [field: Tooltip("平地區域的起伏程度為多少")]
        [field: SerializeField] private float spawnFlatness = .1f;
                                public float SpawnFlatness => spawnFlatness;
                                
        // ================================

        [field: Header("方塊資料")]
        [field: SerializeField] private GrassConfig grass;
                                public GrassConfig Grass => grass;
        [field: SerializeField] private DirtConfig dirt;
                                public DirtConfig Dirt => dirt;
        [field: SerializeField] private StoneConfig stone;
                                public StoneConfig Stone => stone;
        [field: SerializeField] private OreConfig[] ores;
                                public OreConfig[] Ores => ores;
                                
        // ================================
                                
        [field: Header("洞穴入口參數")]
        [field: Tooltip("洞穴入口的數量")]
        [field: SerializeField] private int entranceCount = 6;
                                public int EntranceCount => entranceCount;
                                
        [field: Tooltip("隧道的長度為多少")]
        [field: SerializeField] private int entranceLength = 80;
                                public int EntranceLength => entranceLength;
                                
        [field: Tooltip("隧道的半徑")]
        [field: SerializeField] private float entranceRadius = 1.5f;
                                public float EntranceRadius => entranceRadius;
                                
        [field: Tooltip("隧道的傾斜範圍")]
        [field: SerializeField] private Vector2 entranceSlope = new(15f, 75f);
                                public Vector2 EntranceSlope => entranceSlope;
                                
        [field: Tooltip("隧道的擺動範圍")]
        [field: SerializeField] private float entranceWobble = 45f;
                                public float EntranceWobble => entranceWobble;
                                
        // ================================
        
        [field: Header("洞穴參數")]
        [field: Tooltip("洞穴的大小，越小洞越大、越平滑")]
        [field: SerializeField] private float caveScale = .08f;
                                public float CaveScale => caveScale;
        
        [field: Range(0, 1)]
        [field: Tooltip("挖空的門檻，越低洞越多")]
        [field: SerializeField] private float caveThreshold = .65f;
                                public float CaveThreshold => caveThreshold;
                                
        [field: Tooltip("地表往下幾格後才開始產生洞穴")]
        [field: SerializeField] private int caveStartDepth = 16;
                                public int CaveStartDepth => caveStartDepth;
    }
}