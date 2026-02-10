using System;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Core;
using Runtime.Descriptions;
using Runtime.Stats;
using Runtime.StatusEffects.Applier;
using Runtime.Units;
using UnityEngine;

namespace Runtime.LoadSteps
{
    public class LuaRuntimeLoadStep : IStep
    {
        private readonly AddressableModel _addressableModel;
        private readonly WorldDescription _descriptions;

        public LuaRuntimeLoadStep(AddressableModel addressableModel, WorldDescription descriptions)
        {
            _addressableModel = addressableModel;
            _descriptions = descriptions;
        }

        public async Task Run()
        {
            RegisterUserDataTypes();

            foreach (var statusEffectDescription in _descriptions.StatusEffectCollection.Effects.Values)
            {
                var luaKey = statusEffectDescription.LuaScript;

                var loadModel = _addressableModel.Load<TextAsset>(luaKey);
                await loadModel.LoadAwaiter;

                var env = new Table(LuaRuntime.Instance.LuaScript);

                var meta = new Table(LuaRuntime.Instance.LuaScript)
                {
                    ["__index"] = LuaRuntime.Instance.LuaScript.Globals
                };
                env.MetaTable = meta;

                var moduleValue = LuaRuntime.Instance.LuaScript.DoString(loadModel.Result.text, env, luaKey);

                var table = moduleValue.Type == DataType.Table ? moduleValue.Table : env;
                LuaRuntime.Instance.RegisterModule(luaKey, table);

                _addressableModel.Unload(loadModel);
            }
        }

        private static void RegisterUserDataTypes()
        {
            RegisterTypeIfMissing(typeof(UnitModel));
            RegisterTypeIfMissing(typeof(StatModelCollection));
            RegisterTypeIfMissing(typeof(StatModel));
            RegisterTypeIfMissing(typeof(StatusEffectApplierModel));
            RegisterTypeIfMissing(typeof(World));
        }

        private static void RegisterTypeIfMissing(Type type)
        {
            UserData.RegisterType(type);
        }
    }
}