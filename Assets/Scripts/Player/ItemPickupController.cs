using System.Collections.Generic;
using Item;
using UnityEngine;
using World;

namespace Player
{
    [RequireComponent(typeof(InventoryController))]
    public sealed class ItemPickupController : MonoBehaviour
    {
        [field: Header("世界系統")]
        [field: SerializeField] private WorldItemDropSystem dropSystem;

        [field: Header("參數設定")]
        [field: SerializeField] private float pickupRadius = 1;
        [field: SerializeField] private LayerMask dropLayer;
        
        private InventoryController inventoryController;
        private ContactFilter2D filter;
        private readonly List<Collider2D> hits = new();

        private void Awake()
        {
            inventoryController = GetComponent<InventoryController>();
            filter = new ContactFilter2D
            {
                useLayerMask = true,
                layerMask = dropLayer
            };
        }

        private void FixedUpdate()
        {
            var center = (Vector2)transform.position + Vector2.up;
            Physics2D.OverlapCircle(center, pickupRadius, filter, hits);

            foreach (var hit in hits)
            {
                if (!hit.TryGetComponent(out ItemDrop dropItem)) continue;

                var left = inventoryController.InventorySystem.TryAdd(dropItem.ItemConfig, dropItem.Count);
                
                if (left == 0)
                    dropSystem.Despawn(dropItem);
                else
                    dropItem.SetCount(left);
            }
        }
        
        
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position + Vector3.up, pickupRadius);
        }
    }
}