using System;
using DtoParcer;

namespace DtoTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new Generator("\\descriptions.json", ".\\");
            generator.GenerateClasses();
            Console.ReadLine();
        }
    }
}
