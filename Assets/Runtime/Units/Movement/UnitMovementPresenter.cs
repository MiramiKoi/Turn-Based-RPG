using System.Threading.Tasks;
using DG.Tweening;
using Runtime.Common;
using Runtime.Core;
using Runtime.CustomAsync;
using Runtime.TurnBase;
using UnityEngine;

namespace Runtime.Units.Movement
{
    public class UnitMovementPresenter : IPresenter
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

        private readonly UnitModel _model;
        private readonly UnitView _view;
        private readonly World _world;

        public UnitMovementPresenter(UnitModel model, UnitView view, World world)
        {
            _model = model;
            _view = view;
            _world = world;
        }

        public void Enable()
        {
            _model.Movement.OnMove += HandleMove;
            _view.Transform.position = new Vector3(_model.State.Position.Value.x, _model.State.Position.Value.y, 0);
        }

        public void Disable()
        {
            _model.Movement.OnMove -= HandleMove;
        }

        private async void HandleMove(Vector2Int position)
        {
            _world.GridModel.ReleaseCell(_model.State.Position.Value);
            _world.GridModel.GetCell(position).Occupied(_model);

            var step = new StepModel(StepType.Parallel);
            _world.TurnBaseModel.Steps.Enqueue(step);

            await step.AllowedAwaiter;

            _view.Transform.DOMove(new Vector3(position.x, position.y, 0), 0.2f).SetEase(Ease.Linear);
            await PlayAnimation(IsMoving, 0.2f);

            step.CompletedAwaiter.Complete();
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