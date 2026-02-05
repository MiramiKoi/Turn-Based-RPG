using Runtime.Agents;
using Runtime.Agents.Nodes;
using Runtime.AsyncLoad;
using Runtime.CameraControl;
using Runtime.Descriptions;
using Runtime.GameSystems;
using Runtime.Input;
using Runtime.Landscape.Grid;
using Runtime.Landscape.Grid.Interaction;
using Runtime.TurnBase;
using Runtime.UI.Inventory;
using Runtime.Units;
using UnityEngine;

namespace Runtime.Core
{
    public class World : IWorldContext
    {
        public AddressableModel AddressableModel { get; private set; }
        public Camera MainCamera { get; private set; }
        public CameraControlModel CameraControlModel { get; private set; }
        public TurnBaseModel TurnBaseModel { get; private set; }
        public PlayerControls PlayerControls { get; private set; }
        public GridModel GridModel { get; private set; }
        public InventoryModel InventoryModel { get; private set; }
        public GridInteractionModel GridInteractionModel { get; private set; }
        public WorldDescription WorldDescription { get; private set; }
        public GameSystemCollection GameSystems { get; private set; }
        public UnitModelCollection UnitCollection { get; private set; }
        public AgentModelCollection AgentCollection { get; private set; }

        public void SetData(AddressableModel addressableModel, PlayerControls playerControls, WorldDescription worldDescription)
        {
            WorldDescription = worldDescription;
            
            AddressableModel = addressableModel;

            TurnBaseModel = new TurnBaseModel();
            PlayerControls = playerControls;
            
            GridModel = new GridModel(WorldDescription.SurfaceGenerationDescription.Generate(), worldDescription.SurfaceCollection);
            GridInteractionModel = new GridInteractionModel();

            MainCamera = Camera.main;
            CameraControlModel = new CameraControlModel();
            
            GameSystems = new GameSystemCollection();
            
            UnitCollection = new UnitModelCollection();
            AgentCollection = new AgentModelCollection();

            InventoryModel = new InventoryModel(16);
        }
    }
}