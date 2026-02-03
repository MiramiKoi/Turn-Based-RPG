using Runtime.Common;
using UniRx;
using UnityEngine;

namespace Runtime.CameraControl
{
    public class CameraControlPresenter : IPresenter
    {
        private readonly CameraControlModel _model;
        private readonly CameraControlView _view;
        private readonly CompositeDisposable _disposables = new();

        public CameraControlPresenter(CameraControlModel model, CameraControlView view)
        {
            _model = model;
            _view = view;
        } 
        
        public void Enable()
        {
            _model.Target.Subscribe(HandleTargetChanged).AddTo(_disposables);
        }

        public void Disable()
        {
            _disposables.Dispose();
        }

        private void HandleTargetChanged(Transform target)
        {
            _view.Camera.Follow = target;
        }
    }
}