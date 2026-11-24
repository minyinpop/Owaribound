using UnityEngine;

namespace Item.Configs
{
    [CreateAssetMenu(menuName = "Owaribound/Item/Item Config", fileName = "New Item Config")]
    public sealed class ItemConfig : ScriptableObject
    {
        [field: Header("物品資訊")]
        [field: SerializeField] private string displayName;
                                public string DisplayName => displayName;
        [field: SerializeField] private Sprite icon;
                                public Sprite Icon => icon;
        [field: SerializeField] private int maxStack = 64;
                                public int MaxStack => maxStack;
    }
}