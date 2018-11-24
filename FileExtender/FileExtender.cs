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

        private void TraverseSubDirectories(string directory)
        {
            string[] subDirectories = Directory.GetDirectories(directory);
            foreach (string subDirectory in subDirectories)
            {
                string path = subDirectory.Replace('\\', '/').Replace("//", "/");
                ExtendFilesInDirectory(path);
                TraverseSubDirectories(path);
            }
        }

        private void ExtendFilesInDirectory(string directory)
        {
            string[] files = Directory.GetFiles(directory, extensionSettings.SearchPattern);
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
                case ExtensionAction.Replace:
                    return string.Concat(GetFileNameWithoutExtension(currentFileName), extension);
                case ExtensionAction.Add:
                default:
                    return string.Concat(currentFileName, extension);
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
