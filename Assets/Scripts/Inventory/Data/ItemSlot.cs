using Item.Configs;

namespace Inventory.Data
{
    [System.Serializable]
    public struct ItemSlot
    {
        public ItemConfig item;
        
        public int count;

        public bool IsEmpty => item == null || count <= 0;
    }
}