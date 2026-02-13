using Runtime.Common;
using Runtime.Common.ObjectPool;
using Runtime.Core;
using Runtime.Units.Combat;
using Runtime.Units.Movement;
using Runtime.Units.Rotation;
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
        private UnitRotationPresenter _rotationPresenter;
        private UnitVisibilityPresenter _visibilityPresenter;
        private UnitStatusEffectsPresenter _statusEffectsPresenter;
        private UnitStatsPresenter _statsPresenter;
        private UnitHudPresenter _hudPresenter;

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

            _rotationPresenter = new UnitRotationPresenter(_model, View);
            _rotationPresenter.Enable();

            _movementPresenter = new UnitMovementPresenter(_model, View, _world);
            _movementPresenter.Enable();

            _combatPresenter = new UnitCombatPresenter(_model, View, _world);
            _combatPresenter.Enable();

            _statusEffectsPresenter = new UnitStatusEffectsPresenter(_model, View, _world, _viewDescriptions);
            _statusEffectsPresenter.Enable();

            _visibilityPresenter = new UnitVisibilityPresenter(_model, View);
            _visibilityPresenter.Enable();
            
            _hudPresenter = new UnitHudPresenter(_model, View);
            _hudPresenter.Enable();
        }

        public virtual void Disable()
        {
            _movementPresenter.Disable();
            _combatPresenter.Disable();
            _rotationPresenter.Disable();
            _statusEffectsPresenter.Disable();
            _statsPresenter.Disable();
            _hudPresenter.Disable();
            _visibilityPresenter.Disable();

            _pool.Release(View);
        }
    }
}