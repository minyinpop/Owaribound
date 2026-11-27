using System.Collections.Generic;
using UnityEngine;
using World.Data;

namespace World
{
    [RequireComponent(typeof(WorldSystem))]
    public sealed class WorldDamageSystem : MonoBehaviour
    {
        private WorldSystem worldSystem;
        
        private struct Damage
        {
            public float Amount;
            public float LastHitTime;
        }

        private readonly Dictionary<(Vector3Int cell, WorldLayer layer), Damage> damaged = new();
        private readonly List<(Vector3Int cell, WorldLayer layer)> toRecover = new();
        
        private void Awake()
        {
            worldSystem = GetComponent<WorldSystem>();
        }

        private void Update()
        {
            toRecover.Clear();
            foreach (var (key, damage) in damaged)
            {
                if (Time.time - damage.LastHitTime > worldSystem.RecoverDelay)
                    toRecover.Add(key);
            }

            foreach (var key in toRecover)
                Clear(key);
        }
        
        // 對一格造成傷害，挖掉回傳 true
        public bool ApplyDamage(Vector3Int cell, WorldLayer layer, float amount)
        {
            var config = layer == WorldLayer.Block
                ? worldSystem.GetBlockConfig(cell)
                : worldSystem.GetWallConfig(cell);
            if (!config) return false;

            var key = (cell, layer);
            damaged.TryGetValue(key, out var damage);
            damage.Amount += amount;
            damage.LastHitTime = Time.time;

            if (damage.Amount >= config.MiningTime)
            {
                Clear(key);
                worldSystem.TryBreakBlock(cell);
                return true;
            }

            damaged[key] = damage;
            UpdateCrack(key, damage.Amount / config.MiningTime);
            return false;
        }

        private void UpdateCrack((Vector3Int cell, WorldLayer layer) key, float ratio)
        {
            // 0.25 → 0，0.5 → 1，0.75 → 2，不到 0.25 → -1
            var stage = Mathf.Min(Mathf.FloorToInt(ratio * 4) - 1, worldSystem.CrackTiles.Length - 1);
            worldSystem.GetCrackTilemap(key.layer).SetTile(key.cell, stage >= 0 ? worldSystem.CrackTiles[stage] : null);
        }

        private void Clear((Vector3Int cell, WorldLayer layer) key)
        {
            damaged.Remove(key);
            worldSystem.GetCrackTilemap(key.layer).SetTile(key.cell, null);
        }
        
        
    }
}