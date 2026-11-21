using UnityEngine;

namespace Input
{
    public sealed class InputHandler : MonoBehaviour
    {
        private InputActions input;

        public float WalkDir => input.Player.Move.ReadValue<Vector2>().normalized.x;

        private void Awake()
        {
            input = new InputActions();
        }

        private void OnEnable()
        {
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
        }

        private void OnDestroy()
        {
            input.Dispose();
        }
    }
}