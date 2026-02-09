using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Core;
using Runtime.Descriptions.StatusEffects.Enums;
using Runtime.ViewDescriptions.StatusEffects;
using UniRx;
using UnityEngine;

namespace Runtime.StatusEffects
{
    public class StatusEffectViewPresenter : IPresenter
    {
        private readonly StatusEffectModel _model;
        private readonly StatusEffectView _view;
        private readonly World _world;
        private readonly StatusEffectViewDescription _viewDescription;

        private readonly CompositeDisposable _disposables = new();
        private LoadModel<Sprite> _loadModel;
        
        public StatusEffectViewPresenter(StatusEffectModel model, StatusEffectView view, World world, StatusEffectViewDescription viewDescription)
        {
            _model = model;
            _view = view;
            _world = world;
            _viewDescription = viewDescription;
        }

        public async void Enable()
        {
            var key = $"{_viewDescription.Icons.AssetGUID}[{_viewDescription.Icons.SubObjectName}]";
            _loadModel = _world.AddressableModel.Load<Sprite>(key);
            await _loadModel.LoadAwaiter;
            _view.Icon.sprite = _loadModel.Result;
            _model.RemainingTurns.Subscribe(HandleRemainingTurnsChanged).AddTo(_disposables);
            _model.CurrentStacks.Subscribe(HandleStacksChanged).AddTo(_disposables);
        }

        public void Disable()
        {
            _world.AddressableModel.Unload(_loadModel);
            _disposables.Dispose();
        }

        private void HandleRemainingTurnsChanged(int remainingTurns)
        {
            if (_model.Description.Duration.Type == DurationType.TurnBased)
            {
                _view.TurnsCounter.text = remainingTurns.ToString();
            }
        }
        
        private void HandleStacksChanged(int stacks)
        {
            _view.StackCounter.text = stacks + "X";
        }
    }
}