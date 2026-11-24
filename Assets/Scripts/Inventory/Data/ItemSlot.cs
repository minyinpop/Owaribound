using Item.Configs;

namespace Inventory.Data
{
    [System.Serializable]
    public struct ItemSlot
    {
        public ItemConfig Item;
        public int Count;

        public bool IsEmpty => Count == null || Count <= 0;

        public static readonly ItemSlot Empty = default;
    }
}