using System;
using Runtime.Common;
using UniRx;
using UnityEngine.UIElements;

namespace Runtime.Units
{
    public class UnitVisibilityPresenter : IPresenter
    {
        private const string VisibilityRadiusKey = "visibility_radius";
        private const string HealthKey = "health";

        private readonly UnitView _view;
        private readonly UnitModel _model;
        private IDisposable _visibleSubscription;

        public UnitVisibilityPresenter(UnitModel model, UnitView view)
        {
            _model = model;
            _view = view;
        }

        public void Enable()
        {
            _visibleSubscription = _model.State.Visible.Subscribe(HandleVisibleChange);

            _model.Stats[VisibilityRadiusKey].ValueChanged += HandleChangeVisibilityRadius;
            _model.Stats[HealthKey].ValueChanged += HandleHealthChanged;

            HandleVisibleChange(_model.State.Visible.Value);
            
            HandleChangeVisibilityRadius(_model.Stats[VisibilityRadiusKey].Value);
        }

        public void Disable()
        {
            _model.Stats[VisibilityRadiusKey].ValueChanged -= HandleChangeVisibilityRadius;
            _model.Stats[HealthKey].ValueChanged -= HandleHealthChanged;

            _visibleSubscription?.Dispose();
        }

        private void HandleVisibleChange(bool value)
        {
            _view.SpriteRenderer.enabled = value;

            _view.UIDocument.rootVisualElement.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void HandleHealthChanged(float value)
        {
            if (value <= 0f)
            {
                if (_view.Light != null)
                {
                    _view.Light.intensity = 0;
                    _view.Light.pointLightOuterRadius = 0;
                }
            }
        }

        private void HandleChangeVisibilityRadius(float radius)
        {
            if (_view.Light != null)
            {
                _view.Light.intensity = Math.Clamp(radius, 0f, 0.5f);
                _view.Light.pointLightOuterRadius = radius + 3f;
            }
        }
    }
}