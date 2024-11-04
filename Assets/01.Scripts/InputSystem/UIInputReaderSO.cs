using System;
using BlockManage;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManage
{
    [CreateAssetMenu(menuName = "SO/Input/UIInputReader")]
    public class UIInputReaderSO : ScriptableObject, Controls.IUIActions
    {
        public event Action BuildEvent;
        public event Action<ISelectable> SelectEvent;
        public event Action<bool> LeftClickEvent;
        public event Action<bool> RightClickEvent;
        public event Action OnInventoryOpenEvent;
        public event Action OnInventorySortEvent;

        private Vector2 _mouseScreenPosition;
        public Vector2 AimPosition { get; private set; }

        [SerializeField] private LayerMask _objectLayer;
    
        private Controls _controls;
        private void OnEnable()
        {
            if(_controls == null)
            {
                _controls = new Controls();
                _controls.UI.SetCallbacks(this);
                _controls.UI.Enable();
            }
        }
        
        private void OnDisable()
        {
            _controls.UI.Disable();
        }
        
        public void OnBuild(InputAction.CallbackContext context)
        { // 빌드 키 : B
            BuildEvent?.Invoke();
            
        }

        public void OnSelect(InputAction.CallbackContext context)
        { // 우클릭, Select: 건물 선택
            if (context.performed)
            {
                RightClickEvent?.Invoke(true);
                CheckStructure();
            }
            else if(context.canceled)
                RightClickEvent?.Invoke(false);
        }

        public void OnLeftClick(InputAction.CallbackContext context)
        {
            if(context.performed)
                LeftClickEvent?.Invoke(true);
            else if(context.canceled)
                LeftClickEvent?.Invoke(false);
        }

        public void OnMouse(InputAction.CallbackContext context)
        {
            _mouseScreenPosition = context.ReadValue<Vector2>();
            AimPosition = Camera.main.ScreenToWorldPoint(_mouseScreenPosition);
        }

        public void CheckStructure()
        {
            RaycastHit2D hit = Physics2D.Raycast(AimPosition, Vector2.zero, 10f, _objectLayer);
            if (hit.collider != null)
            {
                if (hit.collider.transform.TryGetComponent(out ISelectable selectedObject))
                {
                    SelectEvent?.Invoke(selectedObject);
                }
            }
        }

        public void OnInventory(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnInventoryOpenEvent?.Invoke();
        }

        public void OnSortInventory(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnInventorySortEvent?.Invoke();
        }
    }
}