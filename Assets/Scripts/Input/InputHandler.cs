using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public sealed class InputHandler : MonoBehaviour
    {
        private InputActions input;

        public float WalkDir => input.Player.Move.ReadValue<Vector2>().normalized.x;

        public event Action WalkStarted;
        public event Action WalkCanceled;

        private void Awake()
        {
            input = new InputActions();
        }

        private void OnEnable()
        {
            input.Player.Move.started += OnWalkStarted;
            input.Player.Move.canceled += OnWalkCanceled;
            
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
            
            input.Player.Move.started -= OnWalkStarted;
            input.Player.Move.canceled -= OnWalkCanceled;
        }

        private void OnDestroy()
        {
            input.Dispose();
        }

        private void OnWalkStarted(InputAction.CallbackContext ctx)
        {
            WalkStarted?.Invoke();
        }

        private void OnWalkCanceled(InputAction.CallbackContext ctx)
        {
            WalkCanceled?.Invoke();
        }
    }
}