using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManage
{
    [CreateAssetMenu(menuName = "SO/Input/PlayerInputSO")]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        public event Action<Vector2> MovementEvent;
        public bool CanControl { get; private set; } = true;

        public void SetControl(bool value)
        {
            CanControl = value;
        }


        private Controls _controls;
        public Vector2 Movement { get; private set; }
        public bool IsMoving { get; private set; }

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
                _controls.Player.Enable();
            }
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            if(!CanControl) return;
            if (context.performed)
                IsMoving = true;
            else if (context.canceled)
                IsMoving = false;
            Movement = context.ReadValue<Vector2>();
            MovementEvent?.Invoke(Movement);
        }

    }

}