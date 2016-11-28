namespace DtoTest
{
    internal class RouterGenerator
    {
        public string PathToJson { get; }
        public string PathToGeneratedClasses { get; }

        public RouterGenerator(string pathToJson, string pathToGeneratedClasses)
        {
            PathToJson = pathToJson;
            PathToGeneratedClasses = pathToGeneratedClasses;
        }
    }
}