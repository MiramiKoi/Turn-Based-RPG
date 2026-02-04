using System.Threading.Tasks;
using DG.Tweening;
using Runtime.Common;
using Runtime.Core;
using Runtime.TurnBase;
using UniRx;
using UnityEngine;

namespace Runtime.Units
{
    public class UnitPresenter : IPresenter
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private readonly CompositeDisposable _disposables = new();
        
        private readonly UnitModel _unit;
        private readonly UnitView _view;
        private readonly World _world;

        public UnitPresenter(UnitModel unit, UnitView view, World world)
        {
            _unit = unit;
            _view = view;
            _world = world;
        }
        
        public virtual void Enable()
        {
            _unit.Direction.Subscribe(OnRotationChanged).AddTo(_disposables);
            _unit.Position.Subscribe(OnPositionChanged).AddTo(_disposables);
            
            _view.Transform.position = new Vector3(_unit.Position.Value.x, _unit.Position.Value.y, 0);
        }

        public virtual void Disable()
        {
            _disposables.Dispose();
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
    }
}