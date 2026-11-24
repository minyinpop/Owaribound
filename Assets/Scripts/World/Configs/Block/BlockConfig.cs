using UnityEngine;
using UnityEngine.Tilemaps;
using World.Data;

namespace World.Configs.Block
{
    public class BlockConfig : ScriptableObject
    {
        [field: Header("方塊種類")]
        [field: SerializeField] private Data.BlockType blockType;
                                public Data.BlockType BlockType => blockType;
        [field: SerializeField] private TileBase blockTile;
                                public TileBase BlockTile => blockTile;
                                
        [field: Header("牆壁種類")]
        [field: SerializeField] private WallType wallType;
                                public WallType WallType => wallType;
        [field: SerializeField] private TileBase wallTile;
                                public TileBase WallTile => wallTile;

        [field: Header("方塊屬性")]
        [field: SerializeField] private float miningTime;
                                public float MiningTime => miningTime;
    }
}