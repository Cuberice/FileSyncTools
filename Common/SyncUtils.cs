using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Hosting;
using System.Windows.Forms;
using Common.Properties;
using System.Web;
using TvdbLib;
using TvdbLib.Data;


namespace Common
{
	public class SyncUtils
	{
		public SyncUtils()
		{
			SyncPathList = new List<SyncPath>();
			DbConnection = GetDataConnection();
			TestConnection();
		}

		#region Fields & Properties
		public const int OneSecond = 1000; //milliseconds
		public const int OneMinute = (OneSecond * 60); //min 

		public SQLiteConnection DbConnection { get; set; }
		public List<SyncPath> SyncPathList { get; set; }
		private string TerraCopyPath { get; set; }
		private int ProcessWaitTime { get; set; }
		public int NotificationInterval { get; set; }
		public bool NotificationEnabled { get; set; }
		public bool Busy { get; set; }

		public bool HasValidProfile { get { return SyncPathList != null && SyncPathList.Any(); } }

		private const string DefaultTerraCopyExe = "C:\\Program Files\\TeraCopy\\TeraCopy.exe";
		private const string DefaultuTorrentExt =".!ut";
		private const string DefaultTorrentExt =".torrent";
		private const string DefaultDbExt =".db";
		private const int DefaultProcessWaitTime = 10; //seconds
		private const int DefaultNotificationInterval = 60; //minutes
		private const bool DefaultNotificationEnabled = false;
		
		private const string ConfigTCPathKey = "TerraCopyPath";
		private const string ConfigProcessWaitTimeKey = "ProcessWaitTime";
		private const string ConfigSkipExtensionsKey = "SkipExtensions";
		private const string ConfigNotificationTimeKey = "NotificationTime";
		private const string ConfigNotificationEnabledKey = "NotificationsEnabled";

		#endregion

		#region Properties

		public DateTime? LastSyncDate { get { return SyncPathList.Any(p => p.LastSyncDate.HasValue) ? SyncPathList.Select(p => p.LastSyncDate).Where(d => d.HasValue).OrderByDescending(d => d.Value).First() : null; } }

		public string DbConnectionStr { get { return DbConnection.ConnectionString; } }
		#endregion

		#region Settings

		public bool GetSettings()
		{
			try
			{
				object path = Queries.GetSettings(DbConnection, ConfigTCPathKey);
				object waittime = Queries.GetSettings(DbConnection, ConfigProcessWaitTimeKey);
				object notify = Queries.GetSettings(DbConnection, ConfigNotificationEnabledKey);
				object notifyTime = Queries.GetSettings(DbConnection, ConfigNotificationTimeKey);
				object skipExtensions = Queries.GetSettings(DbConnection, ConfigSkipExtensionsKey);

				TerraCopyPath = string.IsNullOrEmpty(path.ToString()) ? DefaultTerraCopyExe : path.ToString();
				ProcessWaitTime = waittime == null ? DefaultProcessWaitTime : Convert.ToInt32(waittime);
				NotificationInterval = notifyTime == null ? DefaultNotificationInterval : Convert.ToInt32(notifyTime);
				NotificationEnabled = notify == null ? DefaultNotificationEnabled : Convert.ToBoolean(notify);
				
				return true;
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
				return false;
			}
		}		
		
		public static List<string> GetExtensionSetting(SQLiteConnection conn)
		{
			try
			{
				object obj = Queries.GetSettings(conn, ConfigSkipExtensionsKey);

				List<string> list = obj == null
					? new List<string>() { DefaultTorrentExt, DefaultuTorrentExt, DefaultDbExt }
					: obj.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

				return list.Select(s => s.TrimEnd(new[] { ',' }).Trim()).ToList();
				
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
				return new List<string>();
			}
		}

		#endregion

		#region Helpers

		public static DateTime? GetLastSyncDate(List<SyncPath> list)
		{
			return list.Any(p => p.LastSyncDate.HasValue) ? list.Select(p => p.LastSyncDate).Where(d => d.HasValue).OrderByDescending(d => d.Value).First() : null; 
		}

		/// <summary>
		/// Creates a Bulleted String from the given Format
		/// </summary>
		/// <param name="strings"></param>
		/// <param name="bullet"></param>
		/// <param name="format">Format for each string element {Bullet}{string}</param>
		/// <returns></returns>
		public static string BulletedMessage(string[] strings, string format)
		{
			const char smallBullet = '\u00b7';
			const char mediumBullet = '\u25cf';

			format = format ?? "{0}{1}";

			string message = strings.Aggregate(string.Empty, (current, str) => current + (string.Format(format, mediumBullet, str) + "\n"));
			return message;
		}
		#endregion
		
		#region Terra Copy Sync
		public void PerformSync(bool showResults)
		{
			Busy = true;
			bool CreateDestDir = false; bool AskDir = false;
			SetStatusAll(SyncPath.SyncStatus.New);  //Reset Status

			foreach (SyncPath sp in SyncPathList.Where(sp => sp.HasSyncableFiles))
			{
				try
				{
					sp.SetFiles(sp.GetNotSyncedFiles);
					if (!sp.SourceDir.RefreshExist())
					{
						SetStatus(sp, SyncPath.SyncStatus.DirInfo, sp.GetDirNotFoundMessage(true) );						
						continue;
					}

					if (!sp.DestinationDir.RefreshExist())
					{
						SetStatus(sp, SyncPath.SyncStatus.DirInfo, sp.GetDirNotFoundMessage(false));	
						if (!AskDir)
						{
							CreateDestDir = MessageBox.Show(string.Format("Create Destination Directories [{0}]?", sp.DestinationDir.FullName), "Create Directories?", MessageBoxButtons.YesNo) == DialogResult.Yes;
							AskDir = true;
						}
							
						if(CreateDestDir)
						{
							try
							{
								sp.DestinationDir.Create();
								
								if (!sp.DestinationDir.RefreshExist())
									throw new Exception("Directory Not Created...");

								SetStatus(sp, SyncPath.SyncStatus.DirInfo, sp.GetDirCreateMessage() );
							}
							catch (Exception e)
							{
								SetStatus(sp, SyncPath.SyncStatus.Error, sp.GetDirFailedCreateMessage());
								Trace.WriteLine(e);
							}
						}
					}

					sp.CheckUpdateSubFolderFiles(true);

					if (sp.NonExistingFiles.Any()) //Still unsynced files which is still not found 
					{
						SetStatus(sp, SyncPath.SyncStatus.FileError, sp.GetNotExistingFilesMessage());
						sp.SetFilesOnlyExisting(); //Take only Files that exist
					}
					
					FileInfo fileListFile = CreateSyncFileList(sp.Files);
					if (ExecuteTerraCopy(fileListFile, sp, showResults))
					{
						sp.SetSyncFilesCompleted();
						UpdateDatabaseSync(DbConnection, sp);

						if(sp.HasErrorFiles.Any())  
							SetStatus(sp,  SyncPath.SyncStatus.FileError, sp.GetFailedCopyFilesMessage());
						else
							SetStatus(sp, SyncPath.SyncStatus.Copied);
					}
				}
				catch (Exception e)
				{
					SetStatus(sp, SyncPath.SyncStatus.Error, string.Format("Error during Sync: {0}", sp.Name));
					Trace.WriteLine(e);
				}
			}
			
			Busy = false;
		}

		public bool ExecuteTerraCopy(FileInfo fileList, SyncPath sp, bool showResults)
		{
			try
			{
				string arg_fileinfo = String.Format("Copy *\"{0}\" \"{1}\" /SkipAll {2}", fileList.FullName, sp.DestinationDir.FullName, showResults ? "/NoClose" : " /Close");
				Process process = new Process
				{
					StartInfo =
					{
						FileName = TerraCopyPath,
						Arguments = arg_fileinfo,
						UseShellExecute = false,
						CreateNoWindow = false,
						RedirectStandardOutput = false
					},
					EnableRaisingEvents = true
				};
				
				process.Start();
				process.WaitForExit(ProcessWaitTime * OneMinute );
				process.Close();
				return true;
			}
			catch (Exception regsEx)
			{
				Trace.WriteLine("FAILED TO INITIATE TERACOPY \n" +regsEx);
				return false;
			}
		}

		public bool ExecuteTerraCopySingle(FileInfo file, DirectoryInfo destinationDir)
		{
			try
			{
				string arg_fileinfo = String.Format("Copy {0} {1} /SkipAll /NoClose", file.FullName, destinationDir);
				Process process = new Process
				{
					StartInfo =
					{
						FileName = TerraCopyPath,
						Arguments = arg_fileinfo,
						UseShellExecute = false,
						CreateNoWindow = false,
						RedirectStandardOutput = true
					}
				};
				process.Start();
				process.WaitForExit(ProcessWaitTime * OneSecond);
				process.Close();
				return true;
			}
			catch (Exception regsEx)
			{
				Trace.WriteLine("FAILED TO INITIATE TERACOPY \n" + regsEx);
				return false;
			}
		}

		private FileInfo CreateSyncFileList(List<SyncFile> list)
		{
			StringBuilder sb = new StringBuilder();
			FileInfo fileInfo = new FileInfo(string.Format("{0}\\TerraCopyList\\FileList_{1}{2}.tcl", Application.StartupPath, DateTime.Now.Second, DateTime.Now.Millisecond));
		
			if (!fileInfo.Directory.RefreshExist())
				fileInfo.Directory.Create();
		
			list.ForEach(f => sb.AppendLine(f.File.FullName));

			using (StreamWriter file = new StreamWriter(fileInfo.FullName))
			{
				try
				{
					file.Write(sb.ToString());
				}
				catch (Exception e)
				{
					Trace.Write("CreateSyncFiles Exception: " + fileInfo.FullName);
				}
			}
			return fileInfo;
		}
		#endregion

		#region Profile Management

		/// <summary>
		/// Reads the Profile file and returns the SyncPath Collection
		/// </summary>
		public List<SyncPath> ReadSyncProfileFile(FileInfo file)
		{
			List<SyncPath> list = new List<SyncPath>();
			try
			{
				using (StreamReader sr = new StreamReader(file.FullName))
				{
					String line;
					while ((line = sr.ReadLine()) != null)
					{
						//string strLastSyncDate;
						//if (line.Contains(ConfigLastSyncDateKey)) //&& GetSetting(new List<string> {line}, ConfigLastSyncDateKey, out strLastSyncDate))
						//	continue; //LastSyncDate = Convert.ToDateTime(strLastSyncDate);

						var files = line.Split(',');
						if (files.Count() == 2)
						{
							list.Add(new SyncPath(string.Empty, files.First().TrimEnd(), files.Last().TrimEnd()));
						}
						if (files.Count() == 3)
						{
							list.Add(new SyncPath(files.First().TrimEnd(), files[1].TrimEnd(), files.Last().TrimEnd()));
						}
					}
				}
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
			}
			return list;
		}

		public void MergeProfiles(FileInfo f)
		{
			List<SyncPath> list = ReadSyncProfileFile(f);
			foreach (SyncPath nsp in list.Where(nsp => !ListContainsPath(nsp.SourceDir.FullName, nsp.DestinationDir.FullName )))
			{
				SyncPathList.Add(nsp);
				Queries.UpdatePathDatabase(DbConnection, nsp);
			}

			UpdateFilesCollection(DbConnection, SyncPathList, GetExtensionSetting(DbConnection), true);
			RaiseSyncPathChanged();
		}

		public bool ListContainsPath(string source, string dest)
		{
			return SyncPathList.Any(p => p.SourceDir.FullName == source && p.DestinationDir.FullName == dest);
		}
		
		#endregion

		#region UpdateCollectionEvent
		protected static readonly object EventKey = new object();
		public delegate void UpdatedCollectionEventHandler(object sender, EventArgs args);
		public event UpdatedCollectionEventHandler UpdatedCollection;
		
		private void RaiseSyncPathChanged()
		{
			if(UpdatedCollection != null)
				UpdatedCollection(this, new EventArgs());
		}

		#endregion 		
		
		#region UpdateOnCopyProcess
		protected static readonly object EventCopiedKey = new object();
		public delegate void StatusUpdateEvent(object sender, StatusChangedEventArgs args);
		public event StatusUpdateEvent SyncProcessStatusUpdate;
		public class StatusChangedEventArgs
		{
			public SyncPath.SyncStatus ChangedStatus;
		}

		private void RaiseSyncStatusChanged(SyncPath.SyncStatus toStatus)
		{
			if (SyncProcessStatusUpdate != null)
				SyncProcessStatusUpdate(this, new StatusChangedEventArgs {ChangedStatus = toStatus});
		}

		public void SetStatusAll(SyncPath.SyncStatus status)
		{
			SyncPathList.ForEach(s => s.Status = status);
			RaiseSyncStatusChanged(status);
		}

		public void SetStatus(SyncPath path, SyncPath.SyncStatus status)
		{
			SetStatus(path, status, string.Empty);
		}
		public void SetStatus(SyncPath path, SyncPath.SyncStatus status, string desc)
		{
			Trace.WriteLine(desc);
			if(path.StatusPriority[status] > path.StatusPriority[path.Status])
			{
				path.SetStatus(status, desc);
				RaiseSyncStatusChanged(status);
			}
		}

		#endregion

		#region Database

		#region Connection

		public bool TestConnection()
		{
			try
			{
				return Queries.SyncPath_All(DbConnection);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to Connect to Database : " + ex.Message);
				Console.WriteLine(ex);
				return false;
			}
		}

		public void WaitForConnection(SQLiteConnection conn)
		{
			do
			{
				Thread.Sleep(100);
			} while (conn.State == ConnectionState.Open);
		}

		/// <summary>
		/// Creates a New connection and waits for the Database to become available
		/// </summary>
		/// <returns></returns>
		public static SQLiteConnection GetDataConnection()
		{
			try
			{
				Trace.WriteLine(string.Format("USING DATABASE CONNECTION: {0}", Settings.Default.DbConnectionString));
				SQLiteConnection conn = new SQLiteConnection(Settings.Default.DbConnectionString)
				{
					DefaultTimeout = 30,
					Flags = SQLiteConnectionFlags.LogCallbackException | SQLiteConnectionFlags.LogModuleException,
					ParseViaFramework = false
				};

				do
				{

				} while (conn.State == ConnectionState.Open);

				return conn;
			}
			catch (Exception e)
			{
				Trace.WriteLine(string.Format("FAILED TO OPEN A DATABASE CONNECTION TO {0}\n{1}",
					Settings.Default.DbConnectionString, e));
				return null;
			}
		}

		public static string GetDataConnectionString()
		{
			return GetDataConnection().ConnectionString;
		}

		#endregion

		#region Update Files Collection

		/// <summary>
		/// Reads the Database and Updates the Collection
		/// </summary>
		public bool ReadFileCollection()
		{
			try
			{
				SyncPathList = Queries.ReadFileData(DbConnection);
				return SyncPathList.Any();
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
				return false;
			}
			finally
			{
				RaiseSyncPathChanged();
			}
		}

		public void UpdateFilesCollectionApp()
		{
			ReadFileCollection();
			UpdateFilesCollection(DbConnection, SyncPathList, GetExtensionSetting(DbConnection), true);
		}

		/// <summary>
		/// Updates the Source Files on the system to the Database
		/// </summary>
		public static void UpdateFilesCollectionWeb()
		{
			SQLiteConnection conn = GetDataConnection();
			UpdateFilesCollection(conn, View_GetAllData(), GetExtensionSetting(conn), false);
		}

		/// <summary>
		/// Updates the Existing files on the Filesystem to the Database
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="list"></param>
		/// <param name="skip"></param>
		/// <param name="checkmissingfiles"></param>
		public static void UpdateFilesCollection(SQLiteConnection conn, List<SyncPath> list, List<string> skip, bool checkmissingfiles)
		{
			try
			{
				foreach (SyncPath sp in list)
				{
					if (sp.SourceDir == null || !sp.SourceDir.RefreshExist())
					{
						sp.SetError();
						continue;
					}

					List<FileInfo> files = new List<FileInfo>();
					files = sp.RecurseDirs(sp.SourceDir, files);
					files = files.Where(f => !skip.Contains(f.Extension.Trim(), StringComparer.OrdinalIgnoreCase)).ToList();

					foreach (FileInfo fi in files.Where(fi => !Queries.SyncFiles_ExistInDb(conn, fi)))
					{
						sp.AddFile(fi);
					}
				}

				if (checkmissingfiles)
					CheckMissingFiles(list);

				foreach (SyncPath sp in list)
				{
					foreach (SyncFile sf in sp.Files)
					{
						Queries.UpdateFileDatabase(conn, sp, sf);
					}
				}
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
			}
		}

		public void AddDestinationFiles(string[] fileNames)
		{
			if (!fileNames.Any())
				return;

			FileInfo dfi = new FileInfo(fileNames.First());
			DirectoryInfo di = new DirectoryInfo(dfi.DirectoryName);

			if (!SyncPathList.Any(sp => sp.DestinationDir.FullName == di.FullName))
			{
				DirectoryInfo sd = SyncPathList.First().SourceDir.Parent.Parent.CreateSubdirectory(di.Name);

				SyncPath nsp = new SyncPath(di.Name, sd.FullName, di.FullName);
				SyncPathList.Add(nsp);
				Queries.UpdatePathDatabase(DbConnection, nsp);
			}

			if (SyncPathList.Any(sp => sp.DestinationDir.FullName == di.FullName))
			{
				SyncPath sp = SyncPathList.First(spl => spl.DestinationDir.FullName == di.FullName);
				foreach (string sfn in fileNames)
				{
					FileInfo fi = new FileInfo(sfn);
					sp.AddFile(fi);
				}

				foreach (SyncFile sf in sp.Files)
				{
					sf.SetSynced();
					Queries.UpdateFileDatabase(DbConnection, sp, sf);
				}
			}

			RaiseSyncPathChanged();
		}

		#endregion

		#region Update Path Collection

		public void UpdateAllPathsDatabase()
		{
			foreach (SyncPath nsp in SyncPathList)
			{
				Queries.SyncPath_Update(DbConnection, nsp);
			}
		}

		public void UpdateDatabaseSync(SQLiteConnection conn, SyncPath sp)
		{
			try
			{
				bool updated = sp.Files.Aggregate(false, (current, sf) => current | Queries.UpdateFileDatabase(conn, sp, sf));

				if (updated)
					Queries.SyncPath_UpdateError(conn, sp);

			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
			}
		}

		#endregion

		public static void SubmitFilesWeb(List<SyncFile> list)
		{
			SQLiteConnection conn = GetDataConnection();
			list.ForEach(sf =>
			{
				if (sf.AllowIsSyncEdit && sf.AllowIsWatchedEdit)
					Queries.SyncFiles_Update(conn, sf);
				else if (sf.AllowIsWatchedEdit)
					Queries.SyncFiles_UpdateWatch(conn, sf);
				else if (sf.IsSynced)
					Queries.SyncFiles_UpdateSync(conn, sf);
			});
		}

		public static void SubmitFilesWatch(string id, bool value)
		{
			SQLiteConnection conn = GetDataConnection();
			Queries.SyncFiles_UpdateWatch(conn, new SyncFile(id, value));
		}

		#endregion

		#region Get Web View Data
		
		public static string View_GetConnectionDetails()
		{
			SQLiteConnection conn = GetDataConnection();
			bool connect = Queries.SyncPath_All(conn);
			string str = Queries.SyncPath_AllString(conn);
			return conn.ConnectionString + " Connect:" + connect + " DateTime: "+ DateTime.Now + "<br/>" + str;
		}

		public static List<SyncPath> View_GetAllData()
		{
			SQLiteConnection conn = GetDataConnection();
			List<SyncPath> list = Queries.ReadFileData(conn);
			return list;
		}

		public static List<SyncPath> View_GetAllCollection()
		{
			List<SyncPath> list = View_GetAllData();
			foreach (SyncPath sp in list)
			{
				sp.SetFiles(() => sp.Files.OrderBy(sf => sf.Season).ThenBy(sf => sf.Episode).ToList());
			}
			return list.Where(sp => sp.Files.Any()).OrderBy(sp => sp.Name).ToList();
		}

		public static List<SyncPath> View_GetNotSyncedCollection()
		{
			List<SyncPath> list = View_GetAllData();
			foreach (SyncPath sp in list)
			{
				sp.SetFiles(() => sp.GetNotSyncedOrMissingFiles());
			}

			return list.Where(sp => sp.Files.Any()).OrderByDescending(sp => sp.LastFileDate).ToList();
		}

		public static List<SyncPath> View_SyncedCollection()
		{
			List<SyncPath> list = View_GetAllData();
			DateTime? last = GetLastSyncDate(list);

			list = last.HasValue ? list.Where(sp => sp.LastSyncDate >= last.Value.Subtract(TimeSpan.FromHours(2))).ToList() : list;
			foreach (SyncPath sp in list)
			{
				sp.SetFiles(() => sp.GetSyncedFiles(last));
			}

			return list.Where(sp => sp.Files.Any()).OrderByDescending(sp => sp.LastSyncDate).ToList();
		}

		public static List<SyncPath> View_GetWatchCollection()
		{
			List<SyncPath> list = View_GetAllData();
			foreach (SyncPath sp in list)
			{
				sp.SetFiles(() => sp.GetWatchFiles());
			}

			return list.Where(sp => sp.Files.Any()).OrderBy(sp => sp.LastWatchDate).ToList();
		}		
		
		public static List<SyncPath> View_GetNotWatchedCollection()
		{
			List<SyncPath> list = View_GetAllData();
			foreach (SyncPath sp in list)
			{
				sp.SetFiles(() => sp.GetNotWatchedFiles() );
			}

			return list.Where(sp => sp.Files.Any()).OrderBy(sp => sp.LastSyncDate).ToList();
		}

		public static List<SyncPath> View_GetErrorCollection()
		{
			List<SyncPath> list = View_GetAllData();
			foreach (SyncPath sp in list)
			{
				sp.SetFiles(() => sp.GetErrorSyncFiles( ));
			}

			return list.Where(sp => sp.Files.Any()).OrderBy(sp => sp.Name).ToList();
		}

		#endregion

		#region File Name Matching & Missing Files

		public void TestRegex()
		{
			foreach (SyncPath sp in SyncPathList)
			{
				foreach (SyncFile sf in sp.Files)
				{
					sf.DetermineSeasonEpisode();
				}
			}
		}

		public static bool HasEpisodeAired(SyncPath sp, int season, int episode)
		{
			/*Account Identifier		50D240BFC7FF5B90 
			 *API Key							5570CC0F3886DE20 */
			try
			{
				if (!sp.TvDbID.HasValue)
					return false;
				
				TvdbDownloader d = new TvdbDownloader("5570CC0F3886DE20");
				TvdbEpisode tvEp = d.DownloadEpisode(sp.TvDbID.Value, season, episode, TvdbEpisode.EpisodeOrdering.DefaultOrder, TvdbLanguage.DefaultLanguage);
				return DateTime.Now > tvEp.FirstAired;
			}
			catch (WebException e)
			{
				Console.WriteLine(e);
				return false;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return false;
			}
		}

		public static void CheckMissingFiles(List<SyncPath> paths )
		{
			foreach (SyncPath sp in paths)
			{
				List<SyncFile> addList = new List<SyncFile>();
				foreach(int season in sp.Files.Select(f => f.Season).Distinct())
				{
					if(!sp.MinEpisode(season).HasValue || !sp.MaxEpisode(season).HasValue)
						continue;

					int minEp = sp.MinEpisode(season).Value;
					int maxEp = sp.MaxEpisode(season).Value;

					IEnumerable<int> range = Enumerable.Range(minEp, maxEp - minEp +1);
					List<int> missing = range.Except(sp.Files.Where(f => f.Episode.HasValue).Select(f => f.Episode.Value)).ToList();

					if (missing.Any())
					{
						missing.ForEach(me => addList.Add(new SyncFile(sp, season, me, true))); //Create New SyncFile, Add to SyncPath with Missing = TRUE
					}
				}

				int? maxSeason = sp.MaxSeason;
				int? maxEpisode;
				if (maxSeason.HasValue && (maxEpisode = sp.MaxEpisode(maxSeason.Value)).HasValue )
				{
					SyncFile f = sp.EpisodeFile(maxSeason.Value, maxEpisode.Value);

					if (f.FileDate.HasValue && DateTime.Now.Subtract(f.FileDate.Value) > TimeSpan.FromDays(7))
					{
						if (HasEpisodeAired(sp, maxSeason.Value, maxEpisode.Value + 1))
							addList.Add(new SyncFile(sp, maxSeason.Value, maxEpisode.Value + 1, true));		//Create New SyncFile, Add to SyncPath with Missing = TRUE
					}
				}

				if (addList.Any())
					addList.ForEach(af => sp.AddFile(af));
			}
		}

		#endregion
		
		#region Delete Files

		public void DoDeleteOldFiles()
		{
			List<FileInfo> deletes = new List<FileInfo>();
			foreach (SyncPath sp in SyncPathList)
			{
				sp.CheckUpdateSubFolderFiles(false);
				foreach (SyncFile sf in sp.Files)
					//.Where(f => f.File.Exists && f.IsSynced && f.SyncDate.HasValue && DateTime.Now.Subtract(f.SyncDate.Value) > TimeSpan.FromDays(14)))
				{
					deletes.Add(sf.File);
				}
			}

			BackgroundWorker worker = new BackgroundWorker();
			worker.DoWork += (sender, args) => ((List<FileInfo>) args.Argument).ForEach(DeleteFile);
			worker.RunWorkerAsync(deletes);
		}

		protected void DeleteFile(FileInfo fi)
		{
			try
			{
				Trace.WriteLine(string.Format("\nDeleting: {0}", fi.Name));
				File.Delete(fi.FullName);
			}
			catch (Exception e)
			{
				Trace.WriteLine(string.Format("Unable to Delete File [{0}] - {1}", fi.FullName, e));
			}
			finally
			{
				fi.Refresh();
				Trace.WriteLine(fi.Exists
					? string.Format("Delete Failed: {0}", fi.Name)
					: string.Format("Successfully Deleted: {0}", fi.Name));
			}
		}

		#endregion
	}

	public static class Extensions
	{
		public static bool RefreshExist(this DirectoryInfo dir)
		{
			dir.Refresh();
			return dir.Exists;
		}

		public static void WriteLine(params string[] strings)
		{
			Array.ForEach(strings, s => Trace.WriteLine(s));
		}
	}
}


/* Account Identifier		50D240BFC7FF5B90
 * API Key							5570CC0F3886DE20 
 */
