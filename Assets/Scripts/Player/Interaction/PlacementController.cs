using UnityEngine;
using World;

namespace Player.Interaction
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(InventoryController))]
    [RequireComponent(typeof(CursorController))]
    public sealed class PlacementController : MonoBehaviour
    {
        [field: Header("世界組件")]
        [field: SerializeField] private WorldSystem worldSystem;
        
        // ================================
        
        [field: Header("放置參數")]
        [field: Tooltip("放置方塊後，需要在幾秒後才能再次放置")]
        [field: SerializeField] private float placeCooldown = .12f;
        
        // ================================
        
        private PlayerController playerController;
        private InventoryController inventoryController;
        private CursorController cursorController;

        private float nextPlaceTime;
        
        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            inventoryController = GetComponent<InventoryController>();
            cursorController = GetComponent<CursorController>();
        }

        private void Update()
        {
            if (!inventoryController.IsHoldingPlaceable) return;
            if (!cursorController.InReach) return;
            if (Time.time < nextPlaceTime) return;

            var cell = cursorController.Cell;
            var block = inventoryController.SelectedSlot.itemConfig.PlaceBlock;

            var placed = false;
            
            if (playerController.Input.IsPrimaryHeld)
                placed = worldSystem.TryPlaceBlock(cell, block);
            else if (playerController.Input.IsSecondaryHeld)
                placed = worldSystem.TryPlaceWall(cell, block);

            if (placed) return;

            inventoryController.InventorySystem.TryRemoveAt(inventoryController.SelectedIndex, 1);
            nextPlaceTime = Time.time + placeCooldown;
        }
    }
}