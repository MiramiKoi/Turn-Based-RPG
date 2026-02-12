using Runtime.AsyncLoad;
using Runtime.CameraControl;
using Runtime.Descriptions;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.GameSystems;
using Runtime.Input;
using Runtime.Landscape.Grid;
using Runtime.Landscape.Grid.Interaction;
using Runtime.Player;
using Runtime.TurnBase;
using Runtime.UI.Blocker;
using Runtime.UI.Loot;
using Runtime.UI.Transfer;
using Runtime.Units.Collection;
using UnityEngine;
using UnityEngine.UIElements;

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
        public GridInteractionModel GridInteractionModel { get; private set; }
        public WorldDescription WorldDescription { get; private set; }
        public GameSystemCollection GameSystems { get; private set; }
        public UnitModelCollection UnitCollection { get; private set; }
        public PlayerModel PlayerModel => (PlayerModel)UnitCollection.Get("character_0");
        public UIBlockerModel UIBlockerModel { get; private set; }
        public LootModel LootModel { get; private set; }
        public TransferModel TransferModel { get; private set; }

        public void SetData(AddressableModel addressableModel, PlayerControls playerControls,
            WorldDescription worldDescription)
        {
            WorldDescription = worldDescription;

            AddressableModel = addressableModel;

            TurnBaseModel = new TurnBaseModel();
            PlayerControls = playerControls;

            GridModel = new GridModel(WorldDescription);
            GridInteractionModel = new GridInteractionModel();

            MainCamera = Camera.main;
            CameraControlModel = new CameraControlModel();

            GameSystems = new GameSystemCollection();

            UnitCollection = new UnitModelCollection(this, WorldDescription);

            UIBlockerModel = new UIBlockerModel();

            LootModel = new LootModel();
            TransferModel = new TransferModel();
        }
    }
}