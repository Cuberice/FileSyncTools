namespace FileSyncTool
{
	partial class SyncFolderBrowser
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SyncFolderBrowser));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.treeSource = new WindowsExplorer.ExplorerTree();
			this.treeDestination = new WindowsExplorer.ExplorerTree();
			this.btnMatchDirectories = new System.Windows.Forms.Button();
			this.btnDone = new System.Windows.Forms.Button();
			this.UpdateGUITimer = new System.Windows.Forms.Timer(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(0, 21);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.treeSource);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.treeDestination);
			this.splitContainer1.Size = new System.Drawing.Size(484, 289);
			this.splitContainer1.SplitterDistance = 235;
			this.splitContainer1.TabIndex = 0;
			// 
			// treeSource
			// 
			this.treeSource.BackColor = System.Drawing.Color.White;
			this.treeSource.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeSource.Location = new System.Drawing.Point(0, 0);
			this.treeSource.Name = "treeSource";
			this.treeSource.SelectedPath = "c:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\Common7\\IDE";
			this.treeSource.ShowAddressbar = true;
			this.treeSource.ShowMyDocuments = true;
			this.treeSource.ShowMyFavorites = true;
			this.treeSource.ShowMyNetwork = true;
			this.treeSource.ShowToolbar = true;
			this.treeSource.Size = new System.Drawing.Size(235, 289);
			this.treeSource.StartWithMyComputer = true;
			this.treeSource.TabIndex = 0;
			this.treeSource.PathChanged += new WindowsExplorer.ExplorerTree.PathChangedEventHandler(this.TreeControls_PathChanged);
			// 
			// treeDestination
			// 
			this.treeDestination.BackColor = System.Drawing.Color.White;
			this.treeDestination.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeDestination.Location = new System.Drawing.Point(0, 0);
			this.treeDestination.Name = "treeDestination";
			this.treeDestination.SelectedPath = "c:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\Common7\\IDE";
			this.treeDestination.ShowAddressbar = true;
			this.treeDestination.ShowMyDocuments = true;
			this.treeDestination.ShowMyFavorites = true;
			this.treeDestination.ShowMyNetwork = true;
			this.treeDestination.ShowToolbar = true;
			this.treeDestination.Size = new System.Drawing.Size(245, 289);
			this.treeDestination.StartWithMyComputer = true;
			this.treeDestination.TabIndex = 0;
			this.treeDestination.PathChanged += new WindowsExplorer.ExplorerTree.PathChangedEventHandler(this.TreeControls_PathChanged);
			// 
			// btnMatchDirectories
			// 
			this.btnMatchDirectories.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMatchDirectories.Location = new System.Drawing.Point(258, 319);
			this.btnMatchDirectories.Name = "btnMatchDirectories";
			this.btnMatchDirectories.Size = new System.Drawing.Size(114, 23);
			this.btnMatchDirectories.TabIndex = 1;
			this.btnMatchDirectories.Text = "&Add Directories";
			this.btnMatchDirectories.UseVisualStyleBackColor = true;
			this.btnMatchDirectories.Click += new System.EventHandler(this.btnMatchDirectories_Click);
			// 
			// btnDone
			// 
			this.btnDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDone.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnDone.Location = new System.Drawing.Point(378, 319);
			this.btnDone.Name = "btnDone";
			this.btnDone.Size = new System.Drawing.Size(106, 23);
			this.btnDone.TabIndex = 3;
			this.btnDone.Text = "&Done";
			this.btnDone.UseVisualStyleBackColor = true;
			// 
			// UpdateGUITimer
			// 
			this.UpdateGUITimer.Enabled = true;
			this.UpdateGUITimer.Tick += new System.EventHandler(this.UpdateGUITimer_Tick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(86, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Source Directory";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(367, 5);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(105, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Destination Directory";
			// 
			// SyncFolderBrowser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(484, 344);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnDone);
			this.Controls.Add(this.btnMatchDirectories);
			this.Controls.Add(this.splitContainer1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(450, 380);
			this.Name = "SyncFolderBrowser";
			this.Text = "Add Folder Source & Destination";
			this.Load += new System.EventHandler(this.Control_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private WindowsExplorer.ExplorerTree treeSource;
		private WindowsExplorer.ExplorerTree treeDestination;
		private System.Windows.Forms.Button btnMatchDirectories;
		private System.Windows.Forms.Button btnDone;
		private System.Windows.Forms.Timer UpdateGUITimer;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
	}
}