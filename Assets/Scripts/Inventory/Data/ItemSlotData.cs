using Item.Configs;

namespace Inventory.Data
{
    [System.Serializable]
    public struct ItemSlotData
    {
        public ItemConfig item;
        
        public int count;

        public bool IsEmpty => item == null || count <= 0;

        public static readonly ItemSlotData Empty = default;
    }
}