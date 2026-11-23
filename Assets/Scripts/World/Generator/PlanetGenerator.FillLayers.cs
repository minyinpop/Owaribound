namespace World.Generator
{
    public sealed partial class PlanetGenerator
    {
        private void FillLayers()
        {
            for (var x = 0; x < config.Width; x++)
            for (var y = 0; y < surface[x]; y++)
            {
                var depth = surface[x] - 1 - y; // 距離地表幾格
                var isDirt = depth < config.GrassDepth;
                        
                blocks[x, y]   = isDirt ? Block.Dirt : Block.Stone;
                walls[x, y] = isDirt ? Wall.Dirt : Wall.Stone;
            }
        }
    }
}