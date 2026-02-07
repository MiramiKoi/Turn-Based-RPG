using System.Threading.Tasks;
using DG.Tweening;
using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Core;
using Runtime.CustomAsync;
using Runtime.Stats;
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
        
        private LoadModel<VisualTreeAsset> _statusEffectsLoadModel;

        public UnitPresenter(UnitModel unit, UnitView view, World world, WorldViewDescriptions viewDescriptions)
        {
            _unit = unit;
            _view = view;
            _world = world;
            _viewDescriptions = viewDescriptions;
        }
        
        public async void Enable()
        {
            foreach (var stat in _unit.Stats)
            {
                var statPresenter = new StatPresenter(stat);
                statPresenter.Enable();
            }
            
            _unit.Direction.Subscribe(OnRotationChanged).AddTo(_disposables);
            _unit.Position.Subscribe(OnPositionChanged).AddTo(_disposables);
            _unit.OnAttacked += OnAttacked;
            _unit.OnDamaging += OnDamaged;
            
            _view.Transform.position = new Vector3(_unit.Position.Value.x, _unit.Position.Value.y, 0);

            _statusEffectsLoadModel = _world.AddressableModel.Load<VisualTreeAsset>(_viewDescriptions.StatusEffectViewDescriptions.StatusEffectContainerAsset.AssetGUID);
            await _statusEffectsLoadModel.LoadAwaiter;
            _statusEffectsPresenter = new StatusEffectCollectionPresenter(_unit, _world);
            _statusEffectsPresenter.Enable();
        }

        public void Disable()
        {
            _unit.OnDamaging -= OnDamaged;
            _unit.OnAttacked -= OnAttacked;
            _disposables.Dispose();

            _world.AddressableModel.Unload(_statusEffectsLoadModel);
            _statusEffectsPresenter.Disable();
            _statusEffectsPresenter = null;
        }
        
        private void OnRotationChanged(UnitDirection direction)
        {
            _view.SpriteRenderer.flipX = direction == UnitDirection.Left;
        }

        private async void OnPositionChanged(Vector2Int position)
        {
            await WaiteRender(StepType.Parallel, PlayMoveAnimation(position));
        }

        private async void OnAttacked()
        {
            await WaiteRender(StepType.Parallel, PlayAnimation(IsAttacking));
        }

        private async void OnDamaged()
        {
            await WaiteRender(StepType.Consistent, PlayDamagingAnimation());
        }

        private async Task PlayMoveAnimation(Vector2Int position)
        {
            _view.Transform.DOMove(new Vector3(position.x, position.y, 0), 0.2f).SetEase(Ease.Linear);
            await PlayAnimation(IsMoving, 0.2f);
        }

        private async Task PlayDamagingAnimation()
        {
            await PlayAnimation(IsDamaging);

            if (_unit.Health <= 0)
            {
                await PlayAnimation(IsDead);
            }
        }
        
        private async Task WaiteRender(StepType stepType, Task render)
        {
            var allowedAwaiter = new CustomAwaiter();
            var completedAwaiter = new CustomAwaiter();
            
            var step = new StepModel(stepType, allowedAwaiter, completedAwaiter);
            _world.TurnBaseModel.Steps.Enqueue(step);
            
            await allowedAwaiter;
            
            await render;
            
            completedAwaiter.Complete();
        }
        
        private async Task PlayAnimation(int animationId)
        {
            _view.Animator.SetBool(animationId, true);
            var scheduleAwaiter = new ScheduleAwaiter(_view.Animator.GetCurrentAnimatorStateInfo(0).length, _world.Scheduler);
            scheduleAwaiter.Start();
            await scheduleAwaiter;
            _view.Animator.SetBool(animationId, false);
        }
        
        private async Task PlayAnimation(int animationId, float duration)
        {
            _view.Animator.SetBool(animationId, true);
            var scheduleAwaiter = new ScheduleAwaiter(duration, _world.Scheduler);
            scheduleAwaiter.Start();
            await scheduleAwaiter;
            _view.Animator.SetBool(animationId, false);
        }
    }
}
