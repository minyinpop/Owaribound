using UnityEngine;

namespace World.Generator
{
    public sealed partial class WorldGenerator
    {
        private void GenerateSurface(System.Random rng)
        {
            var offset = (float)(rng.NextDouble() * 10000);

            for (var x = 0; x < world.WorldConfig.Width; x++)
            {
                var big   = Mathf.PerlinNoise(offset + x * world.WorldConfig.Frequency, 0f);
                var small = Mathf.PerlinNoise(offset + x * world.WorldConfig.Frequency * 4f, 100f);
                var noise = big * .8f + small * .2f;

                // 離中間越近，起伏越小
                var dist = Mathf.Abs(x - world.WorldConfig.Width / 2);
                var t = Mathf.InverseLerp(world.WorldConfig.SpawnFlatRadius,
                    world.WorldConfig.SpawnFlatRadius + world.WorldConfig.SpawnBlendWidth, dist);
                var amp = world.WorldConfig.Amplitude * Mathf.Lerp(world.WorldConfig.SpawnFlatness, 1f, t);

                var h = world.WorldConfig.BaseHeight + Mathf.RoundToInt((noise * 2f - 1f) * amp);
                world.WorldData.Surface[x] = Mathf.Clamp(h, 1, world.WorldConfig.Height - 1);
            }
        }
    }
}