using System;
using System.Collections.Generic;
using Runtime.Descriptions.Agents.Commands;
using Runtime.Descriptions.Agents.Nodes;
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

        protected override void Setup()
        {
            base.Setup();

            outputContainer.Clear();

            _commandDropdownField = new DropdownField
            {
                label = "command",
                choices =
                {
                    "log",
                    "set_flag",
                    "has_flag",
                    "has_point_of_interest",
                    "set_random_point_of_interest",
                    "distance_point_of_interest",
                    "move_to_point_of_interest",
                    "move_from_point_of_interest",
                    "has_unit_with_fraction",
                    "has_unit_with_friendly_fraction",
                    "has_unit_with_enemy_fraction",
                    "set_point_of_interest_with_fraction",
                    "set_point_of_interest_with_friendly_fraction",
                    "set_point_of_interest_with_enemy_fraction",
                    "set_point_of_interest_with_another_fraction",
                    "can_place_point_of_interest",
                    "attack_point_of_interest",
                    "stat_condition",
                    "check_stat_point_of_interest",
                    "check_stat_percent",
                    "enter_to_battle",
                    "leave_battle",
                    "can_see_point_of_interest",
                    "apply_random_effect_point_of_interest"
                },
                value = LeafData.CommandDescription == null ? "log" : LeafData.CommandDescription.Type
            };

            _commandDropdownField.RegisterValueChangedCallback(OnChangeCommand);

            _parametersContainer = new VisualElement();

            outputContainer.Add(_commandDropdownField);

            outputContainer.Add(_parametersContainer);

            if (LeafData.CommandDescription == null)
                SetupNewCommand(_commandDropdownField.value);
            else
                SetupCommand(LeafData.CommandDescription);
        }

        private void OnChangeCommand(ChangeEvent<string> evt)
        {
            SetupNewCommand(evt.newValue);
        }

        private void SetupCommand(CommandDescription command)
        {
            SetupFields(command);
        }

        private void SetupNewCommand(string command)
        {
            switch (command)
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
                case "has_point_of_interest":
                    SetupFields<HasPointOfInterestCommand>();
                    break;
                case "set_random_point_of_interest":
                    SetupFields<SetRandomPointOfInterestCommand>();
                    break;
                case "distance_point_of_interest":
                    SetupFields<DistancePointOfInterestCommand>();
                    break;
                case "move_to_point_of_interest":
                    SetupFields<MoveToPointOfInterestCommand>();
                    break;
                case "move_from_point_of_interest":
                    SetupFields<MoveFromPointOfInterestCommand>();
                    break;
                case "has_unit_with_fraction":
                    SetupFields<HasUnitWithFractionCommand>();
                    break;
                case "has_unit_with_friendly_fraction":
                    SetupFields<HasUnitWithFriendlyFractionCommand>();
                    break;
                case "has_unit_with_enemy_fraction":
                    SetupFields<HasUnitWithEnemyFractionCommand>();
                    break;
                case "set_point_of_interest_with_fraction":
                    SetupFields<SetPointOfInterestWithFractionCommand>();
                    break;
                case "set_point_of_interest_with_friendly_fraction":
                    SetupFields<SetPointOfInterestWithFriendlyFractionCommand>();
                    break;
                case "set_point_of_interest_with_enemy_fraction":
                    SetupFields<SetPointOfInterestWithEnemyFractionCommand>();
                    break;
                case "set_point_of_interest_with_another_fraction":
                    SetupFields<SetPointOfInterestWithAnotherFractionCommand>();
                    break;
                case "can_place_point_of_interest":
                    SetupFields<CanPlacePointOfInterestCommand>();
                    break;
                case "attack_point_of_interest":
                    SetupFields<AttackPointOfInterestCommand>();
                    break;
                case "stat_condition":
                    SetupFields<CheckStatCommand>();
                    break;
                case "has_unit_with_another_fraction":
                    SetupFields<HasUnitWithAnotherFractionCommand>();
                    break;
                case "check_stat_point_of_interest":
                    SetupFields<CheckStatPointOfInterest>();
                    break;
                case "can_see_point_of_interest":
                    SetupFields<CanSeePointOfInterest>();
                    break;
                case "check_stat_percent":
                    SetupFields<CheckStatPercentCommand>();
                    break;
                case "apply_random_effect_point_of_interest":
                    SetupFields<ApplyRandomEffectPointOfInterest>();
                    break;
            }
        }


        private void SetupFields<T>() where T : CommandDescription, new()
        {
            _parametersContainer.Clear();

            var command = new T();

            SetupFields(command);
        }

        private void SetupFields(CommandDescription command)
        {
            _currentParameters = command.Serialize();

            foreach (var parameter in _currentParameters)
            {
                if (parameter.Key == CommandDescription.TypeKey) continue;

                var field = parameter.Value switch
                {
                    int intValue => CreateIntField(parameter.Key, intValue,
                        newValue => UpdateParameter(command, parameter.Key, newValue)),
                    float floatValue => CreateFloatField(parameter.Key, floatValue,
                        newValue => UpdateParameter(command, parameter.Key, newValue)),
                    bool boolValue => CreateBoolField(parameter.Key, boolValue,
                        newValue => UpdateParameter(command, parameter.Key, newValue)),
                    string stringValue => CreateTextField(parameter.Key, stringValue,
                        newValue => UpdateParameter(command, parameter.Key, newValue)),
                    _ => throw new ArgumentOutOfRangeException()
                };


                _parametersContainer.Add(field);
            }

            LeafData.CommandDescription = command;
        }

        private void UpdateParameter<T>(T command, string key, object value) where T : CommandDescription
        {
            _currentParameters[key] = value;
            command.Deserialize(_currentParameters);
        }

        private VisualElement CreateIntField(string label, int value, Action<int> callback)
        {
            var field = new IntegerField
            {
                label = label,
                value = value
            };

            field.RegisterValueChangedCallback(e => callback(e.newValue));

            return field;
        }

        private VisualElement CreateTextField(string label, string value, Action<string> callback)
        {
            var field = new TextField
            {
                label = label,
                value = value
            };

            field.RegisterValueChangedCallback(evt => callback(evt.newValue));

            return field;
        }

        private VisualElement CreateFloatField(string label, float value, Action<float> callback)
        {
            var field = new FloatField
            {
                label = label,
                value = value
            };

            field.RegisterValueChangedCallback(evt => callback?.Invoke(evt.newValue));

            return field;
        }

        private VisualElement CreateBoolField(string label, bool value, Action<bool> callback)
        {
            var toggle = new Toggle
            {
                label = label,
                value = value
            };

            toggle.RegisterValueChangedCallback(evt => callback?.Invoke(evt.newValue));

            return toggle;
        }
    }
}