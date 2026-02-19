using MoonSharp.Interpreter;
using Runtime.Common;
using Runtime.Common.ObjectPool;
using Runtime.Core;
using Runtime.CustomAsync;
using Runtime.Descriptions.StatusEffects.Enums;
using Runtime.Units;
using UnityEngine;

namespace Runtime.StatusEffects
{
    public class StatusEffectPresenter : IPresenter
    {
        private readonly StatusEffectModel _model;
        private readonly IObjectPool<StatusEffectView> _pool;
        private readonly UnitModel _unit;
        private readonly World _world;

        private Table _module;
        private Table _context;
        private Table _effectTable;

        private StatusEffectView _view;

        public StatusEffectPresenter(StatusEffectModel model, IObjectPool<StatusEffectView> pool, UnitModel unit,
            World world)
        {
            _model = model;
            _pool = pool;
            _unit = unit;
            _world = world;

        }

        public void Enable()
        {
            _module = LuaRuntime.Instance.GetModule(_model.Description.LuaScript);
            _context = new Table(LuaRuntime.Instance.LuaScript);
            _effectTable = new Table(LuaRuntime.Instance.LuaScript);

            _context["unit"] = UserData.Create(_unit);
            _context["world"] = UserData.Create(_world);
            _context["effect"] = _effectTable;
            
            _view = _pool.Get();

            if (CallBool("CanApply"))
            {
                Call("OnApply");
                PlayParticle(_view.OnApplyParticle);
            }

            switch (_model.Description.Polarity)
            {
                case Polarity.Buff:
                    _world.TurnBaseModel.OnBuffTick += HandleTick;
                    break;
                case Polarity.Debuff:
                    _world.TurnBaseModel.OnDebuffTick += HandleTick;
                    break;
                case Polarity.Mixed:
                    _world.TurnBaseModel.OnMixedBuffTick += HandleTick;
                    break;
            }
        }

        public async void Disable()
        {
            Call("OnRemove");

            PlayParticle(_view.OnRemoveParticle);

            StopParticle(_view.OnApplyParticle);

            var awaiter = new ScheduleAwaiter(_view.OnRemoveParticle.main.duration);
            awaiter.Start();
            await awaiter;

            _pool.Release(_view);

            switch (_model.Description.Polarity)
            {
                case Polarity.Buff:
                    _world.TurnBaseModel.OnBuffTick -= HandleTick;
                    break;
                case Polarity.Debuff:
                    _world.TurnBaseModel.OnDebuffTick -= HandleTick;
                    break;
                case Polarity.Mixed:
                    _world.TurnBaseModel.OnMixedBuffTick -= HandleTick;
                    break;
            }
            
            _module = null;
            _context = null;
            _effectTable =null;
        }

        private void PlayParticle(ParticleSystem system)
        {
            if (system != null)
            {
                system.Play();
            }
        }

        private void StopParticle(ParticleSystem system)
        {
            if (system != null)
            {
                system.Stop();
            }
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
            PlayParticle(_view.OnTickParticle);

            if (!CallBool("CanTick"))
            {
                _model.IsExpired = true;
            }

            _model.DecrementRemainingTurns();
        }
    }
}