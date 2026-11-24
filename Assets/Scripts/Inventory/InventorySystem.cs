using System;
using Inventory.Data;
using Item.Configs;

namespace Inventory
{
    public sealed class InventorySystem
    {
        private readonly ItemSlot[] slots;

        public int Size => slots.Length;
        public ItemSlot this[int index] => slots[index];

        public event Action<int> OnSlotChanged; // 參數是哪一格變了

        public InventorySystem(int size)
        {
            slots = new ItemSlot[size];
        }

        /// <summary>
        /// 嘗試添加物品
        /// </summary>
        /// <param name="item"> 物品資料 </param>
        /// <param name="count"> 物品數量 </param>
        /// <returns> 放不下的數量（0 表示全部放進去了） </returns>
        public int TryAdd(ItemConfig item, int count)
        {
            // 第 1 輪：先疊到已經有同樣物品、還沒滿的格子
            for (var i = 0; i < slots.Length && count > 0; i++)
            {
                if (slots[i].Item != item || slots[i].Count >= item.MaxStack)
                    continue;

                var add = Math.Min(count, item.MaxStack - slots[i].Count);
                slots[i].Count += add;
                count -= add;
                OnSlotChanged?.Invoke(i);
            }

            // 第 2 輪：放到空格
            for (var i = 0; i < slots.Length && count > 0; i++)
            {
                if (!slots[i].IsEmpty)
                    continue;

                var add = Math.Min(count, item.MaxStack);
                slots[i] = new ItemSlot { Item = item, Count = add };
                count -= add;
                OnSlotChanged?.Invoke(i);
            }

            return count;
        }
        
        /// <summary>
        /// 嘗試移除物品
        /// </summary>
        /// <param name="item"> 物品資料 </param>
        /// <param name="count"> 物品數量 </param>
        /// <returns> 回傳是否移除成功（數量不夠就直接回傳 flase） </returns>
        public bool TryRemove(ItemConfig item, int count)
        {
            if (CountOf(item) < count)
                return false;

            for (var i = slots.Length - 1; i >= 0 && count > 0; i--)
            {
                if (slots[i].Item != item)
                    continue;

                var remove = Math.Min(count, slots[i].Count);
                slots[i].Count -= remove;
                count -= remove;

                if (slots[i].Count <= 0)
                    slots[i] = ItemSlot.Empty;

                OnSlotChanged?.Invoke(i);
            }

            return true;
        }

        /// <summary>
        /// 這個儲存空間有幾種這個物品
        /// </summary>
        /// <param name="item"> 物品資料 </param>
        /// <returns> 返回相關物品的總數 </returns>
        public int CountOf(ItemConfig item)
        {
            var total = 0;
            foreach (var slot in slots)
                if (slot.Item == item)
                    total += slot.Count;
            return total;
        }
    }
}