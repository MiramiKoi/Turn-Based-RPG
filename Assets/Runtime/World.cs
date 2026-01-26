using Runtime.Landscape.Grid;
using Runtime.Landscape.Grid.Interaction;
using UnityEngine;

namespace Runtime
{
    public class World
    {
        public PlayerControls PlayerControls { get; private set; }
        public GridModel GridModel { get; private set; }
        public GridInteractionModel GridInteractionModel { get; private set; }
        public Camera MainCamera { get; private set; }

        public void SetData(PlayerControls playerControls)
        {
            PlayerControls = playerControls;
            
            GridModel = new GridModel();
            GridInteractionModel = new GridInteractionModel();

            MainCamera = Camera.main;
        }
    }
}