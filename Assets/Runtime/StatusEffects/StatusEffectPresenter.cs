using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Core;
using Runtime.Descriptions.StatusEffects.Enums;
using Runtime.Units;
using Runtime.ViewDescriptions.StatusEffects;
using UniRx;
using UnityEngine;

namespace Runtime.StatusEffects
{
    public class StatusEffectPresenter : IPresenter
    {
        public bool IsExpired => _model.IsExpired;
        
        private readonly StatusEffectModel _model;
        private readonly StatusEffectView _view;
        private readonly UnitModel _unit;
        private readonly World _world;
        private readonly StatusEffectViewDescription _viewDescription;

        private readonly CompositeDisposable _disposables = new();
        private LoadModel<Sprite> _loadModel;
        
        public StatusEffectPresenter(StatusEffectModel model, StatusEffectView view, UnitModel unit, World world, StatusEffectViewDescription viewDescription)
        {
            _model = model;
            _view = view;
            _unit = unit;
            _world = world;
            _viewDescription = viewDescription;
        }

        public async void Enable()
        {
            _loadModel = _world.AddressableModel.Load<Sprite>(_viewDescription.Icon.AssetGUID);
            await _loadModel.LoadAwaiter;
            _view.Icon.sprite = _loadModel.Result;
            _model.RemainingTurns.Subscribe(HandleRemainingTurnsChanged).AddTo(_disposables);
        }

        public void Disable()
        {
            _world.AddressableModel.Unload(_loadModel);
            _disposables.Dispose();
        }

        public void Tick()
        {
            if (CanApply())
            {
                foreach (var modifier in _model.Description.Modifiers)
                {
                    modifier.Tick(_unit, _world);
                }

                if (_model.Description.Duration.Type == DurationType.TurnBased)
                    _model.DecrementRemainingTurns();
            }
        }

        private bool CanApply()
        {
            foreach (var constraint in _model.Description.Constraint)
            {
                if (constraint.Check(_unit, _world)) continue;
                return false;
            }

            return true;
        }

        private void HandleRemainingTurnsChanged(int remainingTurns)
        {
            if (_model.Description.Duration.Type == DurationType.TurnBased)
            {
                _view.Counter.text = remainingTurns.ToString();
            }
        }
    }
}
