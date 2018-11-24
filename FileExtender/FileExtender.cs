using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtender
{
    public class FileExtender
    {
        private ExtensionSettings extensionSettings;
        public FileExtender(ExtensionSettings extensionSettings)
        {
            this.extensionSettings = extensionSettings;
        }

        public void Extend()
        {
            ValidateSettings(extensionSettings);

            ExtendFilesInDirectory(extensionSettings.DirectoryLocation);

            if (extensionSettings.SubFoldersIncluded)
            {
                TraverseSubDirectories(extensionSettings.DirectoryLocation);
            }
        }

        private void ValidateSettings(ExtensionSettings settings)
        {
            if (string.IsNullOrEmpty(settings.DirectoryLocation))
            {
                throw new ArgumentException("The directory location must be provided.");
            }
            if (!Directory.Exists(settings.DirectoryLocation))
            {
                throw new ArgumentException(string.Format("The directory location {0} does not exist.", settings.DirectoryLocation));
            }
        }

        private void TraverseSubDirectories(string folder)
        {
            string[] subDirectories = Directory.GetDirectories(extensionSettings.DirectoryLocation);
            foreach (string subDirectory in subDirectories)
            {
                string path = Path.Combine(extensionSettings.DirectoryLocation, subDirectory);
                ExtendFilesInDirectory(path);
                TraverseSubDirectories(path);
            }
        }

        private void ExtendFilesInDirectory(string folderPath)
        {
            string[] files = Directory.GetFiles(extensionSettings.DirectoryLocation, extensionSettings.SearchPattern);
            foreach(string file in files)
            {
                string newName = GetNewName(file, extensionSettings.Extension, extensionSettings.ExtensionAction);
                File.Move(file, newName);
            }
        }

        private string GetNewName(string currentFileName, string extension, ExtensionAction extensionAction)
        {
            switch (extensionAction)
            {
                case ExtensionAction.Add:
                    return string.Concat(currentFileName, extension);
                case ExtensionAction.Replace:
                    return string.Concat(GetFileNameWithoutExtension(currentFileName), extension);
                default:
                    return "";
            }
        }

        private string GetFileNameWithoutExtension(string fileName)
        {
            int indexOfLastFileExtension = fileName.LastIndexOf('.');
            if(indexOfLastFileExtension > 0)
            {
                return fileName.Substring(0, indexOfLastFileExtension);
            }
            return fileName;
        }
    }
}
