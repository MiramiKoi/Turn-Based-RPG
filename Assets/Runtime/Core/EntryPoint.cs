using System.Collections.Generic;
using Runtime.AsyncLoad;
using Runtime.CameraControl;
using Runtime.Common;
using Runtime.Descriptions;
using Runtime.Input;
using Runtime.Landscape.Grid;
using Runtime.LoadSteps;
using Runtime.TurnBase;
using Runtime.UI;
using Runtime.UI.Loot;
using Runtime.Units.Collection;
using Runtime.ViewDescriptions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Core
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private UnitModelCollectionView _unitModelCollectionView;
        [SerializeField] private CameraControlView _cameraControlView;
        [SerializeField] private GridView _gridView;
        [SerializeField] private UIDocument _gameplayDocument;

        private readonly World _world = new();
        private readonly WorldDescription _worldDescription = new();
        private readonly WorldViewDescriptions _worldViewDescriptions = new();

        private readonly AddressableModel _addressableModel = new();
        private readonly List<IPresenter> _presenters = new();

        private UIController _uiController;
        private UIContent _uiContent;
        private UIBlocker _uiBlocker;

        private PlayerControls _playerControls;

        private async void Start()
        {
            _playerControls = new PlayerControls();
            _playerControls.Enable();

            _uiContent = new UIContent(_gameplayDocument);

            IStep[] persistentLoadStep =
            {
                new AddressableLoadStep(_addressableModel, _presenters),
                new DescriptionsLoadStep(_worldDescription, _addressableModel),
                new LuaRuntimeLoadStep(_addressableModel, _worldDescription),
                new ViewDescriptionsLoadStep(_worldViewDescriptions, _addressableModel),
                new WorldLoadStep(_world, _addressableModel, _playerControls, _worldDescription,
                    _uiContent.GameplayContent),
                new GridLoadStep(_presenters, _world, _gridView, _worldViewDescriptions),
                new PlayerLoadStep(_presenters, _world, _worldViewDescriptions, _uiContent),
                new UnitsLoadStep(_presenters, _world, _unitModelCollectionView, _worldViewDescriptions),
                new CameraControlLoadStep(_presenters, _cameraControlView, _world)
            };

            foreach (var step in persistentLoadStep)
            {
                await step.Run();
            }

            _uiController = new UIController(_world, _playerControls, _worldViewDescriptions, _uiContent);
            _uiController.Enable();

            _world.TurnBaseModel.Steps.Clear();
            var turnBasePresenter = new TurnBasePresenter(_world.TurnBaseModel, _world);

            turnBasePresenter.Enable();

            var lootPresenter = new LootPresenter(_world, _uiContent, _worldViewDescriptions);
            lootPresenter.Enable();
        }

        private void Update()
        {
            _world.GameSystems?.Update(Time.deltaTime);
        }
    }
}