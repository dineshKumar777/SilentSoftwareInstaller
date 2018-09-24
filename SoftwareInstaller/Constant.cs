using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareInstaller
{
    public class Constant
    {
        public const string CHECKING_IN_REGISTRY = "-->Checking in registry is completed if app found ignore if not check it manually";
        public const string UAC_REGISTRY_LOCATION = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
        public const string ADD_SPECIFIC_COMMAND = " in custom list. Please add specific commands or install it manually...";
        public const string APP_INSTALLED_SUCCESSFULLY = "-->Application installed successfully with exit code : ";
        public const string INSTALLATION_COMPLETE = "-->Installation Complete, Will start to check in registry";
        public const string APP_REGISTRY_LOCATION = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        public const string PROBLEM_WHEN_INSTALLING = "-->There was a problem installing the application!";
        public const string ERROR_READING_REGISTRY = "-->Exception occured when reading Registery : ";
        public const string ERROR_WHEN_INSATLLING = "-->Error when installing";
        public const string SKIPPING_INSTALLATION = " is already installed.. skipping installation";
        public const string APP_CANNOT_BE_FOUND = "-->Checked in registry, app cannot be found.";
        public const string NUMBER_OF_APPS_ADDED = "Number of apps added for installation : ";
        public const string APP_FOUND_64_BIT = "-->App found, which is in 64bit registry...";
        public const string APP_FOUND_32_BIT = "-->App found, which is in 32bit registry...";
        public const string NUMBER_OF_APPS_INSTALLED = "Number of apps installed : ";
        public const string DIRECTORY_NOT_FOUND = "Not a valid file or directory";
        public const string APP_PRESENT_IN_REGISTRY = " app present in registry";
        public const string STARTING_TO_INSTALL = "Starting to install the ";
        public const string UAC_REGISTRY_KEY = "ConsentPromptBehaviorAdmin";
        public const string UNABLE_TO_FIND = "-->Unable to find";
        public const string SOFTWARE_SETUP = "SoftwareSetup";
        public const string INSTALLING = "-- > Installing ";
        public const string DISPLAY_NAME = "DisplayName";
        public const string ARROR_FOR_SPACING = "-->";
        public const string STAR_DOT_STAR = "*.*";
        public const string SPACING = "\n";
        public const string MSI = ".msi";
        public const string EXE = ".exe";
        public const string APP = "app";
    }
}
