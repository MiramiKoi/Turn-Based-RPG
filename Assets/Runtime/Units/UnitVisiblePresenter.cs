using System;
using Runtime.Common;
using Runtime.Units;
using UniRx;
using UnityEngine;

namespace Runtime.Player
{
    public class UnitVisiblePresenter : IPresenter
    {
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
            _visibleSubscription = _model.Visible.Subscribe(OnVisibleChange);
            
            OnVisibleChange(_model.Visible.Value);
        }

        public void Disable()
        {
            _visibleSubscription?.Dispose();
        }

        private void OnVisibleChange(bool value)
        {
            _view.SpriteRenderer.enabled = value;
        }
    }
}