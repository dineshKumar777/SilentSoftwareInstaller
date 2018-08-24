using System;
using System.Windows.Forms;

namespace SoftwareInstaller
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.Run(new MainTab());
        }
    }
}