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
        
        [field: Header("游標顯示")]
        [field: SerializeField] private SpriteRenderer miningCursor;
        [field: SerializeField] private Color inReachColor = new Color(1, 1, 1, 1);
        [field: SerializeField] private Color outOfReachColor = new Color(1, 0, 0, 1);

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
            var worldPos = cam.ScreenToWorldPoint(playerCo.Input.MousePos);
            worldPos.z = 0;
            
            var cell = planet.BlockTilemap.WorldToCell(worldPos);
            var cellCenter = planet.BlockTilemap.GetCellCenterWorld(cell);
            var body = transform.position + Vector3.up;
            var inReach = Vector2.Distance(body, cellCenter) <= reach;
            var blockConfig = planet.GetBlockConfig(cell);

            // 更新游標位置與狀態
            miningCursor.transform.position = cellCenter;
            miningCursor.color = inReach ? inReachColor : outOfReachColor;
            
            // 角色沒有按按鍵就取消挖掘
            if (!playerCo.Input.IsMining)
            {
                ResetProgress();
                return;
            }
            
            // 角色離太遠就取消挖掘
            if (!inReach)
            {
                ResetProgress();
                return;
            }
            
            // 游標指到的格子沒有方塊
            if (!blockConfig)
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
            if (progress >= blockConfig.MiningTime)
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