using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using log4net;
using System.Reflection;
using log4net.Config;
using AutoUpdaterDotNET;

namespace SoftwareInstaller
{
    public partial class MainTab : Form
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        int countofAppInstalled;
        List<string> appNames = new List<string>();
        List<string> silentCode = new List<string>();
        List<string> registryAppNames = new List<string>();
        List<string> appSelected = new List<string>();
        List<string> checkedNodes = new List<string>();
        List<string> installedApps = new List<string>();
        string softwaresFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), Constant.SOFTWARE_SETUP);

        public MainTab()
        {
            InitializeComponent();
            Load += new EventHandler(MainTab_Load);
        }

        // Autoupdater configuration & initialization
        private void MainTab_Load(object sender, EventArgs e)
        {
            //string versionInfoLink = "https://cdn.jsdelivr.net/gh/dineshKumar777/SilentSoftwareInstaller@latest/SoftwareInstaller/VersionInfo.xml";
            string versionInfoLink = "https://raw.githack.com/dineshKumar777/SilentSoftwareInstaller/master/SoftwareInstaller/VersionInfo.xml";

            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.ShowRemindLaterButton = false;
            AutoUpdater.OpenDownloadPage = false;
            AutoUpdater.DownloadPath = Environment.CurrentDirectory;

            AutoUpdater.Start(versionInfoLink);
        }


        private void FilePathBtn_Click(object sender, EventArgs e)
        {
            XmlConfigurator.Configure();
            int offset = 0;
            if (Directory.Exists(softwaresFolderPath))
            {
                foreach (var listofFolders in Directory.GetDirectories(softwaresFolderPath))
                {
                    var softwareFolderName = listofFolders.Split('\\').Last();
                    _log.Info(Constant.ARROW_SPACING_FOR_LOGLIST + softwareFolderName);
                    RadioButton radio_btn = new RadioButton { Name = softwareFolderName, Text = softwareFolderName, Width = 120, Height = 30, AutoCheck = true, Location = new Point(50 + (offset), 50) };
                    Controls.Add(radio_btn);
                    offset += 120;
                    radio_btn.CheckedChanged += new EventHandler(RadioButton_Checked);
                }
            }
            else
            {
                _log.Error(Constant.DIRECTORY_NOT_FOUND);
                MessageBox.Show(Constant.DIRECTORY_NOT_FOUND);
            }
        }

        private void RadioButton_Checked(object sender, EventArgs e)
        {
            RadioButton radio_btn = (sender as RadioButton);
            if (radio_btn.Checked)
            {
                SelectFiles.Enabled = true;
                ListDirectory(MainTreeView, Path.Combine(softwaresFolderPath, radio_btn.Text));
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
            var EXEandMSI_Files = Directory.EnumerateFiles(folderPath, Constant.STAR_DOT_STAR, SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(Constant.EXE) || s.EndsWith(Constant.MSI));
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
            XmlConfigurator.Configure();
            foreach (var selectedAppNames in appSelected)
            {
                foreach (var file in Directory.GetFiles(softwaresFolderPath, selectedAppNames, SearchOption.AllDirectories))
                {
                    SilentInstall(selectedAppNames, new DirectoryInfo(file).FullName);
                }
            }
            MessageBox.Show(Constant.NUMBER_OF_APPS_ADDED + appSelected.Count + Constant.SPACING + Constant.NUMBER_OF_APPS_INSTALLED + countofAppInstalled);
            _log.Info(Constant.NUMBER_OF_APPS_ADDED + appSelected.Count + Constant.NUMBER_OF_APPS_INSTALLED + countofAppInstalled + Constant.SPACING);
            countofAppInstalled = 0;
        }

        public void ValueofAppConfig()
        {
            XmlConfigurator.Configure();
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key.StartsWith(Constant.APP))
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
            XmlConfigurator.Configure();
            ValueofAppConfig();
            try
            {
                int appNameIndex = -1;
                for (int i = 0; i < appNames.Count; i++)
                {
                    if (selectedAppNames.ToUpper().StartsWith(appNames[i].ToUpper()))
                    {
                        appNameIndex = i;
                        break;
                    }
                }
                LogList.Items.Add(Constant.STARTING_TO_INSTALL + selectedAppNames);
                _log.Info(Constant.STARTING_TO_INSTALL + selectedAppNames);
                if (selectedAppNames.ToUpper().Contains(appNames[appNameIndex].ToUpper()))
                {
                    if (!IsAppInstalled(registryAppNames[appNameIndex]))
                    {
                        //LogList.Items.Add(Constant.INSTALLING + selectedAppNames);
                        _log.Info(Constant.INSTALLING + selectedAppNames);
                        LogList.TopIndex = LogList.Items.Count - 1;
                        RunInstallMSI(filePath, silentCode[appNameIndex]);
                        LogList.Items.Add(Constant.INSTALLATION_COMPLETE);
                        _log.Info(Constant.INSTALLATION_COMPLETE);
                        LogList.TopIndex = LogList.Items.Count - 1;
                        countofAppInstalled++;
                        IsAppInstalled(registryAppNames[appNameIndex]);
                        LogList.Items.Add(Constant.CHECKING_IN_REGISTRY);
                        _log.Info(Constant.CHECKING_IN_REGISTRY);
                        LogList.TopIndex = LogList.Items.Count - 1;
                    }
                    else
                    {
                        LogList.Items.Add(Constant.ARROW_SPACING_FOR_LOGLIST + registryAppNames[appNameIndex] + Constant.SKIPPING_INSTALLATION);
                        _log.Info(Constant.ARROW_SPACING_FOR_LOGLIST + registryAppNames[appNameIndex] + Constant.SKIPPING_INSTALLATION);
                    }
                }
                else
                {
                    LogList.Items.Add(Constant.UNABLE_TO_FIND + selectedAppNames + Constant.ADD_SPECIFIC_COMMAND);
                    _log.Error(Constant.UNABLE_TO_FIND + selectedAppNames + Constant.ADD_SPECIFIC_COMMAND);
                }
            }
            catch (Exception ex)
            {
                LogList.Items.Add(Constant.ERROR_WHEN_INSATLLING + selectedAppNames);
                //LogList.Items.Add(ex.StackTrace);
                _log.Error(Constant.ERROR_WHEN_INSATLLING + selectedAppNames);
                _log.Error(ex.StackTrace);
                throw;
            }
        }

        public void RunInstallMSI(string filePath, string silentInstallCode)
        {
            XmlConfigurator.Configure();

            if (silentInstallCode.Contains("SECURITYMODE=SQL"))
            {
                silentInstallCode += " /SQLSYSADMINACCOUNTS=" + Environment.UserDomainName + "\\" + Environment.UserName;
            }

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(filePath, silentInstallCode);
                Process process = Process.Start(startInfo);
                process.WaitForExit();
                //LogList.Items.Add(Constant.APP_INSTALLED_SUCCESSFULLY + process.ExitCode);
                _log.Info(Constant.APP_INSTALLED_SUCCESSFULLY + process.ExitCode);
            }
            catch (Exception)
            {
                LogList.Items.Add(Constant.PROBLEM_WHEN_INSTALLING);
                _log.Error(Constant.PROBLEM_WHEN_INSTALLING);
                throw;
            }
        }

        public bool IsAppInstalled(string registryAppNames)
        {
            XmlConfigurator.Configure();
            if (CheckInstallationState(registryAppNames, RegistryView.Registry64))
            {
                //LogList.Items.Add(Constant.APP_FOUND_64_BIT);
                _log.Info(Constant.APP_FOUND_64_BIT);
                LogList.TopIndex = LogList.Items.Count - 1;
                return true;
            }
            else if (CheckInstallationState(registryAppNames, RegistryView.Registry32))
            {
                //LogList.Items.Add(Constant.APP_FOUND_32_BIT);
                _log.Info(Constant.APP_FOUND_32_BIT);
                LogList.TopIndex = LogList.Items.Count - 1;
                return true;
            }
            else
            {
                //LogList.Items.Add(Constant.APP_CANNOT_BE_FOUND);
                _log.Info(Constant.APP_CANNOT_BE_FOUND);
                LogList.TopIndex = LogList.Items.Count - 1;
                return false;
            }
        }

        public bool CheckInstallationState(string registryAppNames, RegistryView regBit)
        {
            XmlConfigurator.Configure();
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, regBit))
            {
                using (RegistryKey rk = baseKey.OpenSubKey(Constant.APP_REGISTRY_LOCATION))
                {
                    foreach (string skName in rk.GetSubKeyNames())
                    {
                        using (RegistryKey sk = rk.OpenSubKey(skName))
                        {
                            try
                            {
                                var appSelectedName = sk.GetValue(Constant.DISPLAY_NAME);
                                if (appSelectedName != null)
                                {
                                    installedApps.Add(appSelectedName.ToString().ToUpper());
                                    //_log.Info(appSelectedName.ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                LogList.Items.Add(Constant.ERROR_READING_REGISTRY + ex.StackTrace);
                                _log.Error(ex.StackTrace);
                                LogList.TopIndex = LogList.Items.Count - 1;
                                throw;
                            }
                        }
                    }
                }
            }

            if (installedApps.Any(check => check.Contains(registryAppNames.ToUpper())))
            {
                LogList.Items.Add(Constant.ARROW_SPACING_FOR_LOGLIST + registryAppNames + Constant.APP_PRESENT_IN_REGISTRY);
                _log.Info(Constant.ARROW_SPACING_FOR_LOGLIST + registryAppNames + Constant.APP_PRESENT_IN_REGISTRY);
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