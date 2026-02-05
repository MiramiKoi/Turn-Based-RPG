using Runtime.Common;
using Runtime.Core;
using Runtime.Descriptions.StatusEffects.Enums;
using Runtime.Units;

namespace Runtime.StatusEffects
{
    public class StatusEffectPresenter : IPresenter
    {
        private readonly StatusEffectModel _model;
        private readonly UnitModel _unit;
        private readonly World _world;

        public StatusEffectPresenter(StatusEffectModel model, UnitModel unit, World world)
        {
            _model = model;
            _unit = unit;
            _world = world;
        }

        public bool IsExpired => _model.IsExpired;

        public void Enable()
        {
        }

        public void Disable()
        {
        }

        public void Tick(TickMoment moment)
        {
            if (_model.Description.Tick.On == moment)
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
    }
}
