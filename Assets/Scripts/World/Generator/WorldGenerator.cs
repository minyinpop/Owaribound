using UnityEngine;

namespace World.Generator
{
    [RequireComponent(typeof(WorldSystem))]
    public sealed partial class WorldGenerator : MonoBehaviour
    {
        private WorldSystem world;

        private void Awake()
        {
            world = GetComponent<WorldSystem>();
        }

        public void Generator()
        {
            RegisterBlockConfigs();
            
            GenerateSurface(new System.Random(world.WorldSeed));
            
            FillLayers();
            
            CarveCaves(new System.Random(world.WorldSeed + 1));
            
            CarveCaveEntrances(new System.Random(world.WorldSeed + 2));
            
            PlaceOres(new System.Random(world.WorldSeed + 3));

            Render();
            
            SpawnPlayer(world.WorldConfig.Width / 2);
        }
    }
}