using UnityEngine;
using UnityEngine.Tilemaps;

namespace World.Generator
{
    public enum Block : byte { Air, Dirt, Stone }

    public sealed class PlanetGenerator : MonoBehaviour
    {
        [SerializeField] private PlanetConfig config;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private int seed;

        private Block[,] map;      // 整張地圖的資料
        private int[] surface;     // 每一欄的地表高度，之後放建築、出生點都會用到

        [ContextMenu("Generate")]
        public void Generate()
        {
            map = new Block[config.width, config.height];

            GenerateSurface(new System.Random(seed));
            FillLayers();
            // 之後的步驟加在這裡：
            // CarveCaves(new System.Random(seed + 1));
            // PlaceOres(new System.Random(seed + 2));

            Render();
        }

        // 步驟 1：用雜訊算出每一欄的地表高度
        private void GenerateSurface(System.Random rng)
        {
            surface = new int[config.width];
            var offset = (float)(rng.NextDouble() * 10000);

            for (var x = 0; x < config.width; x++)
            {
                var big   = Mathf.PerlinNoise(offset + x * config.frequency, 0f);          // 大山丘
                var small = Mathf.PerlinNoise(offset + x * config.frequency * 4f, 100f);   // 小起伏（頻率 4 倍）
                var noise = big * .8f + small * .2f;                                // 大的佔 80%
                var h = config.baseHeight + Mathf.RoundToInt((noise * 2f - 1f) * config.amplitude);
                surface[x] = Mathf.Clamp(h, 1, config.height - 1);
            }
        }

        // 步驟 2：地表以下，上面幾格放泥土，再往下放岩石
        private void FillLayers()
        {
            for (var x = 0; x < config.width; x++)
            for (var y = 0; y < surface[x]; y++)
            {
                var depth = surface[x] - 1 - y;            // 距離地表幾格
                map[x, y] = depth < config.dirtDepth ? Block.Dirt : Block.Stone;
            }
        }

        // 最後：把資料轉成 Tile，一次打進 Tilemap
        private void Render()
        {
            var w = config.width;
            var h = config.height;
            var tiles = new TileBase[w * h];

            for (var x = 0; x < w; x++)
            for (var y = 0; y < h; y++)
                tiles[x + y * w] = ToTile(map[x, y]);

            tilemap.ClearAllTiles();
            tilemap.SetTilesBlock(new BoundsInt(0, 0, 0, w, h, 1), tiles);
        }

        private TileBase ToTile(Block block) => block switch
        {
            Block.Dirt  => config.dirtTile,
            Block.Stone => config.stoneTile,
            _ => null      // Air
        };
    }
}