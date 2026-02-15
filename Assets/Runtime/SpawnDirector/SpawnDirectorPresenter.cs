using System.Collections.Generic;
using Runtime.Common;
using Runtime.Core;
using Runtime.SpawnDirector.Rules;
using UniRx;

namespace Runtime.SpawnDirector
{
    public class SpawnDirectorPresenter : IPresenter
    {
        private readonly SpawnDirectorModel _model;
        private readonly World _world;

        private readonly Dictionary<string, SpawnRulePresenter> _presenters = new();
        private readonly CompositeDisposable _disposables = new();

        public SpawnDirectorPresenter(SpawnDirectorModel model, World world)
        {
            _model = model;
            _world = world;
        }

        public void Enable()
        {
            _model.Rules.ObserveAdd().Subscribe(HandleAddPresenter).AddTo(_disposables);
            _model.Rules.ObserveRemove().Subscribe(HandleRemovePresenter).AddTo(_disposables);
        }

        public void Disable()
        {
            _disposables.Dispose();

            foreach (var presenter in _presenters.Values)
                presenter.Disable();

            _presenters.Clear();
        }

        private void HandleAddPresenter(DictionaryAddEvent<string, SpawnRuleModel> addEvent)
        {
            var ruleModel = addEvent.Value;
            SpawnRulePresenter presenter = ruleModel.Description.RuleType switch
            {
                "solo" => new SoloSpawnRulePresenter(ruleModel, _world),
                "population" => new PopulationSpawnRule(ruleModel, _world),
                _ => null
            };

            if (presenter != null)
            {
                presenter.Enable();
                _presenters.Add(addEvent.Key, presenter);
            }
        }

        private void HandleRemovePresenter(DictionaryRemoveEvent<string, SpawnRuleModel> removeEvent)
        {
            if (_presenters.TryGetValue(removeEvent.Key, out var presenter))
            {
                presenter.Disable();
                _presenters.Remove(removeEvent.Key);
            }
        }
    }
}