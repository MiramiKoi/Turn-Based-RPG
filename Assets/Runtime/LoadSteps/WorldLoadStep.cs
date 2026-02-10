using System.Threading.Tasks;
using Runtime.AsyncLoad;
using Runtime.Core;
using Runtime.Descriptions;
using Runtime.Input;
using UnityEngine.UIElements;

namespace Runtime.LoadSteps
{
    public class WorldLoadStep : IStep
    {
        private readonly AddressableModel _addressableModel;
        private readonly PlayerControls _playerControls;
        private readonly World _world;
        private readonly WorldDescription _worldDescription;
        private readonly VisualElement _uiRoot;

        public WorldLoadStep(World world, AddressableModel addressableModel, PlayerControls playerControls,
            WorldDescription worldDescription, VisualElement uiRoot)
        {
            _addressableModel = addressableModel;
            _playerControls = playerControls;
            _worldDescription = worldDescription;
            _uiRoot = uiRoot;
            _world = world;
        }

        public Task Run()
        {
            _world.SetData(_addressableModel, _playerControls, _worldDescription, _uiRoot);
            return Task.CompletedTask;
        }
    }
}