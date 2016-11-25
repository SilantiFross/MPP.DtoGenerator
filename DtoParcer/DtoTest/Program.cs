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
            int numberOfMaxTasks;
            var namespaceClasses = ConfigurationManager.AppSettings["namespace"];

            if (IsNumber(out numberOfMaxTasks))
            {
                var generator = new Generator(pathToJson, pathToCsFiles, numberOfMaxTasks, namespaceClasses);
                generator.GenerateClasses();
                WriteItsOkInConsole();
            }
            else
            {
                WriteConfigErrorInConsole();
            }
        }

        private static void WriteItsOkInConsole()
        {
            Console.WriteLine("It's ok");
            Console.ReadLine();
        }

        private static void WriteConfigErrorInConsole()
        {
            Console.WriteLine("Check app.config. key='numberOfTasks' is not a number");
            Console.ReadLine();
        }

        private static bool IsNumber(out int numberOfMaxTasks)
        {
            return int.TryParse(ConfigurationManager.AppSettings["numberOfTasks"], out numberOfMaxTasks);
        }
    }
}