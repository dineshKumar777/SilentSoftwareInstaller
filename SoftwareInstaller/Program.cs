using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace SoftwareInstaller
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            changingUserAccountControl((int)UacRegistryValue.Elevate_without_prompting);
            Application.Run(new MainTab());
            changingUserAccountControl((int)UacRegistryValue.Prompt_for_consent_for_non_Windows_binaries);
        }

        static void changingUserAccountControl(int uacRegistryValue)
        {
            Registry.SetValue(Constant.UAC_REGISTRY_LOCATION, Constant.UAC_REGISTRY_KEY, uacRegistryValue);
        }
    }

    enum UacRegistryValue
    {
        Elevate_without_prompting = 0,
        Prompt_for_credentials_on_the_secure_desktop = 1,
        Prompt_for_consent_on_the_secure_desktop = 2,
        Prompt_for_credentials = 3,
        Prompt_for_consent = 4,
        Prompt_for_consent_for_non_Windows_binaries = 5
    }
}