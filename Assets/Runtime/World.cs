using Runtime.Input;
using Runtime.Landscape.Grid;
using Runtime.Landscape.Grid.Interaction;
using Runtime.TurnBase;
using UnityEngine;

namespace Runtime
{
    public class World
    {
        public TurnBaseModel TurnBaseModel { get; private set; }
        public PlayerControls PlayerControls { get; private set; }
        public GridModel GridModel { get; private set; }
        public GridInteractionModel GridInteractionModel { get; private set; }
        public Camera MainCamera { get; private set; }

        public void SetData(PlayerControls playerControls)
        {
            TurnBaseModel = new TurnBaseModel();
            PlayerControls = playerControls;
            
            GridModel = new GridModel();
            GridInteractionModel = new GridInteractionModel();

            MainCamera = Camera.main;
        }
    }
}