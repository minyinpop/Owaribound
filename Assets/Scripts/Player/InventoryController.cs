using System;
using Inventory;
using Inventory.Data;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public sealed class InventoryController : MonoBehaviour
    {
        [field: Header("儲存格設定")]
        [field: SerializeField] private int size = 40;
        
        private PlayerController playerCo;
        
        public InventorySystem InventorySystem { get; private set; }

        private const int HotbarSize = 10;
        
        public int SelectedIndex { get; private set; }
        public ItemSlotData SelectedSlot => InventorySystem[SelectedIndex];
        
        public bool IsHoldingPlaceable => !SelectedSlot.IsEmpty && SelectedSlot.itemConfig.CanPlace;
        public bool IsOpen { get; private set; }

        public event Action<int> OnSelectedChanged;
        public event Action<bool> OnOpenChanged;

        private void Awake()
        {
            playerCo = GetComponent<PlayerController>();
            
            InventorySystem = new InventorySystem(size);
        }

        private void OnEnable()
        {
            playerCo.Input.OnHotbarSelected += Select;
            playerCo.Input.OnMouseScrolled += Scroll;
            playerCo.Input.OnInventoryToggled += ToggleOpen;
        }

        private void OnDisable()
        {
            playerCo.Input.OnHotbarSelected -= Select;
            playerCo.Input.OnMouseScrolled -= Scroll;
            playerCo.Input.OnInventoryToggled -= ToggleOpen;
        }

        private void Select(int index)
        {
            if (index < 0) return;
            if (index >= HotbarSize) return;
            if (index == SelectedIndex) return;
            SelectedIndex = index;
            OnSelectedChanged?.Invoke(SelectedIndex);
        }

        private void Scroll(int direction)
        {
            Select((SelectedIndex + direction + HotbarSize) % HotbarSize);
        }

        public void SetOpen(bool open)
        {
            if (IsOpen == open) return;
            IsOpen = open;
            OnOpenChanged?.Invoke(open);
        }

        private void ToggleOpen()
        {
            SetOpen(!IsOpen);
        }
    }
}