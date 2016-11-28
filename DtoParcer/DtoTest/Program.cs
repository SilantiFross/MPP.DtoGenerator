using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.IO;
using System.Text;
using DtoParcer;
using DtoParcer.GenerationUnits;
using Newtonsoft.Json;

namespace DtoTest
{
    internal class Program
    {
        private static void Main(string[] args)
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

            var csClasses = GenerateCsClasses(numberOfMaxTasks, namespaceClasses, collectionOfClasses);
            WriteGeneratedCsClassesInFiles(csClasses, routerGenerator);
            WriteMessageInConsole("It's ok");
        }

        private static void WriteGeneratedCsClassesInFiles(ConcurrentQueue<StringBuilder> csClasses,
            RouterGenerator routerGenerator)
        {
            var writer = new Writer.Writer();
            writer.WriteClassesInCsFile(csClasses, routerGenerator.PathToGeneratedClasses);
        }

        private static ConcurrentQueue<StringBuilder> GenerateCsClasses(int numberOfMaxTasks, string namespaceClasses,
            CollectionOfClasses collectionOfClasses)
        {
            var generator = new Generator(numberOfMaxTasks, namespaceClasses);
            var csClasses = generator.GenerateClasses(collectionOfClasses);
            return csClasses;
        }

        private static void WriteMessageInConsole(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}