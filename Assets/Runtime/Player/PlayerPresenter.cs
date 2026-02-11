using Runtime.Core;
using Runtime.Landscape.Grid.Indication;
using Runtime.Units;
using Runtime.ViewDescriptions;
using UnityEngine.InputSystem;

namespace Runtime.Player
{
    public class PlayerPresenter : UnitPresenter
    {
        private readonly PlayerModel _model;
        private readonly World _world;

        private PlayerVisionPresenter _visionPresenter;
        private PlayerPathPresenter _pathPresenter;
        private PlayerInteractionPresenter _interactionPresenter;

        public PlayerPresenter(PlayerModel model, UnitView view, World world,
            WorldViewDescriptions worldViewDescriptions)
            : base(model, view, world, worldViewDescriptions)
        {
            _model = model;
            _world = world;
        }

        public override void Enable()
        {
            base.Enable();
            
            _visionPresenter = new PlayerVisionPresenter(_world.GridModel, _world.UnitCollection, _model);
            _visionPresenter.Enable();

            _pathPresenter = new PlayerPathPresenter(_model, _world);
            _pathPresenter.Enable();
            
            _interactionPresenter = new PlayerInteractionPresenter(_model, _world);
            _interactionPresenter.Enable();
        }

        public override void Disable()
        {
            base.Disable();
            
            _interactionPresenter.Disable();
            _interactionPresenter = null;

            _pathPresenter.Disable();
            _pathPresenter = null;
            
            _visionPresenter.Disable();
            _visionPresenter = null;
        }
    }
}