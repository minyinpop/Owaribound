using System;
using Inventory;
using Inventory.Data;
using Player.Interaction;
using UnityEngine;
using World;

namespace Player
{
    [DefaultExecutionOrder(10)]
    [RequireComponent(typeof(PlayerController))]
    public sealed class InventoryController : MonoBehaviour
    {
        [field: Header("儲存格設定")]
        [field: SerializeField] private int size = 40;
        
        // ================================
        
        [field: Header("丟物品設定")]
        [field: SerializeField] private WorldItemDropSystem dropSystem;
        [field: SerializeField] private float throwForce = 5;
        [field: SerializeField] private float throwPickupDelay = 1.5f;
        
        // ================================
        
        private PlayerController playerCo;
        private CursorController cursorController;
        
        // ================================
        
        public InventorySystem InventorySystem { get; private set; }
        
        // ================================

        public const int HotbarSize = 10;
        
        public int SelectedIndex { get; private set; }
        public ItemSlotData SelectedSlot => InventorySystem[SelectedIndex];
        
        // ================================
        
        public bool IsHoldingPlaceable => !SelectedSlot.IsEmpty && SelectedSlot.itemConfig.CanPlace;
        public bool IsOpen { get; private set; }
        public bool IsHolding => !heldSlot.IsEmpty;
        
        // ================================

        public event Action<int> OnSelectedChanged;
        public event Action<bool> OnOpenChanged;
        
        // ================================

        private ItemSlotData heldSlot;
        public ItemSlotData HeldSlot => heldSlot;
        
        // ================================
        
        public event Action OnHeldChanged;

        private void Awake()
        {
            playerCo = GetComponent<PlayerController>();
            cursorController = GetComponent<CursorController>();
            
            InventorySystem = new InventorySystem(size);
        }

        private void OnEnable()
        {
            playerCo.Input.OnHotbarSelected += Select;
            playerCo.Input.OnMouseScrolled += Scroll;
            playerCo.Input.OnInventoryToggled += ToggleOpen;
        }

        private void Update()
        {
            if (!IsHolding) return;
            if (!playerCo.Input.IsPrimaryPressed && !playerCo.Input.IsSecondaryPressed) return;
            if (cursorController.IsOverUI) return;

            DropHeld();
        }

        private void OnDisable()
        {
            playerCo.Input.OnHotbarSelected -= Select;
            playerCo.Input.OnMouseScrolled -= Scroll;
            playerCo.Input.OnInventoryToggled -= ToggleOpen;
        }

        public void Select(int index)
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

        public void ClickSlot(int index)
        {
            InventorySystem.ClickSlot(index, ref heldSlot);
            OnHeldChanged?.Invoke();
        }

        public void DropHeld()
        {
            if (!IsHolding) return;

            var origin = transform.position + Vector3.up;
            var dir = Mathf.Sign(cursorController.MouseWorldPos.x - origin.x);

            var drop = dropSystem.Spawn(heldSlot.itemConfig, heldSlot.count, origin);
            drop.Rig.linearVelocity = new Vector2(dir, .5f) * throwForce;
            drop.SetPickupDelay(throwPickupDelay);

            heldSlot = ItemSlotData.Empty;
            OnHeldChanged?.Invoke();
        }
    }
}