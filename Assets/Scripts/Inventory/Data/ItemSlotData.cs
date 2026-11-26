using Item.Configs;

namespace Inventory.Data
{
    [System.Serializable]
    public struct ItemSlotData
    {
        public ItemConfig itemConfig;
        
        public int count;

        public bool IsEmpty => itemConfig == null || count <= 0;

        public static readonly ItemSlotData Empty;
    }
}