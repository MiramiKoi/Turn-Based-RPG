using System.Collections.Generic;
using MoonSharp.Interpreter;

namespace Runtime.Common
{
    public class LuaRuntime
    {
        public static LuaRuntime Instance { get; } = new();

        public Script LuaScript { get; }

        private readonly Dictionary<string, Table> _tables = new();

        private LuaRuntime()
        {
            LuaScript = new Script(CoreModules.Preset_Complete);
        }

        public Table GetModuleAsync(string luaKey)
        {
            return _tables[luaKey];
        }

        public bool TryGetModule(string luaKey, out Table table)
        {
            return _tables.TryGetValue(luaKey, out table);
        }

        public void RegisterModule(string luaKey, Table table)
        {
            _tables[luaKey] = table;
        }
    }
}
