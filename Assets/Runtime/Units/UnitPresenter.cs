using Runtime.Common;
using Runtime.Common.ObjectPool;
using Runtime.Core;
using Runtime.Units.Combat;
using Runtime.Units.Movement;
using Runtime.Units.Stats;
using Runtime.ViewDescriptions;

namespace Runtime.Units
{
    public class UnitPresenter : IPresenter
    {
        protected UnitView View;

        private readonly UnitModel _model;
        private readonly World _world;
        private readonly WorldViewDescriptions _viewDescriptions;
        private readonly IObjectPool<UnitView> _pool;

        private UnitMovementPresenter _movementPresenter;
        private UnitCombatPresenter _combatPresenter;
        private UnitVisibilityPresenter _visibilityPresenter;
        private UnitStatusEffectsPresenter _statusEffectsPresenter;
        private UnitStatsPresenter _statsPresenter;

        public UnitPresenter(UnitModel model, IObjectPool<UnitView> pool, World world,
            WorldViewDescriptions viewDescriptions)
        {
            _model = model;
            _pool = pool;
            _world = world;
            _viewDescriptions = viewDescriptions;
        }

        public virtual void Enable()
        {
            View = _pool.Get();

            _statsPresenter = new UnitStatsPresenter(_model);
            _statsPresenter.Enable();

            _visibilityPresenter = new UnitVisibilityPresenter(_model, View);
            _visibilityPresenter.Enable();

            _movementPresenter = new UnitMovementPresenter(_model, View, _world);
            _movementPresenter.Enable();

            _combatPresenter = new UnitCombatPresenter(_model, View, _world);
            _combatPresenter.Enable();

            _statusEffectsPresenter = new UnitStatusEffectsPresenter(_model, View, _world, _viewDescriptions);
            _statusEffectsPresenter.Enable();

            _world.TurnBaseModel.OnWorldStepFinished += CheckInventory;
        }
        
        public virtual void Disable()
        {
            _world.TurnBaseModel.OnWorldStepFinished -= CheckInventory;
            
            _movementPresenter.Disable();
            _combatPresenter.Disable();
            _visibilityPresenter.Disable();
            _statusEffectsPresenter.Disable();
            _statsPresenter.Disable();

            _pool.Release(View);
        }

        private void CheckInventory()
        {
            if (_model.Inventory.IsEmpty() && _model != _world.PlayerModel)
            {
                _world.GridModel.ReleaseCell(_model.State.Position.Value);
                
                Disable();
            }
        }
    }
}