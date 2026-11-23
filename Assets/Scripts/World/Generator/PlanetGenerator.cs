using UnityEngine;
using UnityEngine.Tilemaps;

namespace World.Generator
{
    public enum Block : byte { Air, Dirt, Stone }
    public enum Wall : byte { None, Dirt, Stone }

    public sealed class PlanetGenerator : MonoBehaviour
    {
        [field: Header("Planet Settings")]
        [field: SerializeField] private PlanetConfig config;
        [field: SerializeField] private Tilemap blockTilemap;
        [field: SerializeField] private Tilemap wallTilemap;
        [field: SerializeField] private int seed;
        
        [field: Header("Player Settings")]
        [field: SerializeField] private Transform player;

        private Block[,] blocks; // 整張地圖的資料
        private Wall[,] walls;
        private int[] surface; // 每一欄的地表高度，之後放建築、出生點都會用到
        
        [ContextMenu("Generator")]
        private void Start()
        {
            Generate();
            SpawnPlayer(config.Width / 2);
        }

        #region 生成世界
            private void Generate()
            {
                blocks = new Block[config.Width, config.Height];
                walls = new Wall[config.Width, config.Height];

                GenerateSurface(new System.Random(seed));
                FillLayers();
                CarveCaves(new System.Random(seed + 1));
                CarveCaveEntrances(new System.Random(seed + 2));
                // 之後的步驟加在這裡：
                // PlaceOres(new System.Random(seed + 2));

                Render();
            }

            #region 步驟 1：用雜訊算出每一欄的地表高度
                private void GenerateSurface(System.Random rng)
                {
                    surface = new int[config.Width];
                    var offset = (float)(rng.NextDouble() * 10000);

                    for (var x = 0; x < config.Width; x++)
                    {
                        var big   = Mathf.PerlinNoise(offset + x * config.Frequency, 0f);        // 大山丘
                        var small = Mathf.PerlinNoise(offset + x * config.Frequency * 4f, 100f); // 小起伏（頻率 4 倍）
                        var noise = big * .8f + small * .2f;                                     // 大的佔 80%
                        var h = config.BaseHeight + Mathf.RoundToInt((noise * 2f - 1f) * config.Amplitude);
                        surface[x] = Mathf.Clamp(h, 1, config.Height - 1);
                    }
                }
            #endregion

            #region 步驟 2：地表以下，上面幾格放泥土，再往下放岩石
                private void FillLayers()
                {
                    for (var x = 0; x < config.Width; x++)
                    for (var y = 0; y < surface[x]; y++)
                    {
                        var depth = surface[x] - 1 - y; // 距離地表幾格
                        var isDirt = depth < config.DirtDepth;
                        
                        blocks[x, y]   = isDirt ? Block.Dirt : Block.Stone;
                        walls[x, y] = isDirt ? Wall.Dirt : Wall.Stone;
                    }
                }
            #endregion
            
            #region 步驟 3：用 2D 雜訊挖出洞穴
            private void CarveCaves(System.Random rng)
            {
                var offsetX = (float)(rng.NextDouble() * 10000);
                var offsetY = (float)(rng.NextDouble() * 10000);

                for (var x = 0; x < config.Width; x++)
                for (var y = 0; y < surface[x]; y++)
                {
                    var depth = surface[x] - 1 - y;
                    if (depth < config.CaveStartDepth)
                        continue; // 太靠近地表，不挖

                    var noise = Mathf.PerlinNoise(offsetX + x * config.CaveScale,
                        offsetY + y * config.CaveScale);

                    if (noise > config.CaveThreshold)
                        blocks[x, y] = Block.Air;
                }
            }
            #endregion
            
            #region 步驟 4：挖空隨機個洞穴的入口
                private void CarveCaveEntrances(System.Random rng)
                {
                    for (var i = 0; i < config.EntranceCount; i++)
                    {
                        int x;
                        do x = rng.Next(5, config.Width - 5);
                        while (Mathf.Abs(x - config.Width / 2) < 10);

                        // 這個入口的坡度：在範圍內隨機，越小越緩
                        var slope = Mathf.Lerp(config.EntranceSlope.x, config.EntranceSlope.y, (float)rng.NextDouble());
                        var goRight = rng.Next(2) == 0;

                        // 往右下是 -slope，往左下是 -180 + slope
                        var baseAngle = goRight ? -slope : -180f + slope;
                        var angle = baseAngle;

                        var pos = new Vector2(x, surface[x]);

                        for (var step = 0; step < config.EntranceLength; step++)
                        {
                            CarveCircle(pos, config.EntranceRadius);

                            // 在基準角度附近擺動，不會偏離太遠
                            angle += (float)(rng.NextDouble() * 2 - 1) * 10f;
                            angle = Mathf.Clamp(angle, baseAngle - config.EntranceWobble,
                                baseAngle + config.EntranceWobble);

                            var rad = angle * Mathf.Deg2Rad;
                            pos += new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                        }
                    }
                }
                
                // 以 center 為中心，挖掉半徑 radius 內的方塊（牆不動）
                private void CarveCircle(Vector2 center, float radius)
                {
                    var r  = Mathf.CeilToInt(radius);
                    var cx = Mathf.RoundToInt(center.x);
                    var cy = Mathf.RoundToInt(center.y);

                    for (var x = cx - r; x <= cx + r; x++)
                    for (var y = cy - r; y <= cy + r; y++)
                    {
                        if (x < 0 || x >= config.Width || y < 0 || y >= config.Height)
                            continue;   // 超出地圖

                        if ((new Vector2(x, y) - center).sqrMagnitude <= radius * radius)
                            blocks[x, y] = Block.Air;
                    }
                }
            #endregion

            #region 最後：把資料轉成 Tile，一次打進 Tilemap
                private void Render()
                {
                    var w = config.Width;
                    var h = config.Height;
                    var blockTiles = new TileBase[w * h];
                    var wallTiles = new TileBase[w * h];

                    for (var x = 0; x < w; x++)
                    for (var y = 0; y < h; y++)
                    {
                        blockTiles[x + y * w] = ToBlockTile(blocks[x, y]);
                        wallTiles[x + y * w] = ToWallTile(walls[x, y]);
                    }

                    var bounds = new BoundsInt(0, 0, 0, w, h, 1);

                    blockTilemap.ClearAllTiles();
                    blockTilemap.SetTilesBlock(bounds, blockTiles);
                    
                    wallTilemap.ClearAllTiles();
                    wallTilemap.SetTilesBlock(bounds, wallTiles);
                }

                private TileBase ToBlockTile(Block block) => block switch
                {
                    Block.Dirt  => config.DirtBlockTile,
                    Block.Stone => config.StoneBlockTile,
                    _ => null // Air
                };
                
                private TileBase ToWallTile(Wall wall) => wall switch
                {
                    Wall.Dirt  => config.DirtWallTile,
                    Wall.Stone => config.StoneWallTile,
                    _ => null // Air
                };
            #endregion
        #endregion
        
        #region 生成角色
            private void SpawnPlayer(int x)
            {
                // surface[x] 是這一欄的高度，最上面那格實心方塊的頂部就在 y = surface[x]
                var cell = new Vector3Int(x, surface[x], 0);
                var pos = blockTilemap.CellToWorld(cell) + new Vector3(blockTilemap.cellSize.x / 2f, 0, 0);

                player.position = pos;
            }
        #endregion
    }
}