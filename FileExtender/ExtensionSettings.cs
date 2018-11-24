namespace FileExtender
{
    public class ExtensionSettings
    {
        public ExtensionSettings(string directoryLocation, string extension = ".txt", string searchPattern = "*", bool subFoldersIncluded = false, ExtensionAction extensionAction = ExtensionAction.Add)
        {
            DirectoryLocation = directoryLocation;
            Extension = extension;
            SearchPattern = searchPattern;
            SubFoldersIncluded = subFoldersIncluded;
            ExtensionAction = extensionAction;
        }

        public string  DirectoryLocation { get; private set; }

        public string Extension { get; private set; }

        public bool SubFoldersIncluded { get; set; }

        public ExtensionAction ExtensionAction { get; set; }

        public string SearchPattern { get; set; }
    }
}
