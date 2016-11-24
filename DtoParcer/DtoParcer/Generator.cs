using System;
using System.IO;
using DtoParcer.GenerationUnits;
using Newtonsoft.Json;

namespace DtoParcer
{
    public class Generator
    {
        private readonly RouterGeneration _routerGeneration;
        private CollectionOfClasses _collectionOfClasses;

        public Generator(string pathToJson, string pathToGeneratedClasses)
        {
            _routerGeneration = new RouterGeneration(pathToJson, pathToGeneratedClasses);
        }

        public void GenerateClasses()
        {
            _collectionOfClasses = JsonConvert.DeserializeObject<CollectionOfClasses>(File.ReadAllText(_routerGeneration.PathToJson));

            foreach (var classDescription in _collectionOfClasses.ClassDescriptions)
            {
                Console.WriteLine(classDescription.ClassName);
                foreach (var property in classDescription.Properties)
                {
                    Console.WriteLine(property.Format);
                    Console.WriteLine(property.Type);
                    Console.WriteLine(property.Name);
                }
                Console.WriteLine();
            }
        }
    }
}
