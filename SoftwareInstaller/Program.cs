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
            changingUserAccountControl((int)UacRegistryValue.Never_notify_me);
            Application.Run(new MainTab());
            changingUserAccountControl((int)UacRegistryValue.Notify_me_only_when_apps_try_to_make_changes);
        }
        static void changingUserAccountControl(int uacRegistryValue)
        {
            Registry.SetValue(Constant.UAC_REGISTRY_LOCATION, Constant.UAC_REGISTRY_KEY, uacRegistryValue);
        }
    }
    enum UacRegistryValue
    {
        Notify_me_only_when_apps_try_to_make_changes = 5,
        Never_notify_me = 0,
    }
}