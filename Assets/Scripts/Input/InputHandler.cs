using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    [DefaultExecutionOrder(-90)]
    public sealed class InputHandler : MonoBehaviour
    {
        private InputActions input;

        public float WalkDir => input.Player.Move.ReadValue<Vector2>().normalized.x;
        
        public event Action OnJumpPerformed;
        
        public bool IsPrimaryHeld => input.Mouse.PrimaryMouse.IsPressed();
        public bool IsPrimaryPressed => input.Mouse.PrimaryMouse.WasPressedThisFrame();
        public bool IsSecondaryHeld => input.Mouse.SecondaryMouse.IsPressed();
        public bool IsSecondaryPressed => input.Mouse.SecondaryMouse.WasPressedThisFrame();
        
        public Vector2 MousePos => input.Mouse.Position.ReadValue<Vector2>();
        
        public event Action<int> OnHotbarSelected;
        public event Action<int> OnMouseScrolled;

        public event Action OnInventoryToggled;
        
        private void Awake()
        {
            input = new InputActions();
        }

        private void OnEnable()
        {
            input.Player.Jump.performed += JumpPerformed;
            input.Player.Hotbar.performed += HotbarSelected;
            input.Mouse.Scroll.performed += MouseScrolled;
            input.Player.Inventory.performed += InventoryToggled;
            
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
            
            input.Player.Jump.performed -= JumpPerformed;
            input.Player.Hotbar.performed -= HotbarSelected;
            input.Mouse.Scroll.performed -= MouseScrolled;
            input.Player.Inventory.performed -= InventoryToggled;
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

        private void InventoryToggled(InputAction.CallbackContext ctx)
        {
            OnInventoryToggled?.Invoke();
        }
    }
}