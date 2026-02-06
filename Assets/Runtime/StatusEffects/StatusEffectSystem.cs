using System.Linq;
using Runtime.Core;
using Runtime.Descriptions.StatusEffects.Enums;
using Runtime.Units;

namespace Runtime.StatusEffects
{
    public class StatusEffectSystem
    {
        public bool IsExpired => _model.IsExpired;

        private readonly StatusEffectModel _model;
        private readonly UnitModel _unit;
        private readonly World _world;

        public StatusEffectSystem(StatusEffectModel model, UnitModel unit, World world)
        {
            _model = model;
            _unit = unit;
            _world = world;
        }

        public void Tick()
        {
            if (CanApply())
            {
                var stacks = _model.CurrentStacks.Value;

                foreach (var modifier in _model.Description.Modifiers.Where(modifier =>
                             modifier.Type is ModifierExecutionTime.WhileActive))
                {
                    modifier.Tick(_unit, _world, stacks);
                }

                if (_model.Description.Duration.Type == DurationType.TurnBased)
                    _model.DecrementRemainingTurns();

                foreach (var modifier in _model.Description.Modifiers.Where(modifier =>
                             modifier.Type is ModifierExecutionTime.ImmediateWhileActive))
                {
                    if (!IsExpired)
                    {
                        modifier.Tick(_unit, _world, stacks);
                    }
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