using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Common;
using Microsoft.Win32;

namespace FileSyncTool
{
	public partial class FileSyncForm : Form
	{
		public FileSyncForm()
		{
			InitializeComponent();

			Utils = new SyncUtils();

			clError.StartUp("Error", SyncPath.StatusColor[SyncPath.SyncStatus.Error]);
			clLocked.StartUp("File Error", SyncPath.StatusColor[SyncPath.SyncStatus.FileError]);
			clHasUpdate.StartUp("Update", SyncPath.StatusColor[SyncPath.SyncStatus.UpdateAvailable]);
			clCopied.StartUp("Copied", SyncPath.StatusColor[SyncPath.SyncStatus.Copied]);
			clDirUpdate.StartUp("Dir Info", SyncPath.StatusColor[SyncPath.SyncStatus.DirInfo]);
		}

		public FileSyncForm(string[] args): this()
		{ 
			if (UpdateGUITimer != null)
				UpdateGUITimer.Stop();

			if(args != null && args.Length > 0)
			{
				
			}
		}

		private void FileSyncForm_Shown(object sender, EventArgs e)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				UpdateGUI(false);
				DoTimerTick();
				ResetFileWatcher();
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex);
			}
			finally
			{
				UpdateGUI(true);
				Cursor.Current = Cursors.Default;
			}
		}

		private void FileSyncForm_Load(object sender, EventArgs e)
		{
			try
			{
				Utils.UpdatedCollection += Utils_UpdatedCollection;
				Utils.SyncProcessStatusUpdate += Utils_StatusUpdate;

				Utils.GetSettings();

				SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
				SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;

				btnSync.Text = strSyncButton;
				UpdateGUITimer.Enabled = true;
				UpdateGUITimer.Start();
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex);
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if(MessageBox.Show(this, "Closing will prevent synchronization of files to Web Application. Are you sure?", "Close Application", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
				e.Cancel = true;

			base.OnFormClosing(e);
		}

		#region Properties

		private SyncUtils Utils { get; set; }
		private int TickCounter { get; set; }

		#endregion
		
		#region Form Control Events

		private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
		{
			switch (e.Reason)
			{
				case SessionSwitchReason.SessionLock:
				{
					if (UpdateFileWatcherTimer.Enabled)
					{
						Debug.WriteLine("On Lock: Was running, Stopping Timer");
						UpdateFileWatcherTimer.Stop();
					}
					break;
				}
				case SessionSwitchReason.SessionUnlock:
				{
					UpdateFileWatcherTimer.Start();
					Debug.WriteLine("On UnLock: Starting Timer");
					break;
				}
				case SessionSwitchReason.SessionLogoff:
				{
					if (UpdateFileWatcherTimer.Enabled)
					{
						Debug.WriteLine("On Logoff: Was running, Stopping Timer");
						UpdateFileWatcherTimer.Stop();
					}
					break;
				}
				case SessionSwitchReason.SessionLogon:
				{
					UpdateFileWatcherTimer.Start();
					Debug.WriteLine("On Logon: Starting Timer");
					break;
				}
			}
		}

		private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
		{
			switch (e.Mode)
			{
				case PowerModes.Suspend:
				{
					if (UpdateFileWatcherTimer.Enabled)
					{
						Debug.WriteLine("On Suspend: Was running, Stopping Timer");
						UpdateFileWatcherTimer.Stop();
					}
					break;
				}
				case PowerModes.Resume:
				{
					UpdateFileWatcherTimer.Start();
					Debug.WriteLine("On Resume: Starting Timer");
					break;
				}
			}
		}

		private void FileSyncForm_Closing(object sender, FormClosingEventArgs e)
		{
			UpdateGUITimer.Stop();
		}

		private void btnSync_Click(object sender, EventArgs e)
		{
			DoSync();
		}

		private void btnAddSyncDir_Click(object sender, EventArgs e)
		{
			using (SyncFolderBrowser f = new SyncFolderBrowser())
			{
				f.Utils = Utils;
				f.ShowDialog(this);
				//BindListToProfile();
			}
		}

		private void btnRemoveSyncDir_Click(object sender, EventArgs e)
		{
			if (lstbSyncPaths.SelectedItem == null)
				return;

			SyncPath item = lstbSyncPaths.SelectedItem as SyncPath;
			if (item == null)
				return;

			//Utils.RemoveFromProfile(item);
			//BindListToProfile();
		}

		private void lstSyncPaths_DoubleClick(object sender, MouseEventArgs e) //Change Sync Dir's
		{
			if (lstbSyncPaths.SelectedItem == null)
				return;

			SyncPath item = lstbSyncPaths.SelectedItem as SyncPath;
			if (item == null)
				return;

			using (SyncFolderBrowser f = new SyncFolderBrowser())
			{
				f.Utils = Utils;
				f.ShowDialog(this, item);
			}
			UpdateGUI(true);
		}

		private void lstSyncPaths_Click(object sender, MouseEventArgs e)
		{
			if (lstbSyncPaths.SelectedItem == null)
				return;

			SyncPath item = lstbSyncPaths.SelectedItem as SyncPath;
			if (item == null)
				return;

			int Dx = MousePosition.X - Location.X;
			int Dy = MousePosition.Y - Location.Y;
			Point p = new Point(Location.X + Dx, Location.Y + Dx);
			string str = string.Format("Mx={0}, My={1}, Fx={2}, Fy={3}, Dx={4}, Dy={5}", MousePosition.X, MousePosition.Y, Location.X, Location.Y, Dx, Dy);
			
			SyncPathItemToolTip.Show(GetToolTipText(), this, Dx, Dy );
		}

		private void OpenProfile(object sender, EventArgs e)
		{
			using (OpenFileDialog dialog = new OpenFileDialog {CheckFileExists = false, Filter = "Sync Profile|*.spf"})
			{
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					Utils.MergeProfiles(new FileInfo(dialog.FileName));
				}
			}
		}

		private void BtnCheckNow_Click(object sender, EventArgs e)
		{
			DoCheckFileAvailability(true);
		}
		
		private void cbxNotificationsEnabled_Checked(object sender, EventArgs e)
		{
			Utils.NotificationEnabled = cbxNotificationsEnabled.Checked;
			ResetFileWatcher();
		}

		private void listBox_DrawItem(object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();
			bool selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

			int index = e.Index;
			if (index >= 0 && index < lstbSyncPaths.Items.Count)
			{
				SyncPath item = (SyncPath)lstbSyncPaths.Items[index];
				Graphics g = e.Graphics;
				Color color = selected ? Color.Gray : SyncPath.StatusColor[item.Status];

				g.FillRectangle(new SolidBrush(color), e.Bounds);
				g.DrawString(item.ToString(), e.Font,
										 (selected) ? Brushes.Black : Brushes.Black,
										 lstbSyncPaths.GetItemRectangle(index).Location);
			}

			e.DrawFocusRectangle();
		}

		private void testDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Connection: " + Utils.DbConnectionStr);
		}

		private void addFilesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Utils.Busy = true;
			try
			{
				using (OpenFileDialog d = new OpenFileDialog(){Multiselect = true})
				{
					if(d.ShowDialog(this) == DialogResult.Cancel)
						return;

					Utils.AddDestinationFiles(d.FileNames);
				}
			}
			finally
			{
				Utils.Busy = false;
			}
		}

		private void DeleteOldFiles_Click(object sender, EventArgs e)
		{
			DoDeleteOldFiles();
		}

		private void ChangeBaseDir_Click(object sender, EventArgs e)
		{
			GetDriveWithDialog(Utils.SyncPathList, true, false);
			Utils.UpdateAllPathsDatabase();
		}
		#endregion

		#region Util Events

		/// <summary>
		/// Event Occurs when Items to the Sync are Added or Removed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void Utils_UpdatedCollection(object sender, EventArgs args)
		{
			BindListToProfile();
		}

		private void Utils_StatusUpdate(object sender, SyncUtils.StatusChangedEventArgs args)
		{
			lstbSyncPaths.BeginUpdate();
			lstbSyncPaths.Refresh();
			Application.DoEvents();
			lstbSyncPaths.EndUpdate();
		}

		#endregion

		#region Methods

		private void DoSync()
		{
			bool destExist;
			bool sourceExist;
			if (HasSourceDestinationDrives(out sourceExist, out destExist))
				btnSync.Text = "Drive Letter Updated...";
			
			btnSync.Text = strSyncButtonSynching;
			lstbSyncPaths.SelectedItems.Clear();
			Utils.PerformSync(cbxShowCopyResults.Checked);
			btnSync.Text = strSyncButtonCompleted;

			Application.DoEvents();
		}

		private void DoTimerTick()
		{
			if (!Utils.NotificationEnabled || Utils.Busy)
				return;

			DoCheckFileAvailability(false);
		}

		private void DoDeleteOldFiles()
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				UpdateGUI(false);
				Utils.DoDeleteOldFiles();
			}
			finally
			{
				UpdateGUI(true);
				Cursor.Current = Cursors.Default;
			}

		}

		private void DoCheckFileAvailability(bool useraction)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				UpdateGUI(false);
				
				try
				{
					Utils.UpdateFilesCollectionApp();
					Utils.DoDeleteOldFiles();
					Utils.SetStatusAll(SyncPath.SyncStatus.New);
					Utils.SyncPathList.Where(sp => !sp.SourceDir.RefreshExist()).ToList().ForEach(sp => Utils.SetStatus(sp, SyncPath.SyncStatus.DirInfo, sp.GetDirNotFoundMessage(true)));
					List<SyncPath> paths = Utils.SyncPathList.Where(sp => sp.HasSyncableFiles).ToList();

					if (paths.Any())
					{
						foreach (SyncPath sp in paths)
						{
							sp.SetFiles(sp.GetNotSyncedFiles);
							sp.CheckUpdateSubFolderFiles(true);

							if(sp.AllFilesExist)
								Utils.SetStatus(sp, SyncPath.SyncStatus.UpdateAvailable);
							else
								Utils.SetStatus(sp, SyncPath.SyncStatus.FileError, sp.GetNotExistingFilesMessage());
						}
						
						btnSync.Text = strSyncButton;
					}
					else if (useraction)
					{
						ShowNotifyIcon("No files found available for Synchronization...");
					}
				}
				catch (Exception e)
				{
					Trace.WriteLine(e);

					if(useraction)
						ShowNotifyIcon("Error Occured while checking for new files");
				}
			}
			finally
			{
				UpdateGUI(true);
				Cursor.Current = Cursors.Default;
			}
		}
		
		/// <summary>
		/// Checks and Updates the Source and Destination Structures
		/// </summary>
		/// <param name="sourceExist">True if Source Existed before Update</param>
		/// <param name="destExist">True if Destination Existed before Update</param>
		/// <returns>True if Sourse Drives were updated</returns>
		private bool HasSourceDestinationDrives(out bool sourceExist, out bool destExist)
		{
			bool UpdatedSource = false;
			sourceExist = SyncPath.StructureExits(Utils.SyncPathList.Select(sp => sp.SourceDir).ToList());
			destExist = SyncPath.StructureExits(Utils.SyncPathList.Select(sp => sp.DestinationDir).ToList());

			if (!sourceExist && (SyncPath.GetDriveWithStructure(Utils.SyncPathList, true) || GetDriveWithDialog(Utils.SyncPathList, true, true)))
			{
				UpdatedSource = sourceExist = SyncPath.StructureExits(Utils.SyncPathList.Select(sp => sp.SourceDir).ToList());
				Trace.WriteLine("\n UPDATED SOURCE STRUCTURE");
			}

			if (!destExist && (SyncPath.GetDriveWithStructure(Utils.SyncPathList, false) || GetDriveWithDialog(Utils.SyncPathList, false, true)))
			{
				UpdatedSource = UpdatedSource || (destExist = SyncPath.StructureExits(Utils.SyncPathList.Select(sp => sp.DestinationDir).ToList()));
				Trace.WriteLine("\n UPDATED DESTINATION STRUCTURE");
			}

			return UpdatedSource;
		}

		private bool GetDriveWithDialog(List<SyncPath> spList, bool source, bool rootonly)
		{
			string desc = rootonly ? source ? "Select the SOURCE Drive Letter" : "Select the DESTINATION Drive Letter" : source ? "Select the SOURCE Location" : "Select the DESTINATION Location";
			using (FolderBrowserDialog dialog = new FolderBrowserDialog() { Description = desc })
			{
				DialogResult result = dialog.ShowDialog();
				if (result == DialogResult.Cancel)
					return false;

				spList.ForEach(sp =>
				{
					DirectoryInfo d = (source ? sp.SourceDir : sp.DestinationDir);
					int lastslash = d.FullName.LastIndexOf(@"\");
					string name = d.FullName.Substring(lastslash + 1);

					string dir = rootonly ? d.FullName.Replace(d.Root.Name, dialog.SelectedPath) : string.Format("{0}\\{1}", dialog.SelectedPath, name);
					sp.Update(source, dir);
				});
				return true;
			}
		}

		#endregion
		
		#region Drag Drop

		private void Form_DragEnter(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

			if (e.Data.GetDataPresent(DataFormats.FileDrop) && files.Count() == 1 && files.First().EndsWith(".spf"))
				e.Effect = DragDropEffects.Copy; // Okay
			else
				e.Effect = DragDropEffects.None; // Unknown data, ignore it

		}

		private void Form_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			if(files.Count() != 1 || !files.First().EndsWith(".spf"))
				return;
			
			//DoOpenProfile(files.First());
		}

		#endregion
		
		#region Update GUI

		/// <summary>
		/// Updates the List with the current Sync Path Collection
		/// </summary>
		private void BindListToProfile()
		{
			lstbSyncPaths.BeginUpdate();
			lstbSyncPaths.Items.Clear();

			if (Utils.SyncPathList != null) 
				lstbSyncPaths.Items.AddRange(Utils.SyncPathList.OrderBy(sp => sp.Name).ToArray());

			lstbSyncPaths.EndUpdate();
			Trace.WriteLine("\n UPDATED LIST PROFILE");
		}

		/// <summary>
		/// Updates GUI according to current state of the Files, Collections and global Properties
		/// </summary>
		/// <param name="enable"></param>
		private void UpdateGUI(bool enable)
		{
			lstbSyncPaths.Enabled = btnAddSyncDir.Enabled = btnRemoveSyncDir.Enabled = btnSync.Enabled = btnCheck.Enabled = menuStrip1.Enabled = enable;
			btnSync.Text = strSyncButton;
		}

		private void UpdateGUITimer_Tick(object sender, EventArgs e)
		{
			btnRemoveSyncDir.Enabled = lstbSyncPaths.SelectedItem != null;
			btnSync.Enabled = lstbSyncPaths.Items != null && lstbSyncPaths.Items.Count > 0;
		}

		#endregion
		
		#region Notification Icon

		private void ResetFileWatcher()
		{
			cbxNotificationsEnabled.Checked = Utils.NotificationEnabled;
			UpdateFileWatcherTimer.Interval = SyncUtils.OneMinute * Utils.NotificationInterval;
			UpdateFileWatcherTimer.Enabled = true;
			UpdateFileWatcherTimer.Start();
		}

		private void UpdateFileWatcherTimer_Tick(object sender, EventArgs e)
		{
			DoTimerTick();
		}
		
		/// <summary>
		/// Shows the Balloon with given Description
		/// </summary>
		/// <param name="description"></param>
		private void ShowNotifyIcon(string description)
		{
			NotifyIcon.ShowBalloonTip(NotifyTimeOut, strNotifyHeading, description, ToolTipIcon.Info);
		}

		private void NotifyIcon_Clicked(object sender, EventArgs e)
		{
			ShowAndFocusForm();

			if (btnSync.Enabled)
			{
				DoSync();
			}
		}

		private void NotifyIcon_DoubleClicked(object sender, EventArgs e)
		{
			ShowAndFocusForm();
		}

		private void ShowAndFocusForm()
		{
			if (WindowState == FormWindowState.Minimized)
			{
				Show();
				WindowState = FormWindowState.Normal;
			}

			Activate();
			Focus();
		}

		private void FileSyncForm_SizeChanged(object sender, System.EventArgs e)
		{
			ShowInTaskbar = WindowState != FormWindowState.Minimized;
		}

		#endregion

		#region ToolTip 

		private string GetToolTipText()
		{
			SyncPath item = DetermineHoveredItem();
			if (item == null)
				return string.Empty;

			return item.ErrorDescription;
		}
		private SyncPath DetermineHoveredItem()
		{
			Point listBoxClientAreaPosition = lstbSyncPaths.PointToClient(MousePosition);

			int hoveredIndex = lstbSyncPaths.IndexFromPoint(listBoxClientAreaPosition);
			if (hoveredIndex != -1)
				return lstbSyncPaths.Items[hoveredIndex] as SyncPath;
			
			return null;
		}

		private void ToolTip_Popup(object sender, PopupEventArgs e)
		{
			//(sender as ToolTip).SetToolTip(lstbSyncPaths, "2");
			//SyncPathItemToolTip.SetToolTip(lstbSyncPaths, GetToolTipText());
		}


		#endregion

		private void GetNetwork()
		{
			List<string> list = new List<string>();
			//using (DirectoryEntry root = new DirectoryEntry("WinNT:"))
			//{
			//  foreach (DirectoryEntry computers in root.Children)
			//  {
			//    foreach (DirectoryEntry computer in computers.Children)
			//    {
			//      if ((computer.Name != "Schema"))
			//      {
			//        list.Add(computer.Name);
			//      }
			//    }
			//  }
			//}
		}

		private const int NotifyTimeOut = 100000;
		private const string strNotifyHeading = "Directory Synchronizer";
		private const string strSyncButton = "Perform Sync...";
		private const string strSyncButtonSynching = "Synchronizing...";
		private const string strSyncButtonCompleted = "Sync Completed...";

		private void testSeriesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//Utils.TestRegex();
			//Utils.CheckMissingFiles();
		}
		
	}
}
