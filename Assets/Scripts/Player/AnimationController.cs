using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public sealed class AnimationController : MonoBehaviour
    {
        private static readonly int axisXHash = Animator.StringToHash("AxisX");
        private static readonly int idleHash = Animator.StringToHash("Idle");
        private static readonly int walkHash = Animator.StringToHash("Walk");
        private static readonly int jumpHash = Animator.StringToHash("Jump");
        private static readonly int fallHash = Animator.StringToHash("Fall");

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
                playerCo.Anima.SetFloat(axisXHash, playerCo.Input.WalkDir);
            }

            if (playerCo.IsAirborne)
            {
                Play(playerCo.Rig.linearVelocityY > 0 ? jumpHash : fallHash);
                return;
            }

            Play(playerCo.Rig.linearVelocityX is > .05f or < -.05f ? walkHash : idleHash);
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