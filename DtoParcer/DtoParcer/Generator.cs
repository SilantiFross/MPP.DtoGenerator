using System.IO;
using DtoParcer.GenerationUnits;
using DtoParcer.Parcer.Table;
using Newtonsoft.Json;

namespace DtoParcer
{
    public class Generator
    {
        private readonly RouterGeneration _routerGeneration;
        private CollectionOfClasses _collectionOfClasses;
        private TableTypes _tableTypes;

        public Generator(string pathToJson, string pathToGeneratedClasses)
        {
            _routerGeneration = new RouterGeneration(pathToJson, pathToGeneratedClasses);
            _tableTypes = new TableTypes();
        }

        public void GenerateClasses()
        {
            _collectionOfClasses =
                JsonConvert.DeserializeObject<CollectionOfClasses>(File.ReadAllText(_routerGeneration.PathToJson));

            ParceCsFiles();
        }

        private void ParceCsFiles()
        {
            
        }
    }
}
