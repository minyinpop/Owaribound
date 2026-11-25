using Inventory;
using Player;
using UnityEngine;

namespace UI.Inventory
{
    public sealed class InventoryUI : MonoBehaviour
    {
        [field: Header("玩家組件")]
        [field: SerializeField] private InventoryController inventoryController;
        
        [field: Header("格子組件")]
        [field: SerializeField] private ItemSlotUI[] itemSlotUIs;

        private InventorySystem inventorySystem;

        private bool started;

        private void OnEnable()
        {
            if (!started)
                return;
            
            inventorySystem.OnSlotChanged += RefreshSlot;
            inventoryController.OnSelectedChanged += RefreshSelected;
            
            for (var i = 0; i < itemSlotUIs.Length; i ++)
                itemSlotUIs[i].Refresh(inventorySystem[i]);
        }
        
        private void Start()
        {
            inventorySystem = inventoryController.InventorySystem;
            RefreshSelected(inventoryController.SelectedIndex);
            
            for (var i = 0; i < itemSlotUIs.Length; i ++)
                itemSlotUIs[i].SetNumber((i + 1) % 10);

            started = true;
            OnEnable();
        }

        private void OnDisable()
        {
            inventorySystem.OnSlotChanged -= RefreshSlot;
            inventoryController.OnSelectedChanged -= RefreshSelected;
        }

        private void RefreshSlot(int index)
        {
            if (index < itemSlotUIs.Length)
                itemSlotUIs[index].Refresh(inventorySystem[index]);
        }

        private void RefreshSelected(int index)
        {
            for (var i = 0; i < itemSlotUIs.Length; i ++)
                itemSlotUIs[i].SetSelected(i == index);
        }
    }
}