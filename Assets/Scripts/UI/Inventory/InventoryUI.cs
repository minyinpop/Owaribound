using Inventory;
using Player;
using UnityEngine;

namespace UI.Inventory
{
    public sealed class InventoryUI : MonoBehaviour
    {
        [field: Header("玩家組件")]
        [field: SerializeField] private InventoryController inventoryController;
        
        // ================================
        
        [field: Header("格子組件")]
        [field: SerializeField] private ItemSlotUI[] itemSlotUIs;
        
        // ================================
        
        [field: Header("")]
        [field: SerializeField] private GameObject expandedRows;

        private InventorySystem inventorySystem;

        private bool started;

        private void OnEnable()
        {
            if (!started) return;
            
            inventorySystem.OnSlotChanged += RefreshSlot;
            inventoryController.OnSelectedChanged += RefreshSelected;
            inventoryController.OnOpenChanged += RefreshOpen;
            
            RefreshSelected(inventoryController.SelectedIndex);
            RefreshOpen(inventoryController.IsOpen);
            
            for (var i = 0; i < itemSlotUIs.Length; i ++)
                itemSlotUIs[i].Refresh(inventorySystem[i]);
        }
        
        private void Start()
        {
            inventorySystem = inventoryController.InventorySystem;
            
            for (var i = 0; i < itemSlotUIs.Length; i ++)
                itemSlotUIs[i].SetNumber(i < 10 ? (i + 1) % 10 : -1);

            started = true;
            OnEnable();
        }

        private void OnDisable()
        {
            if (!started) return;
            
            inventorySystem.OnSlotChanged -= RefreshSlot;
            inventoryController.OnSelectedChanged -= RefreshSelected;
            inventoryController.OnOpenChanged -= RefreshOpen;
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

        private void RefreshOpen(bool open)
        {
            expandedRows.SetActive(open);
        }
    }
}