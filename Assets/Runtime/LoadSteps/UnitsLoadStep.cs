using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.Common;
using Runtime.Core;
using Runtime.SpawnDirector;
using Runtime.Units.Collection;
using Runtime.ViewDescriptions;

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
            var unitCollectionPresenter = new UnitModelCollectionPresenter(_world.UnitCollection, _collectionView, _world,
                _worldViewDescriptions);
            unitCollectionPresenter.Enable();
            _presenters.Add(unitCollectionPresenter);

            var spawnDirectorPresenter = new SpawnDirectorPresenter(_world.SpawnDirectorModel, _world);
            spawnDirectorPresenter.Enable();
            _presenters.Add(spawnDirectorPresenter);

            await Task.CompletedTask;
        }
    }
}