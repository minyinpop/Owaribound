using UnityEngine;
using World;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public sealed class MiningController : MonoBehaviour
    {
        [field: Header("挖掘設定")]
        [field: SerializeField] private WorldSystem world;
        [field: SerializeField] private float reach = 4; // 能挖多遠
        [field: SerializeField] private float miningPower = 1;
        
        [field: Header("游標顯示")]
        [field: SerializeField] private SpriteRenderer miningCursor;
        [field: SerializeField] private Color inReachColor = new(1, 1, 1, 1);
        [field: SerializeField] private Color outOfReachColor = new(1, 0, 0, 1);

        private PlayerController playerCo;
        private Camera cam;

        private void Awake()
        {
            playerCo = GetComponent<PlayerController>();
            cam = Camera.main;
        }

        private void Update()
        {
            var worldPos = cam.ScreenToWorldPoint(playerCo.Input.MousePos);
            worldPos.z = 0;
            
            var cell = world.BlockTilemap.WorldToCell(worldPos);
            var cellCenter = world.BlockTilemap.GetCellCenterWorld(cell);
            var body = transform.position + Vector3.up;
            var inReach = Vector2.Distance(body, cellCenter) <= reach;

            // 更新游標位置與狀態
            miningCursor.transform.position = cellCenter;
            miningCursor.color = inReach ? inReachColor : outOfReachColor;
            
            // 角色沒有按按鍵就取消挖掘
            if (!playerCo.Input.IsMining)
                return;
            
            // 角色離太遠就取消挖掘
            if (!inReach)
                return;

            world.WorldDamageSystem.ApplyDamage(cell, miningPower * Time.deltaTime);
        }
    }
}