using System;
using Runtime.Common;
using UniRx;

namespace Runtime.Units.Components
{
    public class UnitVisiblePresenter : IPresenter
    {
        private const string VisibilityRadiusKey = "visibility_radius";

        private readonly UnitView _view;
        private readonly UnitModel _model;
        private IDisposable _visibleSubscription;

        public UnitVisiblePresenter(UnitModel model, UnitView view)
        {
            _model = model;
            _view = view;
        }

        public void Enable()
        {
            _visibleSubscription = _model.State.Visible.Subscribe(OnVisibleChange);

            _model.Stats[VisibilityRadiusKey].ValueChanged += OnChangeVisibilityRadius;

            OnVisibleChange(_model.State.Visible.Value);
        }

        public void Disable()
        {
            _model.Stats[VisibilityRadiusKey].ValueChanged -= OnChangeVisibilityRadius;

            _visibleSubscription?.Dispose();
        }

        private void OnVisibleChange(bool value)
        {
            _view.SpriteRenderer.enabled = value;
        }

        private void OnChangeVisibilityRadius(float radius)
        {
            if (_view.Light != null)
            {
                _view.Light.pointLightOuterRadius = radius;
            }
        }
    }
}