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
        
        public bool IsMining => input.Mouse.Mine.IsPressed();
        public Vector2 MousePos => input.Mouse.Position.ReadValue<Vector2>();
        
        public event Action<int> OnHotbarSelected;
        public event Action<int> OnMouseScrolled;
        
        private void Awake()
        {
            input = new InputActions();
        }

        private void OnEnable()
        {
            input.Player.Jump.performed += JumpPerformed;
            input.Player.Hotbar.performed += HotbarSelected;
            input.Mouse.Scroll.performed += MouseScrolled;
            
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
            
            input.Player.Jump.performed -= JumpPerformed;
            input.Player.Hotbar.performed -= HotbarSelected;
            input.Mouse.Scroll.performed -= MouseScrolled;
        }

        private void OnDestroy()
        {
            input.Dispose();
        }

        private void JumpPerformed(InputAction.CallbackContext ctx)
        {
            OnJumpPerformed?.Invoke();
        }

        private void HotbarSelected(InputAction.CallbackContext ctx)
        {
            var key = int.Parse(ctx.control.name);
            OnHotbarSelected?.Invoke(key == 0 ? 9 : key - 1);
        }

        private void MouseScrolled(InputAction.CallbackContext ctx)
        {
            var y = ctx.ReadValue<float>();
            if (y != 0)
                OnMouseScrolled?.Invoke(y > 0 ? -1 : 1);
        }
    }
}