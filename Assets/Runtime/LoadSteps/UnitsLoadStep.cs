using System.Linq;
using System.Threading.Tasks;
using Runtime.Core;
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
            var characterModel = new UnitModel
            (
                "character", 
                _world.WorldDescription.UnitCollection.First(),
                new Vector2Int(5, 4)
            );
            
            _world.GridModel.TryPlace(characterModel, characterModel.Position.Value);
            _world.UnitCollection.Add(characterModel.Id, characterModel);
            
            var bearModel = new UnitModel
            (
                "bear", 
                _world.WorldDescription.UnitCollection.Last(), 
                new Vector2Int(5, 5)
            );
            
            _world.GridModel.TryPlace(bearModel, bearModel.Position.Value);
            _world.UnitCollection.Add(bearModel.Id, bearModel);
            
            return Task.CompletedTask;
        }
    }
}