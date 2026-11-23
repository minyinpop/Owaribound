using UnityEngine;

namespace World.Generator
{
    public sealed partial class PlanetGenerator
    {
        private void GenerateSurface(System.Random rng)
        {
            surface = new int[config.Width];
            var offset = (float)(rng.NextDouble() * 10000);

            for (var x = 0; x < config.Width; x++)
            {
                var big   = Mathf.PerlinNoise(offset + x * config.Frequency, 0f);
                var small = Mathf.PerlinNoise(offset + x * config.Frequency * 4f, 100f);
                var noise = big * .8f + small * .2f;

                // 離中間越近，起伏越小
                var dist = Mathf.Abs(x - config.Width / 2);
                var t = Mathf.InverseLerp(config.SpawnFlatRadius,
                    config.SpawnFlatRadius + config.SpawnBlendWidth, dist);
                var amp = config.Amplitude * Mathf.Lerp(config.SpawnFlatness, 1f, t);

                var h = config.BaseHeight + Mathf.RoundToInt((noise * 2f - 1f) * amp);
                surface[x] = Mathf.Clamp(h, 1, config.Height - 1);
            }
        }
    }
}