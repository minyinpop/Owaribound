using System;
using Inventory.Data;
using Item.Configs;

namespace Inventory
{
    public sealed class InventorySystem
    {
        private readonly ItemSlotData[] slots;

        public int Size => slots.Length;
        public ItemSlotData this[int index] => slots[index];

        public event Action<int> OnSlotChanged; // 參數是哪一格變了

        public InventorySystem(int size)
        {
            slots = new ItemSlotData[size];
        }

        /// <summary>
        /// 嘗試添加物品
        /// </summary>
        /// <param name="item"> 物品資料 </param>
        /// <param name="count"> 物品數量 </param>
        /// <returns> 放不下的數量（0 表示全部放進去了） </returns>
        public int TryAdd(ItemConfig item, int count)
        {
            if (item == null || count <= 0)
                return count;
            
            for (var i = 0; i < slots.Length && count > 0; i++)
            {
                if (slots[i].itemConfig != item || slots[i].count >= item.MaxStack)
                    continue;

                var add = Math.Min(count, item.MaxStack - slots[i].count);
                slots[i].count += add;
                count -= add;
                OnSlotChanged?.Invoke(i);
            }

            for (var i = 0; i < slots.Length && count > 0; i++)
            {
                if (!slots[i].IsEmpty)
                    continue;

                var add = Math.Min(count, item.MaxStack);
                slots[i] = new ItemSlotData { itemConfig = item, count = add };
                count -= add;
                OnSlotChanged?.Invoke(i);
            }

            return count;
        }
        
        /// <summary>
        /// 從背包中移除特定的物品
        /// </summary>
        /// <param name="item"> 要移除的物品資料 </param>
        /// <param name="count"> 要移除的數量 </param>
        /// <returns> 回傳是否移除成功（數量不夠就直接回傳 false） </returns>
        public bool TryRemove(ItemConfig item, int count)
        {
            if (item == null || count <= 0)
                return false;
            
            if (CountOf(item) < count)
                return false;

            for (var i = slots.Length - 1; i >= 0 && count > 0; i--)
            {
                if (slots[i].itemConfig != item)
                    continue;

                var remove = Math.Min(count, slots[i].count);
                slots[i].count -= remove;
                count -= remove;

                if (slots[i].count <= 0)
                    slots[i] = ItemSlotData.Empty;

                OnSlotChanged?.Invoke(i);
            }

            return true;
        }
        
        /// <summary>
        /// 從指定的格子裡移除特定數量的物品
        /// </summary>
        /// <param name="index"> 格子的索引 </param>
        /// <param name="count"> 要移除的數量 </param>
        /// <returns> 回傳是否移除成功 </returns>
        public bool TryRemoveAt(int index, int count)
        {
            if (index < 0 || index >= slots.Length) return false;
            if (count <= 0) return false;
            if (slots[index].IsEmpty) return false;
            if (slots[index].count < count) return false;

            slots[index].count -= count;
            if (slots[index].count <= 0)
                slots[index] = ItemSlotData.Empty;
            
            OnSlotChanged?.Invoke(index);
            return true;
        }
        
        /// <summary>
        /// 這個儲存空間有幾種這個物品
        /// </summary>
        /// <param name="item"> 物品資料 </param>
        /// <returns> 回傳相關物品的總數 </returns>
        public int CountOf(ItemConfig item)
        {
            var total = 0;
            foreach (var slot in slots)
                if (slot.itemConfig == item)
                    total += slot.count;
            return total;
        }
    }
}