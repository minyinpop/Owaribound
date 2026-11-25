using System.Collections.Generic;
using UnityEngine;

namespace World
{
    [RequireComponent(typeof(WorldSystem))]
    public sealed class WorldDamageSystem : MonoBehaviour
    {
        private WorldSystem world;
        
        private struct Damage
        {
            public float Amount;      // 累積的傷害（以秒計）
            public float LastHitTime; // 最後一次被打的時間
        }

        private readonly Dictionary<Vector3Int, Damage> damaged = new();
        private readonly List<Vector3Int> toRecover = new();
        
        private void Awake()
        {
            world = GetComponent<WorldSystem>();
        }

        private void Update()
        {
            // 找出太久沒被打的格子
            toRecover.Clear();
            foreach (var (cell, damage) in damaged)
            {
                if (Time.time - damage.LastHitTime > world.RecoverDelay)
                    toRecover.Add(cell);
            }

            // 迴圈跑完才刪，跑 foreach 時不能修改 Dictionary
            foreach (var cell in toRecover)
                Clear(cell);
        }
        
        // 對一格造成傷害，挖掉回傳 true
        public bool ApplyDamage(Vector3Int cell, float amount)
        {
            var blockConfig = world.GetBlockConfig(cell);
            if (!blockConfig)
                return false;   // 空氣或地圖外

            damaged.TryGetValue(cell, out var damage);
            damage.Amount += amount;
            damage.LastHitTime = Time.time;

            // 傷害夠了 → 挖掉
            if (damage.Amount >= blockConfig.MiningTime)
            {
                Clear(cell);
                world.TryBreakBlock(cell);
                return true;
            }

            damaged[cell] = damage;
            UpdateCrack(cell, damage.Amount / blockConfig.MiningTime);
            return false;
        }

        private void UpdateCrack(Vector3Int cell, float ratio)
        {
            // 0.25 → 0，0.5 → 1，0.75 → 2，不到 0.25 → -1
            var stage = Mathf.Min(Mathf.FloorToInt(ratio * 4) - 1, world.CrackTiles.Length - 1);
            world.CrackTilemap.SetTile(cell, stage >= 0 ? world.CrackTiles[stage] : null);
        }

        private void Clear(Vector3Int cell)
        {
            damaged.Remove(cell);
            world.CrackTilemap.SetTile(cell, null);
        }
        
        
    }
}