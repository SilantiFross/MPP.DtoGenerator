using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DtoTest.Writer
{
    internal class Writer
    {
        public void WriteClassesInCsFile(List<StringBuilder> generatedClass, string pathToGeneratedClasses)
        {
            foreach (var stringBuilder in generatedClass)
            {
                var className = Regex.Match(stringBuilder.ToString(), @"(?<=class )(\w+)");
                var file = new StreamWriter(pathToGeneratedClasses + className + ".cs");
                file.WriteLine(stringBuilder);
                file.Close();
            }
        }
    }
}
