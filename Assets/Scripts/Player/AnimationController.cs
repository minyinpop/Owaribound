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

        private void Awake()
        {
            playerCo = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            playerCo.Input.WalkStarted += OnWalkStarted;
            playerCo.Input.WalkCanceled += OnWalkCanceled;
        }

        private void Update()
        {
            if (playerCo.Input.WalkDir is > 0 or < 0)
            {
                playerCo.Anima.SetFloat(axisXHash, playerCo.Input.WalkDir);
            }
        }

        private void OnDisable()
        {
            playerCo.Input.WalkStarted -= OnWalkStarted;
            playerCo.Input.WalkCanceled -= OnWalkCanceled;
        }

        private void OnWalkStarted()
        {
            playerCo.Anima.Play(walkHash);
        }
        
        private void OnWalkCanceled()
        {
            playerCo.Anima.Play(idleHash);
        }
    }
}