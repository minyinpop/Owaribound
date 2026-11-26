using UnityEngine;
using World.Configs.Block;

namespace Item.Configs
{
    [CreateAssetMenu(menuName = "Owaribound/Item/Item Config", fileName = "New Item Config")]
    public sealed class ItemConfig : ScriptableObject
    {
        [field: Header("物品資訊")]
        [field: Tooltip("物品的顯示名稱")]
        [field: SerializeField] private string displayName;
                                public string DisplayName => displayName;
                                
        [field: Tooltip("物品的標示")]
        [field: SerializeField] private Sprite icon;
                                public Sprite Icon => icon;
                                
        [field: Tooltip("物品的最大堆疊上限")]
        [field: SerializeField] private int maxStack = 64;
                                public int MaxStack => maxStack;
                                
        // ================================
                                
        [field: Header("放置設定")]
        [field: Tooltip("這個物品放置時候變成什麼方塊，空著代表不能放置")]
        [field: SerializeField] private BlockConfig placeBlock;
                                public BlockConfig PlaceBlock => placeBlock;
                                
        public bool CanPlace => placeBlock != null;
    }
}