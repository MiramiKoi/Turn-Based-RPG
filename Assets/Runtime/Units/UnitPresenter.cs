using System.Threading.Tasks;
using DG.Tweening;
using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Common.ObjectPool;
using Runtime.Core;
using Runtime.CustomAsync;
using Runtime.Stats;
using Runtime.StatusEffects.Applier;
using Runtime.StatusEffects.Collection;
using Runtime.TurnBase;
using Runtime.Units.Components;
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

        protected UnitView View;

        private readonly UnitModel _model;
        private readonly World _world;
        private readonly WorldViewDescriptions _viewDescriptions;
        private readonly IObjectPool<UnitView> _pool;

        private readonly CompositeDisposable _disposables = new();

        private StatusEffectCollectionPresenter _statusEffectsPresenter;
        private StatusEffectApplierPresenter _statusEffectApplierPresenter;

        private LoadModel<VisualTreeAsset> _statusEffectsLoadModel;

        private UnitVisiblePresenter _unitVisiblePresenter;

        public UnitPresenter(UnitModel model, IObjectPool<UnitView> pool, World world,
            WorldViewDescriptions viewDescriptions)
        {
            _model = model;
            _pool = pool;
            _world = world;
            _viewDescriptions = viewDescriptions;
        }

        public virtual async void Enable()
        {
            View = _pool.Get();

            foreach (var stat in _model.Stats)
            {
                var statPresenter = new StatPresenter(stat);
                statPresenter.Enable();
            }

            _unitVisiblePresenter = new UnitVisiblePresenter(_model, View);
            _unitVisiblePresenter.Enable();

            _model.State.Direction.Subscribe(OnRotationChanged).AddTo(_disposables);
            _model.State.Position.Subscribe(OnPositionChanged).AddTo(_disposables);
            _model.Combat.OnAttacked += OnAttacked;
            _model.Combat.OnDamaged += OnDamaged;

            View.Transform.position = new Vector3(_model.State.Position.Value.x, _model.State.Position.Value.y, 0);
            _statusEffectApplierPresenter = new StatusEffectApplierPresenter(_model.Effects, _model, _world);
            _statusEffectApplierPresenter.Enable();

            _statusEffectsLoadModel = _world.AddressableModel.Load<VisualTreeAsset>(_viewDescriptions
                .StatusEffectViewDescriptions.StatusEffectContainerAsset.AssetGUID);
            await _statusEffectsLoadModel.LoadAwaiter;

            _statusEffectsPresenter = new StatusEffectCollectionPresenter(_model, View, _world, _viewDescriptions);
            _statusEffectsPresenter.Enable();
        }

        public virtual void Disable()
        {
            _pool.Release(View);

            _model.Combat.OnAttacked -= OnAttacked;
            _model.Combat.OnDamaged -= OnDamaged;
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
            View.SpriteRenderer.flipX = direction == UnitDirection.Left;
        }

        private async void OnPositionChanged(Vector2Int position)
        {
            _world.GridModel.GetCell(_model.State.Position.Value).Release();
            _world.GridModel.GetCell(position).Occupied(_model);

            var step = CreateStep(StepType.Parallel);

            await step.AllowedAwaiter;

            View.Transform.DOMove(new Vector3(position.x, position.y, 0), 0.2f).SetEase(Ease.Linear);
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

            if (_model.Health <= 0)
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
            View.Animator.SetBool(animationId, true);
            var scheduleAwaiter = new ScheduleAwaiter(View.Animator.GetCurrentAnimatorStateInfo(0).length);
            scheduleAwaiter.Start();
            await scheduleAwaiter;
            View.Animator.SetBool(animationId, false);
        }

        private async Task PlayAnimation(int animationId, float duration)
        {
            View.Animator.SetBool(animationId, true);
            var scheduleAwaiter = new ScheduleAwaiter(duration);
            scheduleAwaiter.Start();
            await scheduleAwaiter;
            View.Animator.SetBool(animationId, false);
        }
    }
}