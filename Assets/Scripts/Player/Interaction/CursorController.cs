using UnityEngine;
using UnityEngine.EventSystems;
using World;

namespace Player.Interaction
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
        
        public Vector3Int Cell { get; private set; }
        public bool InReach { get; private set; }
        
        private PlayerController playerCo;
        private Camera cam;
        
        public bool IsOverUI { get; private set; }
        public Vector3 MouseWorldPos { get; private set; }

        private void Awake()
        {
            playerCo = GetComponent<PlayerController>();
            cam = Camera.main;
        }

        private void Update()
        {
            #region 方塊互動游標在格子上就不顯示
                IsOverUI = EventSystem.current && EventSystem.current.IsPointerOverGameObject();
                cursorRenderer.enabled = !IsOverUI;
            #endregion
            
            var worldPos = cam.ScreenToWorldPoint(playerCo.Input.MousePos);
            worldPos.z = 0;
            
            MouseWorldPos = worldPos;
            
            Cell = worldSystem.BlockTilemap.WorldToCell(worldPos);
            var cellCenter = worldSystem.BlockTilemap.GetCellCenterWorld(Cell);
            var body = transform.position + Vector3.up;
            InReach = Vector2.Distance(body, cellCenter) <= reach;

            cursorRenderer.transform.position = cellCenter;
            cursorRenderer.color = InReach ? inReachColor : outOfReachColor;
        }
    }
}