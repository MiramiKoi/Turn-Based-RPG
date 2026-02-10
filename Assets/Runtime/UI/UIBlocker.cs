using Runtime.Input;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.UI
{
    public class UIBlocker
    {
        private readonly VisualElement _root;
        private readonly PlayerControls _playerControls;

        public UIBlocker(VisualElement root, PlayerControls playerControls)
        {
            _root = root;
            _playerControls = playerControls;
        }
        
        public bool IsPointerOverUI
        {
            get
            {
                var pos = _playerControls.Gameplay.PointerPosition.ReadValue<Vector2>();
                return _root.panel.Pick(pos) != null;
            }
        }
    }
}