using Runtime.Common;
using Runtime.Core;
using Runtime.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Runtime.UI.Blocker
{
    public class UIBlockerPresenter : IPresenter
    {
        private readonly UIBlockerModel _model;
        private readonly VisualElement _root;
        private readonly PlayerControls _playerControls;

        public UIBlockerPresenter(UIBlockerModel model, World world, UIContent uiContent)
        {
            _model = model;
            _root = uiContent.GameplayContent;
            _playerControls = world.PlayerControls;
        }

        public void Enable()
        {
            _playerControls.Gameplay.PointerPosition.performed += HandlePointerPositionChanged;
        }

        public void Disable()
        {
            _playerControls.Gameplay.PointerPosition.performed -= HandlePointerPositionChanged;
        }

        private void HandlePointerPositionChanged(InputAction.CallbackContext context)
        {
            var screenPosition  = context.ReadValue<Vector2>();
            var invertPosition = new Vector2(screenPosition.x, Screen.height - screenPosition.y);
            
            var panelPosition = RuntimePanelUtils.ScreenToPanel(_root.panel, invertPosition);
            
            _model.IsPointerOverUI.Value = _root.panel.Pick(panelPosition) != null;
        }
    }
}