using System;
using System.IO;

namespace PDF_Converter.UI
{
    public class UI
    {
        public string directoryPath;
        public string[] GetFiles()
        {

            Console.Write("Enter Directory Path: ");
            directoryPath = Console.ReadLine();

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Directory does not exist");
                throw new Exception("Directory does not exist");
            }
            Console.Write(directoryPath);
            string[] files = Directory.GetFiles(directoryPath, "*.pdf");
            return files;
        }

        public string GetDirectoryPath()
        {
            return directoryPath;
        }
    }

}