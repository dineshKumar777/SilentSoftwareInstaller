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
        int CountofAppforInstallation = 0;
        int CountofAppInstalled = 0;
        List<string> appNames = new List<string>();
        List<string> silentCode = new List<string>();
        List<string> registryAppNames = new List<string>();
        List<string> appSelected = new List<string>().Distinct().ToList();
        List<string> checkedNodes = new List<string>();
        List<string> installedApps = new List<string>().Distinct().ToList();
        public TreeNode selectedNode;
        public MainTab()
        {
            InitializeComponent();
            TreeView mainTreeView = new TreeView();
        }

        private void filePathButton(object sender, EventArgs e)
        {
            int i = 0;
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string setupPath = path + @"\SoftwareSetup";
            RadioButton box = null;
            if (Directory.Exists(setupPath))
            {
                var directories = Directory.GetDirectories(setupPath);
                foreach (var n in directories)
                {
                    var setupPath2 = n.Split('\\').Last();
                    box = new RadioButton { Name = setupPath2, Text = setupPath2, Width = 120, Height = 30, AutoCheck = true, Location = new Point(50 + (i), 50) };
                    this.Controls.Add(box);
                    i += 120;
                    box.CheckedChanged += new EventHandler(RadioButton_Checked);
                }
            }
            else
            {
                MessageBox.Show("Not a valid file or directory");
            }
        }

        public void updateButton1()
        {
            SelectFiles.Enabled = true;
        }
        public void updateButton3()
        {
            Install.Enabled = true;
        }

        private void RadioButton_Checked(object sender, EventArgs e)
        {
            RadioButton box = (sender as RadioButton);
            if (box.Checked)
            {
                updateButton1();
                string setupPath3 = box.Text;
                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                string setupPath = path + @"\SoftwareSetup\" + setupPath3;
                ListDirectory(mainTreeView, setupPath);
                if (box.Checked == true)
                {
                    mainTreeView.ExpandAll();
                    foreach (TreeNode nodes in mainTreeView.Nodes)
                    {
                        nodes.Checked = true;
                        foreach (TreeNode node in nodes.Nodes)
                        {
                            node.Checked = true;
                        }
                    }
                }
            }
        }

        private void ListDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();
            treeView.Nodes.Add(CreateDirectoryNode(new DirectoryInfo(path).Name, path));
        }

        private static TreeNode CreateDirectoryNode(string rootFolderName, string folderPath)
        {
            return new TreeNode(rootFolderName, GetChildNodes(folderPath));
        }

        private static TreeNode[] GetChildNodes(string folderPath)
        {
            var files = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".exe") || s.EndsWith(".msi"));
            var childs = new TreeNode[files.Count()];
            var i = 0;
            foreach (var file in files)
            {
                childs[i] = new TreeNode(Path.GetFileName(file));
                i++;
            }
            return childs;
        }

        private void mainTreeView_NodeMouseClick_1(object sender, TreeNodeMouseClickEventArgs e)
        {
            foreach (TreeNode node in e.Node.Nodes)
            {
                node.Checked = !node.Checked;
            }
        }

        private void mainTreeView_AfterSelect(object sender, TreeViewEventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            TreeNode oMainNode = mainTreeView.Nodes[0];
            PrintNodesRecursive(oMainNode);
            updateButton3();
            SelectedAppList.Items.Clear();
            foreach (var n in appSelected)
            {
                SelectedAppList.Items.Add(n);
            }
        }

        public void PrintNodesRecursive(TreeNode oParentNode)
        {
            foreach (TreeNode oSubNode in oParentNode.Nodes)
            {
                if (oSubNode.Checked == false)
                {
                    appSelected.Remove(oSubNode.Text);
                }
                if (!Contain(oSubNode.Text))
                {
                    if (oSubNode.Checked == true)
                    {
                        appSelected.Add(oSubNode.Text);
                    }
                }
            }
        }

        public bool Contain(string textValue)
        {
            bool contained = appSelected.Any(l => l.Contains(textValue));
            if (contained)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (var SelectedAppNames in appSelected)
            {
                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                string setupPath1 = path + @"\SoftwareSetup";
                var files = Directory.GetFiles(setupPath1, SelectedAppNames, SearchOption.AllDirectories);
                foreach (var fileNames in files)
                {
                    DirectoryInfo setupPath = new DirectoryInfo(fileNames);
                    string one = setupPath.FullName;
                    silentInstall(SelectedAppNames, one); 
                }
            }
            CountofAppforInstallation = appSelected.Count;
            MessageBox.Show("Number of apps added for installation : " + CountofAppforInstallation + "\n" + "Number of apps installed : " + CountofAppInstalled);
            CountofAppInstalled = 0;
        }
        
        public void appCon()
        {
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key.StartsWith("app"))
                {
                    string value = ConfigurationManager.AppSettings[key];
                    string[] one = value.Split(';');
                    appNames.Add(one[0]);
                    silentCode.Add(one[1]);
                    registryAppNames.Add(one[2]);
                }
            }
        }

        public void silentInstall(string appname, string setupPath)
        {
            appCon();
            try
            {
                string appname1 = appname.ToUpper();
                int appNameIndex = -1;
                for (int i = 0; i < appNames.Count; i++)
                {
                    if (appname1.Contains(appNames[i].ToUpper()))
                    {
                        appNameIndex = i;
                        break;
                    }
                }
                LogList.Items.Add("Starting to install the " + appname);
                if (appname1.Contains(appNames[appNameIndex].ToUpper()))
                {
                    if (!isAppInstalled(registryAppNames[appNameIndex]))
                    {
                        LogList.Items.Add("-->Installing " + appname);
                        LogList.TopIndex = LogList.Items.Count - 1;
                        RunInstallMSI(setupPath, silentCode[appNameIndex]);
                        LogList.Items.Add("-->Installation Complete, Will start to check in registry");
                        LogList.TopIndex = LogList.Items.Count - 1;
                        CountofAppInstalled++;
                        isAppInstalled(registryAppNames[appNameIndex]);
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
                    LogList.Items.Add("-->Unable to find" + appname + " in custom list. Please add specific commands or install it manually...");
                }
            }
            catch (Exception ex)
            {
                LogList.Items.Add("-->Error when installing" + appname);
                LogList.Items.Add(ex.StackTrace);
                throw;
            }
        }

        public void RunInstallMSI(string exePath, string cmd)
        {
            try
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo(exePath, cmd);
                process = Process.Start(startInfo);
                process.WaitForExit();
                LogList.Items.Add("-->Application installed successfully with exit code : " + process.ExitCode);
            }
            catch (Exception)
            {
                LogList.Items.Add("-->There was a problem installing the application!");
                throw;
            }
        }

        public bool isAppInstalled(string registryAppNames)
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
            string appName1 = registryAppNames.ToUpper();
            bool contained = installedApps.Any(l => l.Contains(appName1));
            if (contained)
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