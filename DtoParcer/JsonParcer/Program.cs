using System;
using System.Configuration;

namespace JsonParcer
{
    class Program
    {
        static void Main(string[] args)
        {
            var limitOfTasks = int.Parse(ConfigurationManager.AppSettings["limitOfTasks"]);
            var namespaceParse = ConfigurationManager.AppSettings["namespaceParser"];
        }
    }
}
