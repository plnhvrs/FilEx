﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtender
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderLocation = args[0];
            string extension = args[1];
            string searchPattern = args[2];

            ExtensionSettings settings = new ExtensionSettings(folderLocation, extension, searchPattern);

            if (args.Length > 3)
            {
                for(int i = 3; i < args.Length; i++)
                {
                    InterpretArgument(args[i], settings);
                }
            }
            
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
        }

        private static void InterpretArgument(string arg, ExtensionSettings settings)
        {
            switch(arg)
            {
                case "-r":
                    settings.ExtensionAction = ExtensionAction.Replace;
                    break;
                case "-s":
                    settings.SubFoldersIncluded = true;
                    break;
                default: throw new ArgumentException(string.Format("The console argument {0} passed is unrecognized.", arg));
            }
        }
    }
}