using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fastJSON;
using Runtime.AsyncLoad;
using Runtime.Descriptions;
using UnityEngine;

namespace Runtime.LoadSteps
{
    public class DescriptionsLoadStep : IStep
    {
        private readonly WorldDescription _worldDescription;
        private readonly AddressableModel _addressableModel;

        private readonly Dictionary<string, List<string>> _keys = new()
        {
            { "camera_control", new List<string> { "camera-control-description" } },

            { "surface_generation", new List<string> { "surface-generation-description" } },
            { "surfaces", new List<string> { "surface-description" } },
            { "environment_generation", new List<string> { "environment-generation-description" } },
            { "environment", new List<string> { "environment-description" } },
            { "locations", new List<string> { "locations-description" } },

            { "units", new List<string> { "unit-description" } },
            {
                "agent_decisions", new List<string>
                {
                    "bear-description",
                    "panda-description",
                    "trader-description",
                    "archer-description",
                    "sheep-description",
                    "loot-description",
                    "hunter-description",
                    "run-away-description",
                    "patrol-description"
                }
            },
            { "spawn_director", new List<string> { "spawn-director-description" } },

            { "items", new List<string> { "items-description" } },

            { "status_effects", new List<string> { "status-effect-descriptions" } }
        };

        public DescriptionsLoadStep(
            WorldDescription worldDescription,
            AddressableModel addressableModel)
        {
            _worldDescription = worldDescription;
            _addressableModel = addressableModel;
        }

        public async Task Run()
        {
            var nodeTasks = _keys.Select(async kvp =>
            {
                var nodeData = await LoadNode(kvp.Value);
                return (key: kvp.Key, value: nodeData);
            });

            var results = await Task.WhenAll(nodeTasks);

            var finalData = results.ToDictionary(x => x.key, x => x.value);

            _worldDescription.SetData(finalData);
        }

        private async Task<object> LoadNode(List<string> addressKeys)
        {
            if (addressKeys.Count == 1)
            {
                return await LoadJson(addressKeys[0]);
            }

            var loadTasks = addressKeys.Select(async key =>
            {
                var json = await LoadJson(key);
                return (id: ExtractId(key), json);
            });

            var results = await Task.WhenAll(loadTasks);

            return results.ToDictionary(x => x.id, x => (object)x.json);
        }

        private async Task<Dictionary<string, object>> LoadJson(string addressKey)
        {
            var loadModel = _addressableModel.Load<TextAsset>(addressKey);
            await loadModel.LoadAwaiter;

            return JSON.ToObject<Dictionary<string, object>>(loadModel.Result.text);
        }

        private static string ExtractId(string addressKey)
        {
            return addressKey.Replace("-description", "");
        }
    }
}