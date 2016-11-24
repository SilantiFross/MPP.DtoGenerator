using System;
using System.IO;
using System.Text;
using DtoParcer.GenerationUnits;
using DtoParcer.Parcer.Table;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Formatting;
using Newtonsoft.Json;

namespace DtoParcer
{
    public class Generator
    {
        private readonly RouterGeneration _routerGeneration;
        private readonly ConfigApp _configApp;
        private CollectionOfClasses _collectionOfClasses;
        private TableTypes _tableTypes;

        public Generator(string pathToJson, string pathToGeneratedClasses, int numberOfMaxTasks, string namespaceClasses)
        {
            _routerGeneration = new RouterGeneration(pathToJson, pathToGeneratedClasses);
            _configApp = new ConfigApp(numberOfMaxTasks, namespaceClasses);
            _tableTypes = new TableTypes();
        }

        public void GenerateClasses()
        {
            _collectionOfClasses =
                JsonConvert.DeserializeObject<CollectionOfClasses>(File.ReadAllText(_routerGeneration.PathToJson));
        }

        private void ParceCsFiles()
        {
            var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName(_configApp.NamespaceClasses));

            var classDeclaration = SyntaxFactory.ClassDeclaration(_collectionOfClasses.ClassDescriptions[0].ClassName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.SealedKeyword));

            namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration);

            var compilationUnit = SyntaxFactory.CompilationUnit();

            compilationUnit = compilationUnit.AddMembers(namespaceDeclaration);

            var cw = new AdhocWorkspace();
            var options = cw.Options;
            options = options.WithChangedOption(CSharpFormattingOptions.SpaceBeforeOpenSquareBracket, false);

            var formattedNode = Formatter.Format(compilationUnit, cw, options);
            var stringBuilder = new StringBuilder();

            using (var writer = new StringWriter(stringBuilder))
            {
                formattedNode.WriteTo(writer);
            }

            Console.WriteLine(stringBuilder);
        }
    }
}