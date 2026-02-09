using MoonSharp.Interpreter;
using Runtime.Common;
using Runtime.Core;
using Runtime.Units;

namespace Runtime.StatusEffects
{
    public class StatusEffectSystem
    {
        public bool IsExpired => _model.IsExpired;

        private readonly StatusEffectModel _model;
        private readonly Table _module;
        private readonly Table _context;
        private readonly Table _effectTable;

        public StatusEffectSystem(StatusEffectModel model, UnitModel unit, World world)
        {
            _model = model;

            _module = LuaRuntime.Instance.GetModuleAsync(model.Description.LuaScript);
            _context = new Table(LuaRuntime.Instance.LuaScript);
            _effectTable = new Table(LuaRuntime.Instance.LuaScript);

            _context["unit"] = UserData.Create(unit);
            _context["world"] = UserData.Create(world);
            _context["effect"] = _effectTable;
        }

        public void Tick()
        {
            if (CanApply())
            {
                Call("OnTick");
            }
        }

        public void Apply()
        {
            if (CanApply())
                Call("OnApply");
        }

        public void Remove()
        {
            Call("OnRemove");
        }

        private void Call(string functionName)
        {
            var function = _module.Get(functionName);
            
            RefreshEffectTable();
            LuaRuntime.Instance.LuaScript.Call(function, _context);
        }

        private void RefreshEffectTable()
        {
            _effectTable["stacks"] = _model.CurrentStacks.Value;
            _effectTable["remaining_turns"] = _model.RemainingTurns.Value;
        }
        
        public bool CanApply()
        {
            var function = _module.Get("CanApply");

            RefreshEffectTable();
            var result = LuaRuntime.Instance.LuaScript.Call(function, _context);

            return result.Boolean;
        }
    }
}