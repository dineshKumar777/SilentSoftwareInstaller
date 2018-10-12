namespace SoftwareInstaller
{
    public class Constant
    {
        #region Extensions
        public const string MSI = ".msi";
        public const string EXE = ".exe";
        #endregion

        #region Spacing
        public const string ARROW_SPACING_FOR_LOGLIST = "--> ";
        public const string STAR_DOT_STAR = "*.*";
        public const string SPACING = "\n";
        #endregion

        #region Location
        public const string UAC_REGISTRY_LOCATION = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
        public const string APP_REGISTRY_LOCATION = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        #endregion

        #region Registry constant
        public const string CHECKING_IN_REGISTRY = ARROW_SPACING_FOR_LOGLIST + "Checking in registry is completed if app found ignore if not check it manually";
        public const string UAC_REGISTRY_KEY = "ConsentPromptBehaviorAdmin";
        public const string ERROR_READING_REGISTRY = ARROW_SPACING_FOR_LOGLIST + "Exception occured when reading Registery : ";
        public const string APP_FOUND_64_BIT = ARROW_SPACING_FOR_LOGLIST + "App found, which is in 64bit registry...";
        public const string APP_FOUND_32_BIT = ARROW_SPACING_FOR_LOGLIST + "App found, which is in 32bit registry...";
        public const string APP_PRESENT_IN_REGISTRY = " app present in registry";
        #endregion

        #region Direct naming
        public const string SOFTWARE_SETUP = "SoftwareSetup";
        public const string INSTALLING = ARROW_SPACING_FOR_LOGLIST + "Installing ";
        public const string DISPLAY_NAME = "DisplayName";
        public const string APP = "app";
        #endregion

        #region Error message
        public const string PROBLEM_WHEN_INSTALLING = ARROW_SPACING_FOR_LOGLIST + "There was a problem installing the application!";
        public const string APP_CANNOT_BE_FOUND = ARROW_SPACING_FOR_LOGLIST + "Checked in registry, app cannot be found.";
        public const string UNABLE_TO_FIND = ARROW_SPACING_FOR_LOGLIST + "Unable to find";
        public const string DIRECTORY_NOT_FOUND = "Not a valid file or directory";
        public const string ERROR_WHEN_INSATLLING = ARROW_SPACING_FOR_LOGLIST + "Error when installing";
        #endregion

        #region Success message
        public const string APP_INSTALLED_SUCCESSFULLY = ARROW_SPACING_FOR_LOGLIST + "Application installed successfully with exit code : ";
        public const string INSTALLATION_COMPLETE = ARROW_SPACING_FOR_LOGLIST + "Installation Complete, Will start to check in registry";
        #endregion

        #region Other message
        public const string NUMBER_OF_APPS_ADDED = "Number of apps added for installation : ";
        public const string SKIPPING_INSTALLATION = " is already installed.. skipping installation";
        public const string ADD_SPECIFIC_COMMAND = " in custom list. Please add specific commands or install it manually...";
        public const string STARTING_TO_INSTALL = "Starting to install the ";
        public const string NUMBER_OF_APPS_INSTALLED = " Number of apps installed : ";
        #endregion
    }
}