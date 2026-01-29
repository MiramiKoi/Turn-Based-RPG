using System;
using System.Collections.Generic;
using Runtime.Agents.Nodes;
using UnityEngine;

namespace Runtime.Agents
{
    [Serializable]
    public class Unit : IUnit
    {
        public IReadOnlyDictionary<string, IUnitCommand> AvailableCommands => _commands;
        public IReadOnlyDictionary<string, bool> Flags => _flags;

        private Dictionary<string, IUnitCommand> _commands;

        private Dictionary<string, bool> _flags;
        
        [SerializeField] private bool HasPlayer;

        [SerializeField] private bool HasHouse;

        public Unit()
        {
            _flags = new Dictionary<string, bool>();
            _commands = new Dictionary<string, IUnitCommand>();
        }

        public void RegisterCommand(string key, IUnitCommand command)
        {
            _commands.Add(key, command);
        }

        public void Update()
        {
            _flags["has_player"] = HasPlayer;
            _flags["has_house"] = HasHouse;
        }
    }
}