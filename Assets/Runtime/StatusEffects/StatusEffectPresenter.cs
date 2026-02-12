using MoonSharp.Interpreter;
using Runtime.Common;
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
        private readonly StatusEffectView _view;
        private readonly World _world;
        private readonly Table _module;
        private readonly Table _context;
        private readonly Table _effectTable;

        public StatusEffectPresenter(StatusEffectModel model, StatusEffectView view, UnitModel unit, World world)
        {
            _model = model;
            _view = view;
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

            Object.Destroy(_view.GameObject);

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