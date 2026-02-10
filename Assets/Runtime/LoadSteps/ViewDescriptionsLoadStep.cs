using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Runtime.AsyncLoad;
using Runtime.ViewDescriptions;
using Runtime.ViewDescriptions.Inventory;
using Runtime.ViewDescriptions.Items;
using Runtime.ViewDescriptions.Landscape.Environment;
using Runtime.ViewDescriptions.Landscape.Grid;
using Runtime.ViewDescriptions.Landscape.Surface;
using Runtime.ViewDescriptions.StatusEffects;
using Runtime.ViewDescriptions.Units;

namespace Runtime.LoadSteps
{
    public class ViewDescriptionsLoadStep : IStep
    {
        private readonly AddressableModel _addressableModel;
        private readonly Dictionary<string, Action<object>> _loadMap;

        public ViewDescriptionsLoadStep(WorldViewDescriptions worldViewDescriptions, AddressableModel addressableModel)
        {
            _addressableModel = addressableModel;

            _loadMap = new Dictionary<string, Action<object>>
            {
                {
                    "GridIndicationViewDescription",
                    obj => worldViewDescriptions.GridIndicationViewDescription = obj as GridIndicationViewDescription
                },
                {
                    "SurfaceViewDescriptionCollection",
                    obj => worldViewDescriptions.SurfaceViewDescriptions = obj as SurfaceViewDescriptionCollection
                },
                {
                    "UnitViewDescriptionCollection",
                    obj => worldViewDescriptions.UnitViewDescriptions = obj as UnitViewDescriptionCollection
                },
                {
                    "InventoryViewDescription",
                    obj => worldViewDescriptions.InventoryViewDescription = obj as InventoryViewDescription
                },
                {
                    "ItemViewDescriptionCollection",
                    obj => worldViewDescriptions.ItemViewDescriptions = obj as ItemViewDescriptionCollection
                },
                {
                    "StatusEffectViewDescriptionCollection",
                    obj => worldViewDescriptions.StatusEffectViewDescriptions =
                        obj as StatusEffectViewDescriptionCollection
                },
                {
                    "EnvironmentViewDescriptionCollection",
                    obj => worldViewDescriptions.EnvironmentViewDescriptions =
                        obj as EnvironmentViewDescriptionCollection
                }
            };
        }

        public async Task Run()
        {
            var tasks = _loadMap.Select(async kvp =>
            {
                var model = _addressableModel.Load<object>(kvp.Key);

                await model.LoadAwaiter;

                kvp.Value(model.Result);
            }).ToArray();

            await Task.WhenAll(tasks);
        }
    }
}