using Runtime.Common;
using Runtime.Core;
using Runtime.SpawnDirector.Corpse;
using UnityEngine;

namespace Runtime.SpawnDirector.Rules
{
    public abstract class SpawnRulePresenter : IPresenter
    {
        protected readonly SpawnRuleModel Model;
        protected readonly World World;

        private CorpseRulePresenter _corpseRulePresenter;

        protected SpawnRulePresenter(SpawnRuleModel model, World world)
        {
            Model = model;
            World = world;
        }

        public void Enable()
        {
            World.TurnBaseModel.OnWorldStepFinished += HandleStep;
            Initialize();
        }

        public void Disable()
        {
            World.TurnBaseModel.OnWorldStepFinished -= HandleStep;
            _corpseRulePresenter?.Disable();
            _corpseRulePresenter = null;
        }

        protected virtual void Initialize()
        {
            if (Model.Description.Corpse is { IsInfinite: false })
            {
                var corpseModel = new CorpseRuleModel(Model, Model.Description.Corpse);
                _corpseRulePresenter = new CorpseRulePresenter(corpseModel, World);
                _corpseRulePresenter.Enable();
            }
        }

        protected void SpawnOne()
        {
            var position = GetSpawnPosition();
            var unit = World.UnitCollection.Create(Model.Description.UnitDescriptionId);
            unit.Movement.SetPosition(position);
            Model.Units.Add(unit);
        }

        private Vector2Int GetSpawnPosition()
        {
            if (Model.Description.Spawn.Mode == "fixed" && Model.Description.Spawn.FixedPosition.HasValue)
            {
                var fixedPosition = Model.Description.Spawn.FixedPosition.Value;
                if (!World.GridModel.GetCell(fixedPosition).IsOccupied)
                    return fixedPosition;

                var neighbors = World.GridModel.GetNeighborAvailablePositions(fixedPosition);
                
                return neighbors[Random.Range(0, neighbors.Count)];
            }

            var position = World.GridModel.GetRandomAvailablePosition();
            return position ?? Vector2Int.zero;
        }

        protected abstract void HandleStep();
    }
}