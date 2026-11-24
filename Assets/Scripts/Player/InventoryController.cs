using Inventory;
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

        private void Awake()
        {
            playerCo = GetComponent<PlayerController>();
            
            InventorySystem = new InventorySystem(size);
        }
    }
}