using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsExplorer;
using Common;

namespace FileSyncTool
{
	public partial class SyncFolderBrowser : Form
	{
		public SyncFolderBrowser()
		{
			InitializeComponent();
		}

		public SyncUtils Utils { get; set; }
		public ExplorerTree SourceTree { get { return treeSource; } }
		public ExplorerTree DestinationTree { get { return treeDestination; } }
		private bool UpdateMode;

		private void Control_Load(object sender, EventArgs e)
		{
			SourceTree.ResetSelectedFolder();
			DestinationTree.ResetSelectedFolder();
		}

		public DialogResult ShowDialog(IWin32Window owner, SyncPath item)
		{
			UpdateMode = true;
			btnMatchDirectories.Enabled = false;
			btnDone.Text = "Update";

			SourceTree.SelectedPath = item.SourceDir.FullName;
			SourceTree.StartWithMyComputer = false;
			DestinationTree.SelectedPath = item.DestinationDir.FullName;
			DestinationTree.StartWithMyComputer = false;

			DialogResult result;
			if((result = ShowDialog(owner)) == DialogResult.OK)
			{
				item.SourceDir = new DirectoryInfo(SourceTree.SelectedPath);
				item.DestinationDir = new DirectoryInfo(DestinationTree.SelectedPath);
				//Utils.WriteSyncProfileFile();
			}

			return result;
		}

		private void btnMatchDirectories_Click(object sender, EventArgs e)
		{
			//Utils.AddToProfile(SourceTree.SelectedPath, DestinationTree.SelectedPath);
			SourceTree.ResetSelectedFolder();
			DestinationTree.ResetSelectedFolder();
			UpdateMatchButtonGUI();
		}

		private void TreeControls_PathChanged(object sender, EventArgs e)
		{
			UpdateMatchButtonGUI();
		}

		private void UpdateMatchButtonGUI()
		{
			bool hasValidData = SourceTree.SelectedPath != null &&
				DestinationTree.SelectedPath != null &&
				!Utils.ListContainsPath(SourceTree.SelectedPath, DestinationTree.SelectedPath);

			btnMatchDirectories.Enabled = !UpdateMode && hasValidData;
			btnDone.Enabled = UpdateMode && hasValidData || !UpdateMode;
		}

		private void UpdateGUITimer_Tick(object sender, EventArgs e)
		{
			UpdateMatchButtonGUI();
		}
	}
}
