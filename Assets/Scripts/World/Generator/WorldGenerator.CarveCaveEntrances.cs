using UnityEngine;
using World.Data;

namespace World.Generator
{
    public sealed partial class WorldGenerator
    {
        private void CarveCaveEntrances(System.Random rng)
        {
            for (var i = 0; i < world.WorldConfig.EntranceCount; i++)
            {
                int x;
                do x = rng.Next(5, world.WorldConfig.Width - 5);
                while (Mathf.Abs(x - world.WorldConfig.Width / 2) < 10);

                // 這個入口的坡度：在範圍內隨機，越小越緩
                var slope = Mathf.Lerp(world.WorldConfig.EntranceSlope.x, world.WorldConfig.EntranceSlope.y, (float)rng.NextDouble());
                var goRight = rng.Next(2) == 0;

                // 往右下是 -slope，往左下是 -180 + slope
                var baseAngle = goRight ? -slope : -180f + slope;
                var angle = baseAngle;

                var pos = new Vector2(x, world.WorldData.Surface[x]);

                for (var step = 0; step < world.WorldConfig.EntranceLength; step++)
                {
                    CarveCircle(pos, world.WorldConfig.EntranceRadius);

                    // 在基準角度附近擺動，不會偏離太遠
                    angle += (float)(rng.NextDouble() * 2 - 1) * 10f;
                    angle = Mathf.Clamp(angle, baseAngle - world.WorldConfig.EntranceWobble,
                        baseAngle + world.WorldConfig.EntranceWobble);

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
                if (x < 0 || x >= world.WorldConfig.Width || y < 0 || y >= world.WorldConfig.Height)
                    continue;   // 超出地圖

                if ((new Vector2(x, y) - center).sqrMagnitude <= radius * radius)
                    world.WorldData.Blocks[x, y] = BlockType.Air;
            }
        }
    }
}