using System.Threading.Tasks;
using Runtime.AsyncLoad;
using Runtime.Core;
using Runtime.Descriptions;
using Runtime.Input;

namespace Runtime.LoadSteps
{
    public class WorldLoadStep : IStep
    {
        private readonly AddressableModel _addressableModel;
        private readonly PlayerControls _playerControls;
        private readonly World _world;
        private readonly WorldDescription _worldDescription;

        public WorldLoadStep(World world, AddressableModel addressableModel, PlayerControls playerControls,
            WorldDescription worldDescription)
        {
            _addressableModel = addressableModel;
            _playerControls = playerControls;
            _worldDescription = worldDescription;
            _world = world;
        }

        public Task Run()
        {
            _world.SetData(_addressableModel, _playerControls, _worldDescription);
            return Task.CompletedTask;
        }
    }
}