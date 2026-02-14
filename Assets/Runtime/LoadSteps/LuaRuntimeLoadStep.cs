using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Core;
using Runtime.Descriptions;
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
            RegisterModelTypes();
            RegisterDescriptionTypes();

            foreach (var statusEffectDescription in _descriptions.StatusEffectCollection.Effects.Values)
            {
                var luaKey = statusEffectDescription.LuaScript;

                var loadModel = _addressableModel.Load<TextAsset>("Lua/" + luaKey + ".lua");
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

        private static void RegisterModelTypes()
        {
            var assembly = typeof(World).Assembly;

            var types = GetModelTypes(assembly);

            foreach (var type in types)
            {
                UserData.RegisterType(type);
            }
        }
        
        private static void RegisterDescriptionTypes()
        {
            var assembly = typeof(World).Assembly;

            var types = GetDescriptionsTypes(assembly);

            foreach (var type in types)
            {
                UserData.RegisterType(type);
            }
        }

        private static IEnumerable<Type> GetModelTypes(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(IsModelType);
        }

        private static IEnumerable<Type> GetDescriptionsTypes(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(IsRuntimeDescription);
        }
        
        private static bool IsModelType(Type type)
        {
            if (!type.IsClass ||
                !type.IsPublic ||
                type.IsAbstract ||
                type.IsGenericTypeDefinition)
                return false;

            if (type.Namespace == null)
                return false;

            if (!(type.Namespace == "Runtime" || type.Namespace.StartsWith("Runtime.")))
                return false;

            var name = type.Name;

            if (name.EndsWith("ModelCollection", StringComparison.Ordinal))
                return true;

            if (name.EndsWith("Model", StringComparison.Ordinal))
                return true;

            return false;
        }

        private static bool IsRuntimeDescription(Type type)
        {
            if (!type.IsClass ||
                !type.IsPublic ||
                type.IsAbstract ||
                type.IsGenericTypeDefinition)
                return false;

            if (type.Namespace == null)
                return false;

            var ns = type.Namespace;

            if (!(ns == "Runtime" || ns.StartsWith("Runtime")))
                return false;

            if (ns.Contains(".ViewDescriptions"))
                return false;

            if (!type.Name.EndsWith("Description", StringComparison.Ordinal))
                return false;

            if (type.Name == "ViewDescription")
                return false;

            return true;
        }
    }
}