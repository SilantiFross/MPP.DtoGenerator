using System.Collections.Generic;

namespace DtoParcer.Parcer.Table
{
    internal class TableTypes
    {
        private readonly Dictionary<JsonType, string> _types;

        public TableTypes()
        {
            _types = new Dictionary<JsonType, string>
            {
                { new JsonType("integer", "int32"), "int" },
                { new JsonType("integer", "int64"), "long" },
                { new JsonType("number", "float"), "float" },
                { new JsonType("number", "double"), "double" },
                { new JsonType("string", "byte"), "byte" },
                { new JsonType("boolean", ""),  "bool"},
                { new JsonType("string", "date"), "DateTime" },
                { new JsonType("string", "string"),  "string"}
            };
        }

        public string GetNetType(JsonType jsonType)
        {
            string netType;
            return _types.TryGetValue(jsonType, out netType) ? netType : string.Empty;
        }
    }
}
