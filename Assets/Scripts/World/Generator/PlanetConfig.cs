using UnityEngine;
using UnityEngine.Tilemaps;

namespace World.Generator
{
    [CreateAssetMenu(menuName = "Owaribound/World/Generator/Planet Config")]
    public sealed class PlanetConfig : ScriptableObject
    {
        [Header("Size")]
        public int width = 200;
        public int height = 100;

        [Header("Surface")]
        public int baseHeight = 60;       // 地表平均高度
        public int amplitude = 6;         // 起伏幅度（上下各幾格）
        public float frequency = .03f;    // 越小越平緩
        public int dirtDepth = 5;         // 泥土層厚度

        [Header("Tiles")]
        public TileBase dirtTile;
        public TileBase stoneTile;
    }
}