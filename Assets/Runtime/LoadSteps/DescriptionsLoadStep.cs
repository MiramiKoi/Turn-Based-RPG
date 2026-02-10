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

        private readonly Dictionary<string, string> _keys = new()
        {
            { "camera_control", "camera-control-description" },
            { "surface_generation", "surface-generation-description" },
            { "surfaces", "surface-description" },
            { "units", "unit-description" },
            { "items", "items-description" },
            { "bear", "bear-description" },
            { "panda", "panda-description" },
            { "trader", "trader-description" },
            { "environment_generation", "environment-generation-description" },
            { "environment", "environment-description" },
            { "status_effects", "status-effect-descriptions" }
        };

        public DescriptionsLoadStep(WorldDescription worldDescription, AddressableModel addressableModel)
        {
            _worldDescription = worldDescription;
            _addressableModel = addressableModel;
        }

        public async Task Run()
        {
            var data = new Dictionary<string, object>();

            var tasks = _keys.Select(async kvp =>
            {
                var loadModel = _addressableModel.Load<TextAsset>(kvp.Value);
                await loadModel.LoadAwaiter;
                var parsed = JSON.ToObject<Dictionary<string, object>>(loadModel.Result.text);
                data[kvp.Key] = parsed;
            }).ToArray();

            await Task.WhenAll(tasks);

            _worldDescription.SetData(data);
        }
    }
}