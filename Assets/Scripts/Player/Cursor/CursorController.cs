using UnityEngine;
using World;

namespace Player.Cursor
{
    [DefaultExecutionOrder((-10))]
    [RequireComponent(typeof(PlayerController))]
    public sealed class CursorController : MonoBehaviour
    {
        [field: Header("世界組件")]
        [field: SerializeField] private WorldSystem worldSystem;
        
        // ================================
        
        [field: Header("游標設定")]
        [field: Tooltip("玩家可以碰到多遠的格子")]
        [field: SerializeField] private float reach = 3;
        
        // ================================
        
        [field: Header("游標顯示")]
        [field: SerializeField] private SpriteRenderer cursorRenderer;
        [field: SerializeField] private Color inReachColor = new(1, 1, 1, 1);
        [field: SerializeField] private Color outOfReachColor = new(1, 0, 0, 1);
        
        // ================================
        
        public Vector3Int Cell { get; set; }
        public bool InReach { get; private set; }

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
            
            Cell = worldSystem.BlockTilemap.WorldToCell(worldPos);
            var cellCenter = worldSystem.BlockTilemap.GetCellCenterWorld(Cell);
            var body = transform.position + Vector3.up;
            InReach = Vector2.Distance(body, cellCenter) <= reach;

            cursorRenderer.transform.position = cellCenter;
            cursorRenderer.color = InReach ? inReachColor : outOfReachColor;
        }
    }
}