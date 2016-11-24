namespace DtoParcer
{
    public class ConfigApp
    {
        public int NumberOfMaxTasks { get; }
        public string NamespaceClasses { get; }

        public ConfigApp(int numberOfMaxTasks, string namespaceClasses)
        {
            NumberOfMaxTasks = numberOfMaxTasks;
            NamespaceClasses = namespaceClasses;
        }
    }
}
