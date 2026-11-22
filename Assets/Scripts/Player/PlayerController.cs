using Input;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(AnimationController))]
    public sealed class PlayerController : MonoBehaviour
    {
        [field: Header("Self Components")]
        [field: SerializeField] private Rigidbody2D rig;
                                public Rigidbody2D Rig => rig;
        [field: SerializeField] private Animator anima;
                                public Animator Anima => anima;
        
        [field: Header("Other Components")]
        [field: SerializeField] private InputHandler input;
                                public InputHandler Input => input;
        
        [field: Header("Move Settings")]
        [field: SerializeField] private float moveSpeed = 5;
        
        [field: Header("Jump Settings")]
        [field: SerializeField] private float jumpForce = 5;
        
        [field: Header("Floor Detect Settings")]
        [field: SerializeField] private Vector2 offset = new(0, .1f);
        [field: SerializeField] private Vector2 size = new(.7f, .05f);
        [field: SerializeField] private float distance = .15f;
        [field: SerializeField] private LayerMask layerMask;
        [field: SerializeField] private float leaveGroundDelay = .08f;
        
        public bool IsGrounded { get; private set; }
        public bool IsAirborne { get; private set; }

        private bool jumpRequested;
        private float leaveGroundTime;

        private void OnEnable()
        {
            Input.OnJumpPerformed += RequestJump;
        }

        private void FixedUpdate()
        {
            #region 地板檢測
                var origin = (Vector2)transform.position + offset;
                IsGrounded = Physics2D.BoxCast(origin, size, 0, Vector2.down, distance, layerMask);
            #endregion
            
            #region 空中檢測
                leaveGroundTime = IsGrounded ? 0 : leaveGroundTime + Time.fixedDeltaTime;

                if (!IsAirborne && leaveGroundTime >= leaveGroundDelay)
                {
                    IsAirborne = true;
                }
                else if (IsAirborne && IsGrounded && rig.linearVelocityY <= .01f)
                {
                    IsAirborne = false;
                }
            #endregion
            
            #region 移動
                rig.linearVelocityX = Input.WalkDir * moveSpeed;
            #endregion

            #region 跳躍
                if (jumpRequested && IsGrounded && !IsAirborne)
                {
                    IsAirborne = true;
                    rig.linearVelocityY = jumpForce;
                }

                jumpRequested = false;
            #endregion
        }

        private void OnDisable()
        {
            Input.OnJumpPerformed -= RequestJump;
        }

        private void RequestJump()
        {
            jumpRequested = true;
        }
    }
}