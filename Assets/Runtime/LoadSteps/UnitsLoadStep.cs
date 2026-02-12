using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.Common;
using Runtime.Core;
using Runtime.Descriptions;
using Runtime.Units.Collection;
using Runtime.ViewDescriptions;

namespace Runtime.LoadSteps
{
    public class UnitsLoadStep : IStep
    {
        private readonly UnitModelCollection _modelCollection;
        private readonly UnitModelCollectionView _collectionView;
        private readonly WorldViewDescriptions _worldViewDescriptions;
        private readonly WorldDescription _worldDescription;
        private readonly World _world;

        private readonly List<IPresenter> _presenters;

        public UnitsLoadStep(List<IPresenter> presenters, World world, UnitModelCollectionView collectionView,
            WorldDescription worldDescription,
            WorldViewDescriptions worldViewDescriptions)
        {
            _presenters = presenters;
            _modelCollection = world.UnitCollection;
            _collectionView = collectionView;
            _world = world;
            _worldDescription = worldDescription;
            _worldViewDescriptions = worldViewDescriptions;
        }

        public async Task Run()
        {
            var presenter = new UnitModelCollectionPresenter(_world.UnitCollection, _collectionView, _world, _worldViewDescriptions);
            presenter.Enable();
            _presenters.Add(presenter);
            
            // var bearModel = new UnitModel
            // (
            //     "bear_0",
            //     new Vector2Int(60, 50), _world.WorldDescription.UnitCollection["bear"], _world.WorldDescription);
            //
            // _world.GridModel.TryPlace(bearModel, bearModel.Position.Value);
            // _world.UnitCollection.Add(bearModel.Id, bearModel);
            //
            // var bearModel1 = new UnitModel
            // (
            //     "bear_1",
            //     new Vector2Int(20, 80), _world.WorldDescription.UnitCollection["bear"], _world.WorldDescription);
            //
            // _world.GridModel.TryPlace(bearModel1, bearModel1.Position.Value);
            // _world.UnitCollection.Add(bearModel1.Id, bearModel1);
            //
            // var pandaModel = new UnitModel("panda_0", new Vector2Int(60, 80),
            //     _world.WorldDescription.UnitCollection["panda"], _world.WorldDescription);
            //
            // _world.GridModel.TryPlace(pandaModel, pandaModel.Position.Value);
            // _world.UnitCollection.Add(pandaModel.Id, pandaModel);
            //
            // var traderModel = new UnitModel("trader_0", new Vector2Int(80, 50),
            //     _world.WorldDescription.UnitCollection["trader"], _world.WorldDescription);
            //
            // _world.GridModel.TryPlace(traderModel, traderModel.Position.Value);
            // _world.UnitCollection.Add(traderModel.Id, traderModel);

            await Task.CompletedTask;
        }
    }
}