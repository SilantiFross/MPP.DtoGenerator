using System.Collections.Generic;

namespace DtoParcer.Parcer.Table
{
    internal class TableTypes
    {
        private readonly Dictionary<ParcerType, string> _types;

        public TableTypes()
        {
            _types = new Dictionary<ParcerType, string>
            {
                { new ParcerType("integer", "int32"), "int" },
                { new ParcerType("integer", "int64"), "long" },
                { new ParcerType("number", "float"), "float" },
                { new ParcerType("number", "double"), "double" },
                { new ParcerType("string", "byte"), "byte" },
                { new ParcerType("boolean", ""),  "bool"},
                { new ParcerType("string", "date"), "DateTime" },
                { new ParcerType("string", "string"),  "string"}
            };
        }

        public string GetNetType(ParcerType type)
        {
            string netType;
            return _types.TryGetValue(type, out netType) ? netType : "undefined";
        }
    }
}
