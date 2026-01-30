using Runtime.Descriptions;
using Runtime.GameSystems;
using Runtime.Input;
using Runtime.Landscape.Grid;
using Runtime.Landscape.Grid.Interaction;
using Runtime.TurnBase;
using UnityEngine;

namespace Runtime.Core
{
    public class World
    {
        public TurnBaseModel TurnBaseModel { get; private set; }
        public PlayerControls PlayerControls { get; private set; }
        public GridModel GridModel { get; private set; }
        public GridInteractionModel GridInteractionModel { get; private set; }
        public Camera MainCamera { get; private set; }
        public WorldDescription WorldDescription { get; private set; }
        public GameSystemCollection GameSystems { get; private set; }

        public void SetData(PlayerControls playerControls, WorldDescription worldDescription)
        {
            WorldDescription = worldDescription;

            TurnBaseModel = new TurnBaseModel();
            PlayerControls = playerControls;
            
            GridModel = new GridModel(WorldDescription.SurfaceGenerationDescription.Generate(), worldDescription.SurfaceCollection);
            GridInteractionModel = new GridInteractionModel();

            MainCamera = Camera.main;
            GameSystems = new GameSystemCollection();
        }
    }
}