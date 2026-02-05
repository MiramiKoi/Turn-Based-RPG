using System;
using System.Collections.Generic;
using NUnit.Framework;
using Runtime.Descriptions.Agents;
using Runtime.Descriptions.Agents.Commands;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Agents.Nodes
{
    public class AgentLeafView : AgentBaseNodeView
    {
        private AgentLeaf LeafData => Data.Node as AgentLeaf;
        
        private DropdownField _commandDropdownField;

        private Dictionary<string, object> _currentParameters;
        
        private VisualElement _parametersContainer;
        
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

            _commandDropdownField = new DropdownField()
            {
                label = "command",
                choices = new List<string>()
                {
                    "log",
                    "has_flag",
                    "set_flag",
                    "has_point_of_interest",
                    "set_random_point_of_interest",
                },
                value = "log"
            };
            
            _commandDropdownField.RegisterValueChangedCallback(OnChangeCommand);
            
            _parametersContainer = new VisualElement();
            
            outputContainer.Add(_commandDropdownField);
            
            outputContainer.Add(_parametersContainer);
            
            SetupFields<LogCommand>();
        }

        private void OnChangeCommand(ChangeEvent<string> evt)
        {
            switch (evt.newValue)
            {
                case "log":
                    SetupFields<LogCommand>();
                    break;
                case "set_flag":
                    SetupFields<SetFlagCommand>();
                    break;
                case "has_flag":
                    SetupFields<HasFlagCommand>();
                    break;
                case  "has_point_of_interest":
                    SetupFields<HasPointOfInterestCommand>();
                    break;
                case  "set_random_point_of_interest":
                    SetupFields<SetRandomPointOfInterest>();
                    break;
            }
        }

        private void SetupFields<T>() where T : CommandDescription, new()
        {
            _parametersContainer.Clear();
            
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
                    object[] vector2IntValue => CreateVector2IntField(parameter.Key, vector2IntValue, (newValue) =>
                    {
                        _currentParameters[parameter.Key] = newValue.ToList();
                        command.Deserialize(_currentParameters);
                    }),
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                LeafData.CommandDescription = command;
                
                _parametersContainer.Add(field);
            }
        }

        private VisualElement CreateVector2IntField(string title, object[] value, Action<Vector2Int> callback)
        {
            var field = new Vector2IntField()
            {
                label = title,
                value = new Vector2Int((int)value[0], (int)value[1]),
            };

            field.RegisterValueChangedCallback(evt => callback?.Invoke(evt.newValue));

            return field;
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