using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using DtoParcer.GenerationUnits;
using DtoParcer.GenerationUnits.Components;
using DtoParcer.Parcer.Table;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace DtoParcer
{
    public class Generator
    {
        private readonly ConfigApp _configApp;
        private readonly TableTypes _tableTypes;
        private ConcurrentQueue<StringBuilder> _generatedCsClasses;

        public Generator(int numberOfMaxTasks, string namespaceClasses)
        {
            _configApp = new ConfigApp(numberOfMaxTasks, namespaceClasses);
            _tableTypes = new TableTypes();
        }

        public ConcurrentQueue<StringBuilder> GenerateClasses(CollectionOfClasses collectionOfClasses)
        {
            _generatedCsClasses = new ConcurrentQueue<StringBuilder>();
            var administratorTasks = new AdministratorTasks(_configApp.NumberOfMaxTasks);

            using (var countdownEvent = new CountdownEvent(collectionOfClasses.ClassDescriptions.Count))
            {
                foreach (var classDescription in collectionOfClasses.ClassDescriptions)
                {
                    administratorTasks.GenerateCsClass(_generatedCsClasses, classDescription, countdownEvent, ParceCsFile);
                }
                administratorTasks.WaitAllTasks(countdownEvent);
            }
            return _generatedCsClasses;
        }

        private StringBuilder ParceCsFile(Class classDescription)
        {
            var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName(_configApp.NamespaceClasses));
            var classDeclaration = AddedClassDeclaration(classDescription);

            classDeclaration = classDescription.Properties.Aggregate(classDeclaration, (current, property) => AddedAutoProperty(property, current));

            namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration);
            var compilationUnit = SyntaxFactory.CompilationUnit();
            compilationUnit = compilationUnit.AddMembers(namespaceDeclaration);

            var formattedNode = AddedFormattedNode(compilationUnit);
            var stringBuilder = new StringBuilder();

            using (var writer = new StringWriter(stringBuilder))
            {
                formattedNode.WriteTo(writer);
            }

            return stringBuilder;
        }

        private ClassDeclarationSyntax AddedAutoProperty(Property property, ClassDeclarationSyntax classDeclaration)
        {
            var parcerType = new ParcerType(property.Type, property.Format);

            var propertyDeclaration =
                SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(_tableTypes.GetNetType(parcerType)), property.Name)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithModifiers(
                        SyntaxFactory.TokenList(
                            SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                    .WithAccessorList(
                        SyntaxFactory.AccessorList(
                                SyntaxFactory.List(
                                    new[]
                                    {
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
                                                SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                                    }))
                            .WithOpenBraceToken(
                                SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
                            .WithCloseBraceToken(
                                SyntaxFactory.Token(SyntaxKind.CloseBraceToken)));

            classDeclaration = classDeclaration.AddMembers(propertyDeclaration);
            return classDeclaration;
        }

        private SyntaxNode AddedFormattedNode(SyntaxNode compilationUnit)
        {
            var workspace = new AdhocWorkspace();
            var options = workspace.Options;
            options = options.WithChangedOption(CSharpFormattingOptions.SpaceBeforeOpenSquareBracket, false);

            var formattedNode = Formatter.Format(compilationUnit, workspace, options);
            return formattedNode;
        }

        private ClassDeclarationSyntax AddedClassDeclaration(Class classDescription)
        {
            var classDeclaration = SyntaxFactory.ClassDeclaration(classDescription.ClassName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.SealedKeyword));
            return classDeclaration;
        }
    }
}
