using System;
using System.IO;
using System.Text;
using DtoParcer.GenerationUnits;
using DtoParcer.GenerationUnits.Components;
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
        private readonly TableTypes _tableTypes;
        private CollectionOfClasses _collectionOfClasses;

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

            foreach (var classDescription in _collectionOfClasses.ClassDescriptions)
            {
                ParceCsFile(classDescription);
            }
        }

        private void ParceCsFile(Class classDescription)
        {
            var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName(_configApp.NamespaceClasses));

            var classDeclaration = SyntaxFactory.ClassDeclaration(classDescription.ClassName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.SealedKeyword));

            foreach (var property in classDescription.Properties)
            {
                var jsonType = new JsonType(property.Type, property.Format);
                var propertyDeclaration =
                    SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(_tableTypes.GetNetType(jsonType)), property.Name)
                        .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                        .WithModifiers(
                    SyntaxFactory.TokenList(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                .WithAccessorList(
                    SyntaxFactory.AccessorList(
                        SyntaxFactory.List(
                            new []{
                                SyntaxFactory.AccessorDeclaration(
                                    SyntaxKind.GetAccessorDeclaration)
                                .WithKeyword(
                                    SyntaxFactory.Token(SyntaxKind.GetKeyword))
                                .WithSemicolonToken(
                                    SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                                SyntaxFactory.AccessorDeclaration(
                                    SyntaxKind.SetAccessorDeclaration)
                                .WithKeyword(
                                    SyntaxFactory.Token(SyntaxKind.SetKeyword))
                                .WithSemicolonToken(
                                    SyntaxFactory.Token(SyntaxKind.SemicolonToken))}))
                    .WithOpenBraceToken(
                        SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
                    .WithCloseBraceToken(
                        SyntaxFactory.Token(SyntaxKind.CloseBraceToken)));

                classDeclaration = classDeclaration.AddMembers(propertyDeclaration);
            }

            namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration);
            var compilationUnit = SyntaxFactory.CompilationUnit();
            compilationUnit = compilationUnit.AddMembers(namespaceDeclaration);

            var workspace = new AdhocWorkspace();
            var options = workspace.Options;
            options = options.WithChangedOption(CSharpFormattingOptions.SpaceBeforeOpenSquareBracket, false);

            var formattedNode = Formatter.Format(compilationUnit, workspace, options);
            var stringBuilder = new StringBuilder();

            using (var writer = new StringWriter(stringBuilder))
            {
                formattedNode.WriteTo(writer);
            }

            var file = new StreamWriter(_routerGeneration.PathToGeneratedClasses + classDescription.ClassName + ".cs");
            file.WriteLine(stringBuilder.ToString());
            file.Close();
        }
    }
}