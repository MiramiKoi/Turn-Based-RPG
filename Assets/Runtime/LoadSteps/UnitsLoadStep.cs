using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.Common;
using Runtime.Core;
using Runtime.Units.Collection;
using Runtime.ViewDescriptions;
using UnityEngine;

namespace Runtime.LoadSteps
{
    public class UnitsLoadStep : IStep
    {
        private readonly UnitModelCollectionView _collectionView;
        private readonly WorldViewDescriptions _worldViewDescriptions;
        private readonly World _world;

        private readonly List<IPresenter> _presenters;

        public UnitsLoadStep(List<IPresenter> presenters, World world, UnitModelCollectionView collectionView,
            WorldViewDescriptions worldViewDescriptions)
        {
            _presenters = presenters;
            _collectionView = collectionView;
            _world = world;
            _worldViewDescriptions = worldViewDescriptions;
        }

        public async Task Run()
        {
            var presenter = new UnitModelCollectionPresenter(_world.UnitCollection, _collectionView, _world,
                _worldViewDescriptions);
            presenter.Enable();
            _presenters.Add(presenter);

            var bearModel = _world.UnitCollection.Create("bear");
            bearModel.Movement.SetPosition(new Vector2Int(60, 50));

            var bearModel1 = _world.UnitCollection.Create("bear");
            bearModel1.Movement.SetPosition(new Vector2Int(20, 80));

            var pandaModel = _world.UnitCollection.Create("panda");
            pandaModel.Movement.SetPosition(new Vector2Int(60, 80));

            var traderModel = _world.UnitCollection.Create("trader");
            traderModel.Movement.SetPosition(new Vector2Int(55, 50));
            
            var chestModel = _world.UnitCollection.Create("chest");
            chestModel.Movement.SetPosition(new Vector2Int(60, 52));
            _world.GridModel.TryPlace(chestModel, new Vector2Int(60, 52));

            await Task.CompletedTask;
        }
    }
}