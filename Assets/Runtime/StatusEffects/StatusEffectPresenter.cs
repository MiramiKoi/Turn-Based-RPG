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
            if (CallBool("CanApply"))
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
        
        private bool CallBool(string functionName)
        {
            var function = _module.Get(functionName);

            if (!function.IsNil())
            {
                RefreshEffectTable();
                var result = LuaRuntime.Instance.LuaScript.Call(function, _context);

                return result.Boolean;
            }

            return true;
        }

        private void RefreshEffectTable()
        {
            _effectTable["stacks"] = _model.CurrentStacks.Value;
            _effectTable["remaining_turns"] = _model.RemainingTurns.Value;
        }

        private void HandleTick()
        {
            Call("OnTick");
            
            if (!CallBool("CanTick"))
            {
                _model.IsExpired = true;
            }
            
            _model.DecrementRemainingTurns();
        }
    }
}