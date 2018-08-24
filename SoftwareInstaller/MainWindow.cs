using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;

namespace SoftwareInstaller
{
    public partial class MainTab : Form
    {
        int countofAppforInstallation = 0;
        int countofAppInstalled = 0;
        List<string> appNames = new List<string>();
        List<string> silentCode = new List<string>();
        List<string> registryAppNames = new List<string>();
        List<string> appSelected = new List<string>();
        List<string> checkedNodes = new List<string>();
        List<string> installedApps = new List<string>();
        string softwaresFolderPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "SoftwareSetup");
        public MainTab()
        {
            InitializeComponent();
        }

        private void FilePathBtn_Click(object sender, EventArgs e)
        {
            int offset = 0;
            if (Directory.Exists(softwaresFolderPath))
            {
                foreach (var listofFolders in Directory.GetDirectories(softwaresFolderPath))
                {
                    var softwareFolderName = listofFolders.Split('\\').Last();
                    RadioButton radio_btn = new RadioButton { Name = softwareFolderName, Text = softwareFolderName, Width = 120, Height = 30, AutoCheck = true, Location = new Point(50 + (offset), 50) };
                    Controls.Add(radio_btn);
                    offset += 120;
                    radio_btn.CheckedChanged += new EventHandler(RadioButton_Checked);
                }
            }
            else
            {
                MessageBox.Show("Not a valid file or directory");
            }
        }

        private void RadioButton_Checked(object sender, EventArgs e)
        {
            RadioButton radio_btn = (sender as RadioButton);
            if (radio_btn.Checked)
            {
                SelectFiles.Enabled = true;
                string selectedSoftwareFolder = Path.Combine(softwaresFolderPath, radio_btn.Text);
                ListDirectory(MainTreeView, selectedSoftwareFolder);
                if (radio_btn.Checked)
                {
                    MainTreeView.ExpandAll();
                    foreach (TreeNode parentNode in MainTreeView.Nodes)
                    {
                        parentNode.Checked = true;
                        foreach (TreeNode childNodes in parentNode.Nodes)
                        {
                            childNodes.Checked = true;
                        }
                    }
                }
            }
        }

        private void ListDirectory(TreeView MainTreeview, string selectedSoftwareFolder)
        {
            MainTreeview.Nodes.Clear();
            MainTreeview.Nodes.Add(CreateDirectoryNode(new DirectoryInfo(selectedSoftwareFolder).Name, selectedSoftwareFolder));
        }

        private static TreeNode CreateDirectoryNode(string rootFolderName, string folderPath)
        {
            return new TreeNode(rootFolderName, GetChildNodes(folderPath));
        }

        private static TreeNode[] GetChildNodes(string folderPath)
        {
            var EXEandMSI_Files = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".exe") || s.EndsWith(".msi"));
            var childNodes = new TreeNode[EXEandMSI_Files.Count()];
            int count_EXEandMSI_Files = EXEandMSI_Files.Count();
            for (int index = 0; index < count_EXEandMSI_Files; index++)
            {
                childNodes[index] = new TreeNode(Path.GetFileName(EXEandMSI_Files.ElementAt(index)));
            }
            return childNodes;
        }

        private void MainTreeView_NodeMouseClick_1(object sender, TreeNodeMouseClickEventArgs e)
        {
            foreach (TreeNode node in e.Node.Nodes)
            {
                node.Checked = !node.Checked;
            }
        }

        private void MainTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Checked)
            {
                checkedNodes.Add(e.Node.FullPath.ToString());
            }
            else
            {
                checkedNodes.Remove(e.Node.FullPath.ToString());
            }
        }

        private void SelectFilesBtn_Click(object sender, EventArgs e)
        {
            PrintNodesRecursive(MainTreeView.Nodes[0]);
            Install.Enabled = true;
            SelectedAppList.Items.Clear();
            foreach (var appToList in appSelected)
            {
                SelectedAppList.Items.Add(appToList);
            }
        }

        public void PrintNodesRecursive(TreeNode parentNode)
        {
            foreach (TreeNode subNode in parentNode.Nodes)
            {
                if (!subNode.Checked)
                {
                    appSelected.Remove(subNode.Text);
                }
                if (!appSelected.Any(check => check.Contains(subNode.Text)))
                {
                    if (subNode.Checked)
                    {
                        appSelected.Add(subNode.Text);
                    }
                }
            }
        }

        private void InstallBtn_Click(object sender, EventArgs e)
        {
            foreach (var selectedAppNames in appSelected)
            {
                var fileInfo = Directory.GetFiles(softwaresFolderPath, selectedAppNames, SearchOption.AllDirectories);
                foreach (var file in fileInfo)
                {
                    string filePath = new DirectoryInfo(file).FullName;
                    SilentInstall(selectedAppNames, filePath);
                }
            }
            countofAppforInstallation = appSelected.Count();
            MessageBox.Show("Number of apps added for installation : " + countofAppforInstallation + "\n" + "Number of apps installed : " + countofAppInstalled);
            countofAppInstalled = 0;
        }

        public void ValueofAppConfig()
        {
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key.StartsWith("app"))
                {
                    string[] appInfo = ConfigurationManager.AppSettings[key].Split(';');
                    appNames.Add(appInfo[0]);
                    silentCode.Add(appInfo[1]);
                    registryAppNames.Add(appInfo[2]);
                }
            }
        }

        public void SilentInstall(string selectedAppNames, string filePath)
        {
            ValueofAppConfig();
            try
            {
                int appNameIndex = -1;
                for (int i = 0; i < appNames.Count; i++)
                {
                    if (selectedAppNames.ToUpper().Contains(appNames[i].ToUpper()))
                    {
                        appNameIndex = i;
                        break;
                    }
                }
                LogList.Items.Add("Starting to install the " + selectedAppNames);
                if (selectedAppNames.ToUpper().Contains(appNames[appNameIndex].ToUpper()))
                {
                    if (!IsAppInstalled(registryAppNames[appNameIndex]))
                    {
                        LogList.Items.Add("-->Installing " + selectedAppNames);
                        LogList.TopIndex = LogList.Items.Count - 1;
                        RunInstallMSI(filePath, silentCode[appNameIndex]);
                        LogList.Items.Add("-->Installation Complete, Will start to check in registry");
                        LogList.TopIndex = LogList.Items.Count - 1;
                        countofAppInstalled++;
                        IsAppInstalled(registryAppNames[appNameIndex]);
                        LogList.Items.Add("-->Checking in registry is completed if app found ignore if not check it manually");
                        LogList.TopIndex = LogList.Items.Count - 1;
                    }
                    else
                    {
                        LogList.Items.Add("-->" + registryAppNames[appNameIndex] + " is already installed.. skipping installation");
                    }
                }
                else
                {
                    LogList.Items.Add("-->Unable to find" + selectedAppNames + " in custom list. Please add specific commands or install it manually...");
                }
            }
            catch (Exception ex)
            {
                LogList.Items.Add("-->Error when installing" + selectedAppNames);
                LogList.Items.Add(ex.StackTrace);
                throw;
            }
        }

        public void RunInstallMSI(string filePath, string silentInstallCode)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(filePath, silentInstallCode);
                Process process = Process.Start(startInfo);
                process.WaitForExit();
                LogList.Items.Add("-->Application installed successfully with exit code : " + process.ExitCode);
            }
            catch (Exception)
            {
                LogList.Items.Add("-->There was a problem installing the application!");
                throw;
            }
        }

        public bool IsAppInstalled(string registryAppNames)
        {
            if (CheckInstallationState(registryAppNames, RegistryView.Registry64))
            {
                LogList.Items.Add("-->App found, which is in 64bit registry...");
                LogList.TopIndex = LogList.Items.Count - 1;
                return true;
            }
            else if (CheckInstallationState(registryAppNames, RegistryView.Registry32))
            {
                LogList.Items.Add("-->App found, which is in 32bit registry...");
                LogList.TopIndex = LogList.Items.Count - 1;
                return true;
            }
            else
            {
                LogList.Items.Add("-->Checked in registry, app cannot be found.");
                LogList.TopIndex = LogList.Items.Count - 1;
                return false;
            }
        }

        public bool CheckInstallationState(string registryAppNames, RegistryView regBit)
        {
            string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, regBit))
            {
                using (RegistryKey rk = baseKey.OpenSubKey(registryKey))
                {
                    foreach (string skName in rk.GetSubKeyNames())
                    {
                        using (RegistryKey sk = rk.OpenSubKey(skName))
                        {
                            try
                            {
                                var appSelectedName = sk.GetValue("DisplayName");
                                if (appSelectedName != null)
                                {
                                    installedApps.Add(appSelectedName.ToString().ToUpper());
                                }
                            }
                            catch (Exception ex)
                            {
                                LogList.Items.Add("-->Exception occured when reading Registery : " + ex.StackTrace);
                                LogList.TopIndex = LogList.Items.Count - 1;
                                throw;
                            }
                        }
                    }
                }
            }

            if (installedApps.Any(check => check.Contains(registryAppNames.ToUpper())))
            {
                LogList.Items.Add("-->" + registryAppNames + " app present in registry");
                LogList.TopIndex = LogList.Items.Count - 1;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}