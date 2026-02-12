using System.Collections.Generic;
using Runtime.AsyncLoad;
using Runtime.CameraControl;
using Runtime.Common;
using Runtime.Descriptions;
using Runtime.Input;
using Runtime.Landscape.Grid;
using Runtime.LoadSteps;
using Runtime.Units.Collection;
using Runtime.ViewDescriptions;
using UnityEditor;
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

        private PlayerControls _playerControls;

        private async void Start()
        {
            _playerControls = new PlayerControls();
            _playerControls.Enable();

            IStep[] persistentLoadStep =
            {
                new AddressableLoadStep(_addressableModel, _presenters),
                new DescriptionsLoadStep(_worldDescription, _addressableModel),
                new LuaRuntimeLoadStep(_addressableModel, _worldDescription),
                new ViewDescriptionsLoadStep(_worldViewDescriptions, _addressableModel, _gameplayDocument),
                new WorldLoadStep(_world, _addressableModel, _playerControls, _worldDescription),
                new TurnBaseLoadStep(_presenters, _world),
                new GridLoadStep(_presenters, _world, _gridView, _worldViewDescriptions),
                new PlayerLoadStep(_presenters, _world, _worldViewDescriptions),
                new UnitsLoadStep(_presenters, _world, _unitModelCollectionView, _worldViewDescriptions),
                new CameraControlLoadStep(_presenters, _cameraControlView, _world),
                new UILoadStep(_presenters, _world, _worldViewDescriptions)
            };

            foreach (var step in persistentLoadStep)
            {
                await step.Run();
            }

            Application.quitting += OnQuit;
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
        }

        private void Update()
        {
            _world.GameSystems?.Update(Time.deltaTime);
        }

#if UNITY_EDITOR
        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                Dispose();
            }
        }
#endif

        private void OnQuit()
        {
            Dispose();
        }

        private void Dispose()
        {
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
#endif
            Application.quitting -= OnQuit;

            for (var i = _presenters.Count - 1; i >= 0; i--)
            {
                _presenters[i].Disable();
            }
        }
    }
}