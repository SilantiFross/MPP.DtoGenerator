using System;
using DtoParcer;

namespace DtoTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var pathToJson = args[0];
            var pathToCsFiles = args[1];
            var generator = new Generator(pathToJson, pathToCsFiles);

            generator.GenerateClasses();
            Console.ReadLine();
        }
    }
}