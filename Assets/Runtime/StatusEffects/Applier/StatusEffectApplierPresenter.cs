using MoonSharp.Interpreter;
using Runtime.Common;
using Runtime.Core;
using Runtime.Descriptions.StatusEffects;
using Runtime.Units;

namespace Runtime.StatusEffects.Applier
{
    public class StatusEffectApplierPresenter : IPresenter
    {
        private readonly StatusEffectApplierModel _model;
        private readonly UnitModel _unit;
        private readonly World _world;

        public StatusEffectApplierPresenter(StatusEffectApplierModel model, UnitModel unit, World world)
        {
            _model = model;
            _unit = unit;
            _world = world;
        }

        public void Enable()
        {
            _model.OnApplyRequested += HandleApplyRequested;

            foreach (var effectId in _model.ApplyQueue)
            {
                var description = _world.WorldDescription.StatusEffectCollection.Get(effectId);

                if (CanApply(description))
                {
                    _model.Collection.Create(effectId);
                }
            }
            
            _model.ApplyQueue.Clear();
        }

        public void Disable()
        {
            _model.OnApplyRequested -= HandleApplyRequested;
        }

        private bool CanApply(StatusEffectDescription description)
        {
            var module = LuaRuntime.Instance.GetModule(description.LuaScript);

            var function = module.Get("CanApply");

            if (!function.IsNil())
            {
                var context = new Table(LuaRuntime.Instance.LuaScript);
                var effectTable = new Table(LuaRuntime.Instance.LuaScript)
                {
                    ["stacks"] = 1,
                    ["remaining_turns"] = description.Duration.Turns
                };

                context["unit"] = UserData.Create(_unit);
                context["world"] = UserData.Create(_world);
                context["effect"] = effectTable;

                var result = LuaRuntime.Instance.LuaScript.Call(function, context);
                return result.CastToBool();
            }

            return true;
        }

        private void HandleApplyRequested()
        {
            var effectId = _model.ApplyQueue.Dequeue();
            var description = _world.WorldDescription.StatusEffectCollection.Get(effectId);

            if (CanApply(description))
            {
                _model.Collection.Create(effectId);
            }
        }
    }
}