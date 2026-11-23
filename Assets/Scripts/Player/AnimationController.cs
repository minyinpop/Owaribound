using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public sealed class AnimationController : MonoBehaviour
    {
        private static readonly int AxisXHash = Animator.StringToHash("AxisX");
        private static readonly int IdleHash = Animator.StringToHash("Idle");
        private static readonly int WalkHash = Animator.StringToHash("Walk");
        private static readonly int JumpHash = Animator.StringToHash("Jump");
        private static readonly int FallHash = Animator.StringToHash("Fall");

        private PlayerController playerCo;

        private int currentHash;

        private void Awake()
        {
            playerCo = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (playerCo.Input.WalkDir is > 0 or < 0)
            {
                playerCo.Anima.SetFloat(AxisXHash, playerCo.Input.WalkDir);
            }

            if (playerCo.IsAirborne)
            {
                Play(playerCo.Rig.linearVelocityY > 0 ? JumpHash : FallHash);
                return;
            }

            Play(playerCo.Rig.linearVelocityX is > .05f or < -.05f ? WalkHash : IdleHash);
        }

        private void Play(int hash)
        {
            if (hash == currentHash)
                return;

            currentHash = hash;
            playerCo.Anima.Play(currentHash);
        }
    }
}