using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Core;
using Runtime.Descriptions.StatusEffects.Enums;
using Runtime.StatusEffects;
using Runtime.ViewDescriptions.StatusEffects;
using UniRx;
using UnityEngine;

namespace Runtime.UI.StatusEffect
{
    public class StatusEffectUIViewPresenter : IPresenter
    {
        private readonly StatusEffectModel _model;
        private readonly StatusEffectUIView _uiView;
        private readonly World _world;
        private readonly StatusEffectViewDescription _viewDescription;

        private readonly CompositeDisposable _disposables = new();
        private LoadModel<Sprite> _loadModel;

        public StatusEffectUIViewPresenter(StatusEffectModel model, StatusEffectUIView uiView, World world,
            StatusEffectViewDescription viewDescription)
        {
            _model = model;
            _uiView = uiView;
            _world = world;
            _viewDescription = viewDescription;
        }

        public async void Enable()
        {
            var key = $"{_viewDescription.Icons.AssetGUID}[{_viewDescription.Icons.SubObjectName}]";
            _loadModel = _world.AddressableModel.Load<Sprite>(key);
            await _loadModel.LoadAwaiter;
            _uiView.Icon.sprite = _loadModel.Result;
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
                _uiView.TurnsCounter.text = remainingTurns.ToString();
            }
            else
            {
                _uiView.TurnsCounter.text = "";
                _uiView.TurnsCounter.visible = false;
            }
        }

        private void HandleStacksChanged(int stacks)
        {
            if (stacks <= 1)
            {
                _uiView.StackCounter.text = "";
                _uiView.StackCounter.visible = false;
                return;
            }

            _uiView.StackCounter.visible = true;
            _uiView.StackCounter.text = stacks + "X";
        }
    }
}