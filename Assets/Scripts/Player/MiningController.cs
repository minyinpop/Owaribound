using UnityEngine;
using World.Generator;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public sealed class MiningController : MonoBehaviour
    {
        [field: Header("挖掘設定")]
        [field: SerializeField] private PlanetGenerator planet;
        [field: SerializeField] private float reach = 4;     // 能挖多遠
        [field: SerializeField] private float mineTime = .4f; // 挖掘時間 

        private PlayerController playerCo;
        private Camera cam;

        private Vector3Int? target;
        private float progress;

        private void Awake()
        {
            playerCo = GetComponent<PlayerController>();
            cam = Camera.main;
        }

        private void Update()
        {
            if (!playerCo.Input.IsMining)
            {
                ResetProgress();
                return;
            }

            var worldPos = cam.ScreenToWorldPoint(playerCo.Input.MousePos);
            worldPos.z = 0;
            
            var cell = planet.BlockTilemap.WorldToCell(worldPos);
            var cellCenter = planet.BlockTilemap.GetCellCenterWorld(cell);
            var body = transform.position + Vector3.up;

            // 角色離太遠就取消挖掘
            if (Vector2.Distance(body, cellCenter) > reach)
            {
                ResetProgress();
                return;
            }

            // 換了目標就重新計算
            if (target != cell)
            {
                target = cell;
                progress = 0;
            }

            progress += Time.deltaTime;
            
            // 挖掘成功
            if (progress >= mineTime)
            {
                planet.TryBreakBlock(cell);
                ResetProgress();
            }
        }

        private void ResetProgress()
        {
            target = null;
            progress = 0;
        }
    }
}