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
        
        private async Task AnimateMoveChanged(Vector2Int position)
        {
            await _view.Transform.DOMove(new Vector3(position.x, position.y, 0), 0.2f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        }

        private void OnRotationChanged(UnitDirection direction)
        {
            _view.SpriteRenderer.flipX = direction == UnitDirection.Left;
        }

        private void OnPositionChanged(Vector2Int position)
        {
            var step = new StepModel(StepType.Parallel, StepAction, _unit.Awaiter);
            _world.TurnBaseModel.Steps.Enqueue(step);
            return;

            async void StepAction()
            {
                _view.Animator.SetBool(IsMoving, true);
                await AnimateMoveChanged(position);
                _view.Animator.SetBool(IsMoving, false);

                _unit.Awaiter.Complete();
            }
        }

        private void OnAttacked()
        {
            var step = new StepModel(StepType.Parallel, StepAction, _unit.Awaiter);
            _world.TurnBaseModel.Steps.Enqueue(step);
            return;
            
            async void StepAction()
            {
                _view.Animator.SetBool(IsAttacking, true);
                await Task.Delay((int)(_view.Animator.GetCurrentAnimatorStateInfo(0).length * 1000));
                _view.Animator.SetBool(IsAttacking, false);
                
                _unit.Awaiter.Complete();
            }
        }

        private void OnDamaged()
        {
            var step = new StepModel(StepType.Consistent, StepAction, _unit.Awaiter);
            _world.TurnBaseModel.Steps.Enqueue(step);
            return;
            
            async void StepAction()
            {
                _view.Animator.SetBool(IsDamaging, true);
                await Task.Delay((int)(_view.Animator.GetCurrentAnimatorStateInfo(0).length * 1000));
                _view.Animator.SetBool(IsDamaging, false);
                
                var awaiter = _unit.Awaiter;
                if (_unit.Health <= 0)
                {
                    _unit.Await();
                    var nextStep = new StepModel(StepType.Consistent, DeathAction, _unit.Awaiter);
                    _world.TurnBaseModel.Steps.Enqueue(nextStep);
                }
                awaiter.Complete();
            }
            
            async void DeathAction()
            {
                _view.Animator.SetBool(IsDead, true);
                await Task.Delay((int)(_view.Animator.GetCurrentAnimatorStateInfo(0).length * 1000));
                _view.Animator.SetBool(IsDamaging, false);
                
                _unit.Awaiter.Complete();
            }
        }
    }
}
