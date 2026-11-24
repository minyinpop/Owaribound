using UnityEngine;
using UnityEngine.Tilemaps;
using World.Generator.Data;

namespace World.Generator.Block_Config
{
    public class BlockConfig : ScriptableObject
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

        [field: Header("方塊屬性")]
        [field: SerializeField] private float miningTime;
                                public float MiningTime => miningTime;
    }
}