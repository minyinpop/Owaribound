using Item.Configs;
using UnityEngine;

namespace Item
{
    public sealed class ItemDrop : MonoBehaviour
    {
        [field: Header("自身組件")]
        [field: SerializeField] private SpriteRenderer spriteRenderer;
        [field: SerializeField] private Rigidbody2D rigidbody;
        
        [field: Header("屬性設定")]
        [field: SerializeField] private float popForce = 3;

        public ItemConfig ItemConfig { get; private set; }
        public int Count { get; private set; }

        public void Initialize(ItemConfig itemConfig, int count)
        {
            ItemConfig = itemConfig;
            Count = count;

            spriteRenderer.sprite = ItemConfig.Icon;
            rigidbody.linearVelocity = new Vector2(Random.Range(-.5f, .5f), 1) * popForce;
        }

        public void SetCount(int count)
        {
            Count = count;
        }
    }
}