using UnityEngine;

namespace World.Configs.Block.Ore
{
    [CreateAssetMenu(menuName = "Owaribound/World/Generator/Ore Configs", fileName = "New Ore Config")]
    public sealed class OreConfigs : BlockConfigs
    {
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