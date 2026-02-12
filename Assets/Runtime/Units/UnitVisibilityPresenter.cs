using Runtime.Common;
using Runtime.Units.Components;
using UniRx;

namespace Runtime.Units
{
    public class UnitVisibilityPresenter : IPresenter
    {
        private readonly UnitModel _model;
        private readonly UnitView _view;
        private readonly CompositeDisposable _disposables = new();

        public UnitVisibilityPresenter(UnitModel model, UnitView view)
        {
            _model = model;
            _view = view;
        }

        public void Enable()
        {
            _model.State.Direction.Subscribe(HandleRotationChange).AddTo(_disposables);
        }

        public void Disable()
        {
            _disposables.Dispose();
        }

        private void HandleRotationChange(UnitDirection direction)
        {
            _view.SpriteRenderer.flipX = direction == UnitDirection.Left;
        }
    }

}