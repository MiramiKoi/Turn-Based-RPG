using System.Linq;
using System.Threading.Tasks;
using Runtime.Core;
using Runtime.Player;
using Runtime.Units;
using UnityEngine;

namespace Runtime.LoadSteps
{
    public class UnitsLoadStep : IStep
    {
        private readonly World _world;

        public UnitsLoadStep(World world)
        {
            _world = world;
        }

        public Task Run()
        {
            var characterModel = new PlayerModel
            (
                "character",
                new Vector2Int(50, 49), _world.WorldDescription.UnitCollection.First(), _world.WorldDescription);

            _world.GridModel.TryPlace(characterModel, characterModel.Position.Value);
            _world.UnitCollection.Add(characterModel.Id, characterModel);

            var bearModel = new UnitModel
            (
                "bear_0",
                new Vector2Int(60, 50), _world.WorldDescription.UnitCollection["bear"], _world.WorldDescription);

            _world.GridModel.TryPlace(bearModel, bearModel.Position.Value);
            _world.UnitCollection.Add(bearModel.Id, bearModel);

            var bearModel1 = new UnitModel
            (
                "bear_1",
                new Vector2Int(20, 80), _world.WorldDescription.UnitCollection["bear"], _world.WorldDescription);

            _world.GridModel.TryPlace(bearModel1, bearModel1.Position.Value);
            _world.UnitCollection.Add(bearModel1.Id, bearModel1);

            var pandaModel = new UnitModel("panda_0", new Vector2Int(60, 80),
                _world.WorldDescription.UnitCollection["panda"], _world.WorldDescription);

            _world.GridModel.TryPlace(pandaModel, pandaModel.Position.Value);
            _world.UnitCollection.Add(pandaModel.Id, pandaModel);

            var traderModel = new UnitModel("trader_0", new Vector2Int(53, 48),
                _world.WorldDescription.UnitCollection["trader"], _world.WorldDescription);

            _world.GridModel.TryPlace(traderModel, traderModel.Position.Value);
            _world.UnitCollection.Add(traderModel.Id, traderModel);

            return Task.CompletedTask;
        }
    }
}