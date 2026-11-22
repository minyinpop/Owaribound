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

        [field: Header("Tiles")]
        [field: SerializeField] private TileBase dirtTile;
                                public TileBase DirtTile => dirtTile;
        [field: SerializeField] private TileBase stoneTile;
                                public TileBase StoneTile => stoneTile;
        
        [field: Header("Canvas")]
        [field: SerializeField] private float caveScale = .08f;                           // 越小洞越大
                                public float CaveScale => caveScale;
        [field: SerializeField, Range(0, 1)] private float caveThreshold = .65f;          // 越低洞越多
                                             public float CaveThreshold => caveThreshold;
        [field: SerializeField] private int caveStartDepth = 8;                           // 地表往下幾格才開始挖，避免地表被挖穿
                                public int CaveStartDepth => caveStartDepth;
    }
}