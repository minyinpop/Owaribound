using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using World.Tile;

// 如果你的命名空間改成 World.Tiles，這裡也要跟著改

namespace Editor
{
    public static class RuleTileBuilder
    {
        // 遮罩位元的方向：1 = 上、2 = 右、4 = 下、8 = 左
        private static readonly Vector3Int[] Directions =
        {
            Vector3Int.up,
            Vector3Int.right,
            Vector3Int.down,
            Vector3Int.left
        };

        [MenuItem("Assets/Owaribound/Create Ground Rule Tile")]
        private static void CreateGround() => CreateForSelection<GroundRuleTile>(Tile.ColliderType.Grid);

        [MenuItem("Assets/Owaribound/Create Wall Rule Tile")]
        private static void CreateWall() => CreateForSelection<WallRuleTile>(Tile.ColliderType.None);

        // 有選到圖片時，選單才能按
        [MenuItem("Assets/Owaribound/Create Ground Rule Tile", true)]
        [MenuItem("Assets/Owaribound/Create Wall Rule Tile", true)]
        private static bool HasTextureSelected() => Selection.objects.OfType<Texture2D>().Any();

        private static void CreateForSelection<T>(Tile.ColliderType collider) where T : RuleTile
        {
            foreach (var texture in Selection.objects.OfType<Texture2D>())
            {
                var path = AssetDatabase.GetAssetPath(texture);

                // 取出這張圖切好的所有 Sprite，照名字後面的編號排序（xxx_0、xxx_1 ...）
                var sprites = AssetDatabase.LoadAllAssetsAtPath(path)
                    .OfType<Sprite>()
                    .OrderBy(s => int.Parse(s.name[(s.name.LastIndexOf('_') + 1)..]))
                    .ToArray();

                if (sprites.Length < 18)
                {
                    Debug.LogError($"{texture.name} 只有 {sprites.Length} 個 Sprite，至少要 18 個（0~15 邊緣、16~17 填充）");
                    continue;
                }

                var tile = ScriptableObject.CreateInstance<T>();
                tile.m_DefaultSprite = sprites[16];
                tile.m_DefaultColliderType = collider;

                // 遮罩 0：四周都是方塊 → 內部，從 0、16、17 隨機選
                tile.m_TilingRules.Add(MakeRule(0, collider, sprites[0], sprites[16], sprites[17]));

                // 遮罩 1~15：對應的邊緣
                for (var mask = 1; mask < 16; mask++)
                    tile.m_TilingRules.Add(MakeRule(mask, collider, sprites[mask]));

                var assetPath = AssetDatabase.GenerateUniqueAssetPath($"Assets/Tiles/{texture.name} Rule Tile.asset");
                AssetDatabase.CreateAsset(tile, assetPath);
                Debug.Log($"已建立 {assetPath}");
            }

            AssetDatabase.SaveAssets();
        }

        private static RuleTile.TilingRule MakeRule(int mask, Tile.ColliderType collider, params Sprite[] sprites)
        {
            return new RuleTile.TilingRule
            {
                m_Sprites = sprites,
                m_ColliderType = collider,
                m_Output = sprites.Length > 1
                    ? RuleTile.TilingRuleOutput.OutputSprite.Random
                    : RuleTile.TilingRuleOutput.OutputSprite.Single,

                // 只檢查上右下左四個方向，斜角不管
                m_NeighborPositions = new List<Vector3Int>(Directions),

                // 該方向的位元是 1 → 那邊是空的（NotThis），是 0 → 那邊有方塊（This）
                m_Neighbors = Directions
                    .Select((_, i) => (mask & (1 << i)) != 0
                        ? RuleTile.TilingRuleOutput.Neighbor.NotThis
                        : RuleTile.TilingRuleOutput.Neighbor.This)
                    .ToList()
            };
        }
    }
}