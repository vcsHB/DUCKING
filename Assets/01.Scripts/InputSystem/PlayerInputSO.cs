using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManage
{
    
    [CreateAssetMenu(menuName = "SO/Input/PlayerInputSO")]    
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        public event Action<Vector2> MovementEvent;
        public Vector3 AimPosition { get; private set; }

        [SerializeField] private LayerMask _whatIsGround;
    
        private Vector3 _lastMousePosition;
        private Controls _controls;

        private void OnEnable()
        {
            if(_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
                _controls.Player.Enable();
            }
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            Vector2 screenPosition = context.ReadValue<Vector2>();
            AimPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        }

    }

}