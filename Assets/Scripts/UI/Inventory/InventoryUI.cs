using Inventory;
using Player;
using UnityEngine;

namespace UI.Inventory
{
    public sealed class InventoryUI : MonoBehaviour
    {
        [field: SerializeField] private InventoryController inventoryCo;
        [field: SerializeField] private ItemSlotUI[] itemSlotUIs;

        private InventorySystem inventorySystem;

        private void Awake()
        {
            inventorySystem = inventoryCo.InventorySystem;
        }

        private void OnEnable()
        {
            inventorySystem.OnSlotChanged += RefreshSlot;
        }

        private void OnDisable()
        {
            inventorySystem.OnSlotChanged -= RefreshSlot;
        }

        private void RefreshSlot(int index)
        {
            if (index < itemSlotUIs.Length)
                itemSlotUIs[index].Refresh(inventorySystem[index]);
        }        
    }
}