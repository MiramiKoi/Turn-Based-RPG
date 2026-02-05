using System;
using System.Collections.Generic;
using Runtime.Descriptions.Agents;
using Runtime.Descriptions.Agents.Nodes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Agents.Nodes
{
    public class AgentLeafView : AgentBaseNodeView
    {
        private AgentLeaf LeafData => Data.Node as AgentLeaf;
        
        private DropdownField _commandDropdownField;

        private Dictionary<string, object> _currentParameters;

        public AgentLeafView(AgentNodeEditorWrapper wrapper) : base(wrapper)
        {
            
        }
        
        public AgentLeafView(AgentLeaf data) : base(data)
        {

        }

        public override void SaveData()
        {
            base.SaveData();
        }

        protected override void Setup()
        {
            base.Setup();
            
            outputContainer.Clear();

            AddCommandDropdown();
            
            SetupFields<LogCommand>();
        }

        private void OnChangeCommand(ChangeEvent<string> evt)
        {
            switch (evt.newValue)
            {
                case "log":
                    SetupFields<LogCommand>();
                    break;
            }
        }

        private void SetupFields<T>() where T : CommandDescription, new()
        {
            outputContainer.Clear();
            
            AddCommandDropdown();
            
            var command = new T();

            _currentParameters = command.Serialize();

            foreach (var parameter in _currentParameters)
            {
                if (parameter.Key == CommandDescription.TypeKey)
                {
                    continue;
                }
                
                VisualElement field = parameter.Value switch
                {
                    int intValue => CreateIntField(parameter.Key, intValue, (newValue) =>
                    {
                        _currentParameters[parameter.Key] = newValue;
                        command.Deserialize(_currentParameters);
                    }),
                    float floatValue => CreateFloatField(parameter.Key, floatValue, (newValue) =>
                    {
                        _currentParameters[parameter.Key] = newValue;
                        command.Deserialize(_currentParameters);
                    }),
                    bool boolValue => CreateBoolField(parameter.Key, boolValue, (newValue) =>
                    {
                        _currentParameters[parameter.Key] = newValue;
                        command.Deserialize(_currentParameters);
                    }),
                    string stringValue => CreateTextField(parameter.Key, stringValue, (newValue) =>
                    {
                        _currentParameters[parameter.Key] = newValue;
                        command.Deserialize(_currentParameters);
                    }),
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                LeafData.CommandDescription = command;
                
                outputContainer.Add(field);
            }
        }

        private void AddCommandDropdown()
        {
            _commandDropdownField = new DropdownField()
            {
                label = "command",
                choices = new List<string>()
                {
                    "log",
                    "has_flag",
                },
                value = "log"
            };
            
            _commandDropdownField.RegisterValueChangedCallback(OnChangeCommand);
            
            outputContainer.Add(_commandDropdownField);
        }

        private VisualElement CreateIntField(string title, int value, Action<int> callback)
        {
            var field = new IntegerField()
            {
                label = title,
                value = value
            };
            
            field.RegisterValueChangedCallback(e => callback(e.newValue));
            
            return field;
        }
        
        private VisualElement CreateTextField(string title, string value, Action<string> callback)
        {
            var field = new TextField()
            {
                label = title,
                value = value
            };
            
            field.RegisterValueChangedCallback(evt => callback(evt.newValue));
            
            return field;
        }
        
        private VisualElement CreateFloatField(string title, float value, Action<float> callback)
        {
            var field = new FloatField()
            {
                label = title,
                value = value,
            };

            field.RegisterValueChangedCallback(evt => callback?.Invoke(evt.newValue));
            
            return field;
        }

        private VisualElement CreateBoolField(string title, bool value, Action<bool> callback)
        {
            var toggle = new Toggle()
            {
                label = title,
                value = value
            };

            toggle.RegisterValueChangedCallback(evt => callback?.Invoke(evt.newValue));
            
            return toggle;
        }
    }
}