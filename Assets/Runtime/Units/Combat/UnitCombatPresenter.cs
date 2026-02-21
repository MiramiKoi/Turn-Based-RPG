using System.Threading.Tasks;
using Runtime.Common;
using Runtime.Core;
using Runtime.CustomAsync;
using Runtime.TurnBase;
using UnityEngine;

namespace Runtime.Units.Combat
{
    public class UnitCombatPresenter : IPresenter
    {
        private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
        private static readonly int IsDamaging = Animator.StringToHash("IsDamaging");
        private static readonly int IsDead = Animator.StringToHash("IsDead");

        private readonly UnitModel _model;
        private readonly UnitView _view;
        private readonly World _world;

        public UnitCombatPresenter(UnitModel model, UnitView view, World world)
        {
            _model = model;
            _view = view;
            _world = world;
        }

        public void Enable()
        {
            _model.Combat.OnAttacked += HandleAttack;
            _model.Combat.OnDamaged += HandleDamage;
        }

        public void Disable()
        {
            _model.Combat.OnAttacked -= HandleAttack;
            _model.Combat.OnDamaged -= HandleDamage;
        }

        private async void HandleAttack()
        {
            var step = new StepModel(StepType.Parallel);
            _world.TurnBaseModel.Steps.Enqueue(step);

            await step.AllowedAwaiter;

            await PlayAnimation(IsAttacking);

            step.CompletedAwaiter.Complete();
        }

        private async void HandleDamage()
        {
            var step = new StepModel(StepType.Consistent);
            _world.TurnBaseModel.Steps.Enqueue(step);

            await step.AllowedAwaiter;

            await PlayAnimation(IsDamaging);

            if (_model.Health <= 0)
            {
                await PlayAnimation(IsDead);
            }

            step.CompletedAwaiter.Complete();
        }

        private async Task PlayAnimation(int animationId)
        {
            if (!_model.State.Visible.Value)
            {
                return;
            }
            
            _view.Animator.SetBool(animationId, true);
            var scheduleAwaiter = new ScheduleAwaiter(_view.Animator.GetCurrentAnimatorStateInfo(0).length);
            scheduleAwaiter.Start();
            await scheduleAwaiter;
            _view.Animator.SetBool(animationId, false);
        }
    }
}