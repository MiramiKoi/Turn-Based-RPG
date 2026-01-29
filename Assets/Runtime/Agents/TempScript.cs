using System.Collections.Generic;
using fastJSON;
using Runtime.Agents.Nodes;
using Runtime.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Agents
{
    public class TempScript : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        
        private AgentDecisionRoot _decisionRoot;
        private TempWorld _tempWorld;
        private PlayerControls _playerControls;

        private void Start()
        {
            _playerControls = new PlayerControls();
            
            _playerControls.Enable();

            _playerControls.Player.Attack.performed += OnAttack;
            
            var hasPlayerCommand = new HasFlagCommand("has_player");
            var hasHouseCommand = new HasFlagCommand("has_house");

            var attackPlayerCommand = new LogCommand("attack");
            var goHomeCommand = new LogCommand("move");
            
            _unit.RegisterCommand("has_player", hasPlayerCommand);
            _unit.RegisterCommand("has_house", hasHouseCommand);
            _unit.RegisterCommand("attack", attackPlayerCommand);
            _unit.RegisterCommand("move", goHomeCommand);
            
            var json = Resources.Load<TextAsset>("unit");
            
            var jsonDictionary = JSON.ToObject<Dictionary<string, object>>(json.text);

            _decisionRoot = new AgentDecisionRoot();
            
            _decisionRoot.Deserialize(jsonDictionary);

            _tempWorld = new TempWorld();
        }

        private void Update()
        {
            _unit.Update();
        }

        private void OnAttack(InputAction.CallbackContext obj)
        {
            Debug.Log("Click");
            
            _decisionRoot.Process(_tempWorld, _unit);
        }
    }

    public class TempWorld : IWorldContext
    {
        
    }
}