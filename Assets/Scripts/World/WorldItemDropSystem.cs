using Item;
using Item.Configs;
using UnityEngine;
using World.Configs.Block;

namespace World
{
    public sealed class WorldItemDropSystem : MonoBehaviour
    {
        [field: Header("系統組件")]
        [field: SerializeField] private WorldSystem worldSystem;
        [field: SerializeField] private ItemDrop dropPrefab;

        private void OnEnable()
        {
            worldSystem.OnBlockBroken += BlockBroken;
        }

        private void OnDisable()
        {
            worldSystem.OnBlockBroken -= BlockBroken;
        }

        private void BlockBroken(Vector3Int cell, BlockConfig blockConfig)
        {
            if (!blockConfig.DropItem) return;
            Spawn(blockConfig.DropItem, blockConfig.DropAmount, worldSystem.BlockTilemap.GetCellCenterWorld(cell));
        }

        public ItemDrop Spawn(ItemConfig itemConfig, int count, Vector3 position)
        {
            var drop = Instantiate(dropPrefab, position, Quaternion.identity, transform);
            drop.Initialize(itemConfig, count);
            return drop;
        }

        public void Despawn(ItemDrop drop)
        {
            drop.gameObject.SetActive(false);
            Destroy(drop.gameObject);
        }
    }
}