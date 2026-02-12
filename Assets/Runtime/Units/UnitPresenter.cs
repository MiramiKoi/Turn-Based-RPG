using System.Threading.Tasks;
using DG.Tweening;
using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Core;
using Runtime.CustomAsync;
using Runtime.Stats;
using Runtime.StatusEffects.Applier;
using Runtime.StatusEffects.Collection;
using Runtime.TurnBase;
using Runtime.ViewDescriptions;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Units
{
    public class UnitPresenter : IPresenter
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
        private static readonly int IsDamaging = Animator.StringToHash("IsDamaging");
        private static readonly int IsDead = Animator.StringToHash("IsDead");

        private readonly CompositeDisposable _disposables = new();

        private readonly UnitModel _unit;
        private readonly UnitView _view;
        private readonly World _world;
        private readonly WorldViewDescriptions _viewDescriptions;
        private StatusEffectCollectionPresenter _statusEffectsPresenter;
        private StatusEffectApplierPresenter _statusEffectApplierPresenter;

        private LoadModel<VisualTreeAsset> _statusEffectsLoadModel;

        private UnitVisiblePresenter _unitVisiblePresenter;

        public UnitPresenter(UnitModel unit, UnitView view, World world, WorldViewDescriptions viewDescriptions)
        {
            _unit = unit;
            _view = view;
            _world = world;
            _viewDescriptions = viewDescriptions;
        }

        public virtual async void Enable()
        {
            foreach (var stat in _unit.Stats)
            {
                var statPresenter = new StatPresenter(stat);
                statPresenter.Enable();
            }

            _unitVisiblePresenter = new UnitVisiblePresenter(_unit, _view);
            _unitVisiblePresenter.Enable();

            _unit.Direction.Subscribe(OnRotationChanged).AddTo(_disposables);
            _unit.Position.Subscribe(OnPositionChanged).AddTo(_disposables);
            _unit.OnAttacked += OnAttacked;
            _unit.OnDamaging += OnDamaged;

            _view.Transform.position = new Vector3(_unit.Position.Value.x, _unit.Position.Value.y, 0);

            _statusEffectsLoadModel = _world.AddressableModel.Load<VisualTreeAsset>(_viewDescriptions
                .StatusEffectViewDescriptions.StatusEffectContainerAsset.AssetGUID);
            await _statusEffectsLoadModel.LoadAwaiter;
            _statusEffectsPresenter = new StatusEffectCollectionPresenter(_unit, _world);
            _statusEffectApplierPresenter = new StatusEffectApplierPresenter(_unit.ActiveEffects, _unit, _world);

            _statusEffectsPresenter.Enable();
            _statusEffectApplierPresenter.Enable();
        }

        public virtual void Disable()
        {
            _unit.OnDamaging -= OnDamaged;
            _unit.OnAttacked -= OnAttacked;
            _disposables.Dispose();

            _world.AddressableModel.Unload(_statusEffectsLoadModel);
            _statusEffectsPresenter.Disable();
            _statusEffectApplierPresenter.Disable();

            _statusEffectsPresenter = null;
            _statusEffectApplierPresenter = null;

            _unitVisiblePresenter.Disable();
            _unitVisiblePresenter = null;
        }

        private void OnRotationChanged(UnitDirection direction)
        {
            _view.SpriteRenderer.flipX = direction == UnitDirection.Left;
        }

        private async void OnPositionChanged(Vector2Int position)
        {
            var step = CreateStep(StepType.Parallel);

            await step.AllowedAwaiter;

            _view.Transform.DOMove(new Vector3(position.x, position.y, 0), 0.2f).SetEase(Ease.Linear);
            await PlayAnimation(IsMoving, 0.2f);

            step.CompletedAwaiter.Complete();
        }

        private async void OnAttacked()
        {
            var step = CreateStep(StepType.Parallel);

            await step.AllowedAwaiter;

            await PlayAnimation(IsAttacking);

            step.CompletedAwaiter.Complete();
        }

        private async void OnDamaged()
        {
            var step = CreateStep(StepType.Consistent);

            await step.AllowedAwaiter;

            await PlayAnimation(IsDamaging);

            if (_unit.Health <= 0)
            {
                await PlayAnimation(IsDead);
            }

            step.CompletedAwaiter.Complete();
        }

        private StepModel CreateStep(StepType stepType)
        {
            var step = new StepModel(stepType);
            _world.TurnBaseModel.Steps.Enqueue(step);

            return step;
        }

        private async Task PlayAnimation(int animationId)
        {
            _view.Animator.SetBool(animationId, true);
            var scheduleAwaiter = new ScheduleAwaiter(_view.Animator.GetCurrentAnimatorStateInfo(0).length);
            scheduleAwaiter.Start();
            await scheduleAwaiter;
            _view.Animator.SetBool(animationId, false);
        }

        private async Task PlayAnimation(int animationId, float duration)
        {
            _view.Animator.SetBool(animationId, true);
            var scheduleAwaiter = new ScheduleAwaiter(duration);
            scheduleAwaiter.Start();
            await scheduleAwaiter;
            _view.Animator.SetBool(animationId, false);
        }
    }
}