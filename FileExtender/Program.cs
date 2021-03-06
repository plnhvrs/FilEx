﻿using System;
using System.IO;

namespace FileExtender
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 3)
            {
                Console.WriteLine("To extend your files, you need to supply at least three arguments in the following order: 1) folder location 2) the desired extension 3) a search pattern (use * for all files).");
                return;
            }

            string folderLocation = args[0];
            string extension = args[1];
            string searchPattern = args[2];

            ExtensionSettings settings = new ExtensionSettings(folderLocation, extension, searchPattern);

            if (args.Length > 3)
            {
                for(int i = 3; i < args.Length; i++)
                {
                    InterpretOptionalArgument(args[i], settings);
                }
            }
            Console.WriteLine(String.Format("Starting the extension of files using the extension {0} at location: {1}.", settings.Extension, settings.DirectoryLocation));
            
            FileExtender fileExtender = new FileExtender(settings);
            try
            {
                fileExtender.Extend();
            }
            catch (ArgumentException e) {
                Console.WriteLine(String.Format("An error occurred while extending your files. The error reads: {0}", e.Message));
            }
            catch(IOException e)
            {
                Console.WriteLine(String.Format("An IO error occurred while extending your files. The error reads: {0}", e.Message));
            }
            catch(Exception e)
            {
                Console.WriteLine(String.Format("An unexpected error occurred while extending your files. The error reads: {0}", e.Message)); 
            }
            Console.WriteLine("Extending files has succesfully finished.");
        }

        private static void InterpretOptionalArgument(string arg, ExtensionSettings settings)
        {
            switch(arg)
            {
                case "-r":
                    settings.ExtensionAction = ExtensionAction.Replace;
                    Console.WriteLine("The extension will be replaced instead of concatenated.");
                    break;
                case "-s":
                    settings.SubFoldersIncluded = true;
                    Console.WriteLine("Sub directories will be included.");
                    break;
                default:
                    Console.WriteLine(string.Format("The console argument {0} passed is unrecognized.", arg));
                    break;
            }
        }
    }
}
