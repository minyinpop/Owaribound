using UnityEngine;
using World;
using World.Data;

namespace Player.Interaction
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(InventoryController))]
    [RequireComponent(typeof(CursorController))]
    public sealed class MiningController : MonoBehaviour
    {
        [field: Header("世界組件")]
        [field: SerializeField] private WorldSystem worldSystem;
        
        // ================================
        
        [field: Header("挖掘參數")]
        [field: Tooltip("玩家在破壞方塊時的力度，數值越高破壞的越快")]
        [field: SerializeField] private float miningPower = 1;
        
        // ================================
        
        private PlayerController playerController;
        private InventoryController inventoryController;
        private CursorController cursorController;

        private bool waitPrimaryRelease;
        private bool waitSecondaryRelease;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            inventoryController = GetComponent<InventoryController>();
            cursorController = GetComponent<CursorController>();
        }

        private void Update()
        {
            TryMine(playerController.Input.IsPrimaryHeld, ref waitPrimaryRelease, WorldLayer.Block);
            TryMine(playerController.Input.IsSecondaryHeld, ref waitSecondaryRelease, WorldLayer.Wall);
        }

        private void TryMine(bool isHeld, ref bool waitForRelease, WorldLayer layer)
        {
            if (cursorController.IsOverUI) return;
            if (!isHeld)
            {
                waitForRelease = false;
                return;
            }

            if (inventoryController.IsHoldingPlaceable)
            {
                waitForRelease = true;
                return;
            }

            if (waitForRelease) return;
            if (!cursorController.InReach) return;

            worldSystem.WorldDamageSystem.ApplyDamage(cursorController.Cell, layer, miningPower * Time.deltaTime);
        }
    }
}