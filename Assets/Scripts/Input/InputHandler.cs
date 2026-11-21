using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public sealed class InputHandler : MonoBehaviour
    {
        private InputActions input;

        public float WalkDir => input.Player.Move.ReadValue<Vector2>().normalized.x;

        public event Action OnJumpPerformed;

        private void Awake()
        {
            input = new InputActions();
        }

        private void OnEnable()
        {
            input.Player.Jump.performed += JumpPerformed;
            
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
            
            input.Player.Jump.performed -= JumpPerformed;
        }

        private void OnDestroy()
        {
            input.Dispose();
        }

        private void JumpPerformed(InputAction.CallbackContext ctx)
        {
            OnJumpPerformed?.Invoke();
        }
    }
}