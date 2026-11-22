using UnityEngine;
using UnityEngine.Tilemaps;

namespace World.Generator
{
    [CreateAssetMenu(menuName = "Owaribound/World/Generator/Planet Config")]
    public sealed class PlanetConfig : ScriptableObject
    {
        [field: Header("Size")]
        [field: SerializeField] private int width = 200;
                                public int Width => width;
        [field: SerializeField] private int height = 100;
                                public int Height => height;

        [field: Header("Surface")]
        [field: SerializeField] private int baseHeight = 60;         // 地表平均高度
                                public int BaseHeight => baseHeight;
        [field: SerializeField] private int amplitude = 6;           // 起伏幅度（上下各幾格）
                                public int Amplitude => amplitude;
        [field: SerializeField] private float frequency = .03f;      // 越小越平緩
                                public float Frequency => frequency;
        [field: SerializeField] private int dirtDepth = 5;           // 泥土層厚度
                                public int DirtDepth => dirtDepth;

        [field: Header("Block Tiles")]
        [field: SerializeField] private TileBase dirtBlockTile;
                                public TileBase DirtBlockTile => dirtBlockTile;
        [field: SerializeField] private TileBase stoneBlockTile;
                                public TileBase StoneBlockTile => stoneBlockTile;
                                
        [field: Header("Wall Tiles")]
        [field: SerializeField] private TileBase dirtWallTile;
                                public TileBase DirtWallTile => dirtWallTile;
        [field: SerializeField] private TileBase stoneWallTile;
                                public TileBase StoneWallTile => stoneWallTile;
        
        [field: Header("Canvas")]
        [field: SerializeField] private float caveScale = .08f;                           // 越小洞越大
                                public float CaveScale => caveScale;
        [field: SerializeField, Range(0, 1)] private float caveThreshold = .65f;          // 越低洞越多
                                             public float CaveThreshold => caveThreshold;
        [field: SerializeField] private int caveStartDepth = 8;                           // 地表往下幾格才開始挖，避免地表被挖穿
                                public int CaveStartDepth => caveStartDepth;
    }
}