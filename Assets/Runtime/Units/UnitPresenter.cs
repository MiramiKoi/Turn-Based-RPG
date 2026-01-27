using System;
using System.Collections.Generic;
using Runtime.Common;
using UniRx;
using UnityEngine;

namespace Runtime.Units
{
    public class UnitPresenter : IPresenter
    {
        private readonly List<IDisposable> _disposables = new();
        
        private readonly UnitModel _unit;
        private readonly UnitView _view;

        public UnitPresenter(UnitModel unit, UnitView view)
        {
            _unit = unit;
            _view = view;
        }
        
        public void Enable()
        {
            _unit.Position.Subscribe(OnPositionChanged).AddTo(_disposables);
            OnPositionChanged(_unit.Position.Value);
        }

        public void Disable()
        {
            _disposables.ForEach(x => x.Dispose());
            _disposables.Clear();
        }
        
        private void OnPositionChanged(Vector2Int position)
        {
            _view.Transform.position = new Vector3(position.x, -position.y, 0);
            _view.Transform.position = new Vector3(position.x, position.y, 0);
        }
    }
}