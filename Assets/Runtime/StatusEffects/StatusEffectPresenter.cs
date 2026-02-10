using MoonSharp.Interpreter;
using Runtime.Common;
using Runtime.Core;
using Runtime.Units;

namespace Runtime.StatusEffects
{
    public class StatusEffectPresenter : IPresenter
    {
        private readonly StatusEffectModel _model;
        private readonly World _world;
        private readonly Table _module;
        private readonly Table _context;
        private readonly Table _effectTable;

        public StatusEffectPresenter(StatusEffectModel model, UnitModel unit, World world)
        {
            _model = model;
            _world = world;

            _module = LuaRuntime.Instance.GetModuleAsync(model.Description.LuaScript);
            _context = new Table(LuaRuntime.Instance.LuaScript);
            _effectTable = new Table(LuaRuntime.Instance.LuaScript);

            _context["unit"] = UserData.Create(unit);
            _context["world"] = UserData.Create(world);
            _context["effect"] = _effectTable;
        }

        public void Enable()
        {
            if (CanApply())
            {
                Call("OnApply");
            }

            _world.TurnBaseModel.OnWorldStepFinished += HandleTick;
        }

        public void Disable()
        {
            Call("OnRemove");

            _world.TurnBaseModel.OnWorldStepFinished -= HandleTick;
        }

        private void Call(string functionName)
        {
            var function = _module.Get(functionName);

            if (!function.IsNil())
            {
                RefreshEffectTable();
                LuaRuntime.Instance.LuaScript.Call(function, _context);
            }
        }

        private void RefreshEffectTable()
        {
            _effectTable["stacks"] = _model.CurrentStacks.Value;
            _effectTable["remaining_turns"] = _model.RemainingTurns.Value;
        }

        private bool CanApply()
        {
            var function = _module.Get("CanApply");

            if (!function.IsNil())
            {
                RefreshEffectTable();
                var result = LuaRuntime.Instance.LuaScript.Call(function, _context);

                return result.Boolean;
            }

            return true;
        }

        private void HandleTick()
        {
            if (!_model.IsExpired)
            {
                Call("OnTick");
            }
        }
    }
}