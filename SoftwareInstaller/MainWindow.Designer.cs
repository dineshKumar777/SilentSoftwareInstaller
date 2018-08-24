namespace SoftwareInstaller
{
    partial class MainTab
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainTab));
            this.FilePath = new System.Windows.Forms.Button();
            this.MainTreeView = new System.Windows.Forms.TreeView();
            this.SelectFiles = new System.Windows.Forms.Button();
            this.SelectedAppList = new System.Windows.Forms.ListBox();
            this.Install = new System.Windows.Forms.Button();
            this.LogList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // FilePath
            // 
            this.FilePath.BackColor = System.Drawing.Color.GreenYellow;
            this.FilePath.ForeColor = System.Drawing.Color.DarkOrchid;
            this.FilePath.Location = new System.Drawing.Point(12, 12);
            this.FilePath.Name = "FilePath";
            this.FilePath.Size = new System.Drawing.Size(250, 34);
            this.FilePath.TabIndex = 8;
            this.FilePath.Text = "FilePath";
            this.FilePath.UseVisualStyleBackColor = false;
            this.FilePath.Click += new System.EventHandler(this.FilePathBtn_Click);
            // 
            // MainTreeView
            // 
            this.MainTreeView.AllowDrop = true;
            this.MainTreeView.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.MainTreeView.CheckBoxes = true;
            this.MainTreeView.FullRowSelect = true;
            this.MainTreeView.HotTracking = true;
            this.MainTreeView.Location = new System.Drawing.Point(12, 121);
            this.MainTreeView.Name = "MainTreeView";
            this.MainTreeView.ShowPlusMinus = false;
            this.MainTreeView.Size = new System.Drawing.Size(423, 450);
            this.MainTreeView.TabIndex = 9;
            this.MainTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.MainTreeView_AfterSelect);
            this.MainTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.MainTreeView_NodeMouseClick_1);
            // 
            // SelectFiles
            // 
            this.SelectFiles.BackColor = System.Drawing.Color.GreenYellow;
            this.SelectFiles.Enabled = false;
            this.SelectFiles.ForeColor = System.Drawing.Color.DarkOrchid;
            this.SelectFiles.Location = new System.Drawing.Point(12, 577);
            this.SelectFiles.Name = "SelectFiles";
            this.SelectFiles.Size = new System.Drawing.Size(423, 35);
            this.SelectFiles.TabIndex = 10;
            this.SelectFiles.Text = "SelectFiles";
            this.SelectFiles.UseVisualStyleBackColor = false;
            this.SelectFiles.Click += new System.EventHandler(this.SelectFilesBtn_Click);
            // 
            // SelectedAppList
            // 
            this.SelectedAppList.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.SelectedAppList.FormattingEnabled = true;
            this.SelectedAppList.ItemHeight = 19;
            this.SelectedAppList.Location = new System.Drawing.Point(451, 121);
            this.SelectedAppList.Name = "SelectedAppList";
            this.SelectedAppList.ScrollAlwaysVisible = true;
            this.SelectedAppList.Size = new System.Drawing.Size(484, 175);
            this.SelectedAppList.TabIndex = 13;
            // 
            // Install
            // 
            this.Install.BackColor = System.Drawing.Color.GreenYellow;
            this.Install.Enabled = false;
            this.Install.ForeColor = System.Drawing.Color.DarkOrchid;
            this.Install.Location = new System.Drawing.Point(451, 302);
            this.Install.Name = "Install";
            this.Install.Size = new System.Drawing.Size(210, 34);
            this.Install.TabIndex = 14;
            this.Install.Text = "Install";
            this.Install.UseVisualStyleBackColor = false;
            this.Install.Click += new System.EventHandler(this.InstallBtn_Click);
            // 
            // LogList
            // 
            this.LogList.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.LogList.FormattingEnabled = true;
            this.LogList.HorizontalScrollbar = true;
            this.LogList.ItemHeight = 19;
            this.LogList.Location = new System.Drawing.Point(451, 361);
            this.LogList.Name = "LogList";
            this.LogList.ScrollAlwaysVisible = true;
            this.LogList.Size = new System.Drawing.Size(484, 251);
            this.LogList.TabIndex = 15;
            // 
            // MainTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(947, 639);
            this.Controls.Add(this.LogList);
            this.Controls.Add(this.Install);
            this.Controls.Add(this.SelectedAppList);
            this.Controls.Add(this.SelectFiles);
            this.Controls.Add(this.MainTreeView);
            this.Controls.Add(this.FilePath);
            this.Font = new System.Drawing.Font("Calibri", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainTab";
            this.Text = "SoftwareInstaller";
            this.ResumeLayout(false);

        }

        private void mainTreeView_NodeMouseClick(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion
        private System.Windows.Forms.Button FilePath;
        private System.Windows.Forms.TreeView MainTreeView;
        private System.Windows.Forms.Button SelectFiles;
        private System.Windows.Forms.ListBox SelectedAppList;
        private System.Windows.Forms.Button Install;
        private System.Windows.Forms.ListBox LogList;
    }
}

