using Item.Configs;
using UnityEngine;
using World.Configs.Block;

namespace Item
{
    public sealed class ItemDrop : MonoBehaviour
    {
        [field: Header("自身組件")]
        [field: SerializeField] private SpriteRenderer spr;
        [field: SerializeField] private Rigidbody2D rig;
        
        [field: Header("屬性設定")]
        [field: Tooltip("物品掉落時的跳躍力道")]
        [field: SerializeField] private float popForce = 3;
        
        public ItemConfig ItemConfig { get; private set; }
        public int Count { get; private set; }
        
        public float PickupableTime { get; private set; }
        public bool CanPickup => Time.time >= PickupableTime;

        public void Initialize(ItemConfig itemConfig, int count)
        {
            ItemConfig = itemConfig;
            Count = count;

            spr.sprite = ItemConfig.Icon;
            rig.linearVelocity = new Vector2(Random.Range(-.5f, .5f), 1) * popForce;
        }

        public void SetCount(int count)
        {
            Count = count;
        }

        public void SetPickupDelay(float delay)
        {
            PickupableTime = Time.time + delay;
        }
    }
}