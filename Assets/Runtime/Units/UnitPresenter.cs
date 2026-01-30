using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Runtime.Common;
using UniRx;
using UnityEngine;

namespace Runtime.Units
{
    public class UnitPresenter : IPresenter
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private readonly List<IDisposable> _disposables = new();
        
        private readonly UnitModel _unit;
        private readonly UnitView _view;

        public UnitPresenter(UnitModel unit, UnitView view)
        {
            _unit = unit;
            _view = view;
        }
        
        public virtual void Enable()
        {
            _unit.Direction.Subscribe(OnRotationChanged).AddTo(_disposables);
            _unit.Position.Subscribe(OnPositionChanged).AddTo(_disposables);
            
            _view.Transform.position = new Vector3(_unit.Position.Value.x, _unit.Position.Value.y, 0);
        }

        public virtual void Disable()
        {
            _disposables.ForEach(x => x.Dispose());
            _disposables.Clear();
        }
        
        protected async Task AnimateMoveChanged(Vector2Int position)
        {
            await _view.Transform.DOMove(new Vector3(position.x, position.y, 0), 0.2f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        }

        private void OnRotationChanged(UnitDirection direction)
        {
            _view.SpriteRenderer.flipX = direction == UnitDirection.Left;
        }

        private async void OnPositionChanged(Vector2Int position)
        {
            _view.Animator.SetBool(IsMoving, true);
            await AnimateMoveChanged(position);
            _view.Animator.SetBool(IsMoving, false);
            
            _unit.Awaiter.Complete();
        }
    }
}