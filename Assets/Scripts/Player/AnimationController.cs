using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public sealed class AnimationController : MonoBehaviour
    {
        private static readonly int axisXHash = Animator.StringToHash("AxisX");
        private static readonly int idleHash = Animator.StringToHash("Idle");
        private static readonly int walkHash = Animator.StringToHash("Walk");

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