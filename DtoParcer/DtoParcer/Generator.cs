using System;

namespace DtoParcer
{
    internal class Generator
    {
        private RouterGeneration _routerGeneration;

        public Generator(string pathToJson, string pathToGeneratedClasses)
        {
            _routerGeneration = new RouterGeneration(pathToJson, pathToGeneratedClasses);
        }

        public void GenerateClasses()
        {
            throw new NotImplementedException();
        }
    }
}
