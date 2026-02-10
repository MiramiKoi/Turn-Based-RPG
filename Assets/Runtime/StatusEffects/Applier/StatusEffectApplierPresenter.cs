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
            _model.OnRemoveRequested += HandleRemoveRequested;
        }

        public void Disable()
        {
            _model.OnApplyRequested -= HandleApplyRequested;
            _model.OnRemoveRequested -= HandleRemoveRequested;
        }

        private bool CanApply(StatusEffectDescription description)
        {
            var module = LuaRuntime.Instance.GetModuleAsync(description.LuaScript);

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

        private string HandleApplyRequested(string effectId)
        {
            var description = _world.WorldDescription.StatusEffectCollection.Get(effectId);

            if (CanApply(description))
            {
                var model = _model.Collection.Create(effectId);
                return model.Id;
            }

            return null;
        }

        private void HandleRemoveRequested(string effectId)
        {
            _model.Collection.Remove(effectId);
        }
    }
}