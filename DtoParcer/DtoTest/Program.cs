using System;
using System.Configuration;
using DtoParcer;

namespace DtoTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var pathToJson = args[0];
            var pathToCsFiles = args[1];
            var numberOfMaxTasks = int.Parse(ConfigurationManager.AppSettings["numberOfTasks"]);
            var namespaceClasses = ConfigurationManager.AppSettings["namespace"];
            var generator = new Generator(pathToJson, pathToCsFiles);

            generator.GenerateClasses();
            Console.ReadLine();
        }
    }
}