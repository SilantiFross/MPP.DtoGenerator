using System;
using System.Configuration;
using System.IO;
using DtoParcer;
using DtoParcer.GenerationUnits;
using Newtonsoft.Json;

namespace DtoTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var routerGenerator = new RouterGenerator(args[0], args[1]);
            var namespaceClasses = ConfigurationManager.AppSettings["namespace"];
            int numberOfMaxTasks;

            if (!int.TryParse(ConfigurationManager.AppSettings["numberOfTasks"], out numberOfMaxTasks))
            {
                WriteMessageInConsole("Check app.config. 'numberOfTasks' not a number");
                return;
            }

            var collectionOfClasses =
                JsonConvert.DeserializeObject<CollectionOfClasses>(File.ReadAllText(routerGenerator.PathToJson));

            var generator = new Generator(numberOfMaxTasks, namespaceClasses);
            var csClasses = generator.GenerateClasses(collectionOfClasses);

            var writer = new Writer.Writer();
            writer.WriteClassesInCsFile(csClasses, routerGenerator.PathToGeneratedClasses);
            WriteMessageInConsole("It's ok");
        }

        private static void WriteMessageInConsole(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}