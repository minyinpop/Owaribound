using UnityEngine;
using UnityEngine.Tilemaps;

namespace World.Generator.Config.Ore
{
    [CreateAssetMenu(menuName = "Owaribound/World/Generator/Ore Configs", fileName = "New Name")]
    public sealed class OreConfig : ScriptableObject
    {
        [field: Header("方塊種類")]
        [field: SerializeField] private Block block;
                                public Block Block => block;
        [field: SerializeField] private TileBase blockTile;
                                public TileBase BlockTile => blockTile;
        
        [field: Header("牆壁種類")]
        [field: SerializeField] private Wall wall;
                                public Wall Wall => wall;
        [field: SerializeField] private TileBase wallTile;
                                public TileBase WallTile => wallTile;
        
        [field: Header("生成範圍")]
        [field: SerializeField] private int minDepth = 5;
                                public int MinDepth => minDepth;
        [field: SerializeField] private int maxDepth = 60;
                                public int MaxDepth => maxDepth;
        
        [field: Header("礦物設定")]
        [field: SerializeField] private float scale = .15f; // 越大礦脈越小、越零碎
                                public float Scale => scale;
        [field: SerializeField, Range(0, 1)] private float threshold = .75f; // 越高越稀有
                                             public float Threshold => threshold;
    }
}