namespace World.Generator.Data
{
    public sealed class WorldData
    {
        public int Width { get; }
        public int Height { get; }
        public Block[,] Blocks { get; }   // 整張地圖的資料
        public Wall[,] Walls { get; }
        public int[] Surface { get; }     // 每一欄的地表高度

        public WorldData(int width, int height)
        {
            Width = width;
            Height = height;
            Blocks = new Block[width, height];
            Walls = new Wall[width, height];
            Surface = new int[width];
        }

        public bool InBounds(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;

        public int DepthOf(int x, int y) => Surface[x] - 1 - y;   // 距離地表幾格
    }
}