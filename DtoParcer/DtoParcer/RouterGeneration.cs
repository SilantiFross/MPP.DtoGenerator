namespace DtoParcer
{
    internal class RouterGeneration
    {
        public string PathToJson { get; }
        public string PathToGeneratedClasses { get; }

        public RouterGeneration(string pathToJson, string pathToGeneratedClasses)
        {
            PathToJson = pathToJson;
            PathToGeneratedClasses = pathToGeneratedClasses;
        }
    }
}
