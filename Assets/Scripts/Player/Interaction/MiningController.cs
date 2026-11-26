using UnityEngine;
using World;

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

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            inventoryController = GetComponent<InventoryController>();
            cursorController = GetComponent<CursorController>();
        }

        private void Update()
        {
            if (!playerController.Input.IsPrimaryHeld) return;
            if (inventoryController.IsHoldingPlaceable) return;
            if (!cursorController.InReach) return;

            worldSystem.WorldDamageSystem.ApplyDamage(cursorController.Cell, miningPower * Time.deltaTime);
        }
    }
}