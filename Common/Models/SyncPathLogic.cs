using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common;
using Core.Data;

namespace Models
{
	public partial class SyncPath
	{
		#region Season Episode Helpers

		public int? MinEpisode(int season)
		{
			return Files.Where(f => f.Season == season).Min(sf => sf.Episode);
		}

		public int? MaxEpisode(int season)
		{
			return Files.Where(f => f.Season == season).Max(sf => sf.Episode);
		}

		public bool HasEpisode(int season, int episode)
		{
			return Files.Any(f => f.Season == season && f.Episode == episode);
		}

		public bool HasEpisodesAfter(int season, int episode)
		{
			return Files.Any(f => f.Season == season && f.Episode > episode);
		}

		public bool HasMissing(int season, int episode)
		{
			return MissingBefore(season, episode).Any();
		}

		public List<SyncFile> MissingBefore(int season, int episode)
		{
			return Files.Where(sf => sf.IsMissing && sf.Season == season && sf.Episode <= episode).ToList();
		}

		public List<int> NextEpisodes(int season, int episode, int amount)
		{
			return Files.Where(f => f.Season == season && f.Episode.HasValue && f.Episode > episode).Select(f => f.Episode.Value).OrderBy(e => e).Take(amount).ToList();
		}

		public int? MaxWatchedEpisode(int season)
		{
			return Files.Where(f => f.IsWatched && f.Season == season).Max(sf => sf.Episode);
		}

		public SyncFile EpisodeFile(int season, int episode)
		{
			return Files.First(f => f.Season.HasValue && f.Season == season && f.Episode.HasValue && f.Episode == episode);
		}

		public int? MaxSeason
		{
			get { return Files.Max(sf => sf.Season); }
		}

		public int? MaxWatchedSeasson()
		{
			return Files.Where(f => f.IsWatched).Max(sf => sf.Season);
		}

		public bool HasSeasonsAfter(int season)
		{
			return Files.Any(f => f.Season > season);
		}
		public int SeasonAfter(int season)
		{
			return Files.OrderBy(f => f.Season).First(f => f.Season > season).Season.Value;
		}

		#endregion

		#region Date Helpers

		public DateTime? LastSyncDate { get { return Files.Any(f => f.IsSynced && f.SyncDate.HasValue) ? Files.Where(f => f.IsSynced).OrderByDescending(f => f.SyncDate).First().SyncDate : null; } }
		public DateTime? LastFileDate { get { return Files.Any(f => f.FileDate.HasValue) ? Files.OrderByDescending(f => f.FileDate).First().FileDate : null; } }
		public DateTime? LastWatchDate { get { return Files.Any(f => f.WatchDate.HasValue) ? Files.Where(f => f.IsWatched).OrderByDescending(f => f.WatchDate).First().SyncDate : null; } }
		public bool IsRecentlyWatched { get { return LastWatchDate.HasValue && DateTime.Now.Subtract(LastWatchDate.Value) < TimeSpan.FromDays(14); } }

		#endregion

		#region File Helpers
		/// <summary>
		/// Returns if Path has files which are not Synced and does Exist on system
		/// </summary>
		/// <returns></returns>
		public bool HasSyncableFiles { get { return GetNotSyncedFiles().Any(); } }
		public bool AllFilesExist { get { return Files.All(sf => sf.File.Exists); } }

		public List<SyncFile> NonExistingFiles { get { return Files.Where(sf => !sf.File.Exists).ToList(); } }
		public List<SyncFile> HasErrorFiles { get { return Files.Where(sf => sf.Error > 0).ToList(); } }

		public void SetFiles(Func<List<SyncFile>> GetFiles)
		{
			Files = GetFiles();
		}
		public void SetFilesOnlyExisting()
		{
			SetFiles(() => Files.Where(sf => sf.File.Exists).ToList());
		}
		public static bool IsFileLocked(SyncFile sf)
		{
			FileStream stream = null;
			try
			{
				stream = sf.File.Open(FileMode.Open, FileAccess.Read, FileShare.None);
			}
			catch (IOException)
			{
				Trace.WriteLine("File is Locked : " + sf.File.FullName);
				return true; //Locked
			}
			finally
			{
				if (stream != null)
					stream.Close();
			}
			return false; //Not Locked
		}

		public void CheckUpdateSubFolderFiles(bool onlyNotSynced)
		{
			if (NonExistingFiles.Any())		//File which are not Synced but does not exist in base folder
				NonExistingFiles.ForEach(sf => UpdateSourceFileLocation(sf, onlyNotSynced));	//Search Subfolders
		}

		/// <summary>
		/// Searches all subdirectories for the FileName and sets the File property on SyncFile if found
		/// </summary>
		/// <param name="sf"></param>
		/// <param name="onlyNotSynced"></param>
		public void UpdateSourceFileLocation(SyncFile sf, bool onlyNotSynced)
		{
			if ((onlyNotSynced && !sf.IsSynced) && !sf.File.Exists || (!onlyNotSynced && !sf.File.Exists))
			{
				List<FileInfo> file = new List<FileInfo>();
				file = RecurseDirs(SourceDir, file);
				file = file.Where(f => f.Name == sf.File.Name).ToList();

				if (file.Any() && file.First().Exists)
					sf.File = file.First();
			}
		}

		public List<FileInfo> RecurseDirs(DirectoryInfo di, List<FileInfo> files)
		{
			foreach (DirectoryInfo sd in di.EnumerateDirectories())
			{
				RecurseDirs(sd, files);
			}

			files.AddRange(di.GetFiles().ToList());
			return files;
		}
		#endregion

		public bool HasValidRecord()
		{
			return ID != Guid.Empty;
		}

		public void Update(bool source, string dir)
		{
			try
			{
				DirectoryInfo info = new DirectoryInfo(dir);
				if (!info.Exists)
					return;

				if (source)
					SourceDir = new DirectoryInfo(dir);
				else
					DestinationDir = new DirectoryInfo(dir);
			}
			catch (Exception e)
			{
				Trace.WriteLine("FAILED TO UPDATE DIRECTORY " + e);
			}
		}

		/// <summary>
		/// Updates the copied file on the Destination folders CreationDate to the current time.
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public bool SetUpdatedDate(List<SyncPath> list)
		{
			try
			{
				list.ForEach(sp =>
				{
					string path = string.Format("{0}\\{1}", DestinationDir, sp.Name);
					if (File.Exists(path))
					{
						File.SetLastWriteTime(path, DateTime.Now);		//Reset the File Date to Now
						File.SetCreationTime(path, DateTime.Now);
					}
				});
				return true;
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
				return false;
			}
		}

		#region Files

		//Determines the Season&Episode on Add, not with SyncFile Contruction because we do not want to recalculate data when reading from the database
		public void AddFile(FileInfo fi)
		{
			SyncFile nsf = new SyncFile(fi);
			nsf.DetermineSeasonEpisode();

			List<SyncFile> missing = Files.Where(sf => sf.Season.HasValue && nsf.Season.HasValue && sf.Season.Value == nsf.Season.Value
																							&& sf.Episode.HasValue && nsf.Episode.HasValue && sf.Episode.Value == nsf.Episode.Value //Get Missing File for new File if Exist
																							&& sf.IsMissing).ToList();

			if (missing.Any())
				missing.First().UpdateFromMissingFile(nsf);
			else
				AddFile(nsf);
		}

		public void AddFile(SyncFile f)
		{
			Files.Add(f);
		}

		public List<SyncFile> GetSyncedFiles(DateTime? last)
		{
			return Files.Where(sf => sf.IsSynced && !sf.IsWatched && !sf.IsMissing
				&& sf.SyncDate != null && last.HasValue && sf.SyncDate >= last.Value.Subtract(TimeSpan.FromHours(2)))
				.OrderBy(sf => sf.Season).ThenBy(sf => sf.Episode).ToList();
		}
		/// <summary>
		/// Returns a File List where files are not Synced and does Exist on system
		/// </summary>
		/// <returns></returns>
		public List<SyncFile> GetNotSyncedFiles()
		{
			return Files.Where(sf => !sf.IsSynced && !sf.IsMissing).OrderBy(sf => sf.Season).ThenBy(sf => sf.Episode).ToList();
		}
		public List<SyncFile> GetNotSyncedOrMissingFiles()
		{
			List<SyncFile> f = Files.Where(sf => !sf.IsSynced || sf.IsMissing).ToList();
			return f.OrderBy(sf => sf.Season).ThenBy(sf => sf.Episode).ToList();
		}
		public List<SyncFile> GetWatchFiles()
		{
			List<SyncFile> list = new List<SyncFile>();
			int? season = MaxWatchedSeasson();
			if (!season.HasValue)
				return list;

			int? maxWatched = MaxWatchedEpisode(season.Value);
			if (!maxWatched.HasValue)
				maxWatched = 0;

			if (HasMissing(season.Value, maxWatched.Value))
				list.AddRange(MissingBefore(season.Value, maxWatched.Value));

			bool taketwo = false;
			if (HasEpisodesAfter(season.Value, maxWatched.Value)) //Has more Episodes in this Season
			{
				taketwo = MaxEpisode(season.Value) - maxWatched.Value > 2;
				List<int> nextEpisodes = NextEpisodes(season.Value, maxWatched.Value, taketwo ? 2 : 1);

				list.AddRange(nextEpisodes.Select(e => EpisodeFile(season.Value, e)).ToList());
			}

			if (HasSeasonsAfter(season.Value) && !taketwo) //Has more Seasons
			{
				int seasonAfter = SeasonAfter(season.Value);
				List<int> nextEpisodes = NextEpisodes(seasonAfter, MinEpisode(seasonAfter).Value - 1, 1);
				list.AddRange(nextEpisodes.Select(e => EpisodeFile(seasonAfter, e)));
			}

			return list.OrderBy(sf => sf.Season).ThenBy(sf => sf.Episode).ToList();
		}
		public List<SyncFile> GetNotWatchedFiles()
		{
			return Files.Where(sf => sf.IsSynced && !sf.IsWatched || sf.IsMissing).OrderBy(sf => sf.Season).ThenBy(sf => sf.Episode).ToList();
		}
		public List<SyncFile> GetErrorSyncFiles()
		{
			return Files.Where(sf => (!sf.IsSynced && (sf.FileDate.HasValue && (DateTime.Now.Subtract(sf.FileDate.Value)) > TimeSpan.FromDays(10)))
														|| sf.IsMissing
														|| (Error && !sf.IsSynced))

				.OrderByDescending(sf => sf.FileDate).ToList();
		}

		#endregion

		public override string ToString()
		{
			return Name;
		}

		public static bool StructureExits(List<DirectoryInfo> directories)
		{
			if (!directories.Any())
				return false;

			foreach (DirectoryInfo info in directories.Where(d => d != null && !d.RefreshExist()))
				Debug.WriteLine(string.Format("\nSTRUCTURE-EXISTS: DIRECTORY DOES NOT EXIST [{0}]", info.FullName));

			return directories.Any(d => d != null && d.Exists);
		}

		public static bool GetDriveWithStructure(List<SyncPath> spList, bool source)
		{
			try
			{
				string[] logicalDrives = Environment.GetLogicalDrives();
				foreach (string strDrive in logicalDrives)
				{
					DriveInfo drive = new DriveInfo(strDrive);
					if (!(drive.DriveType == DriveType.Fixed || drive.DriveType == DriveType.Removable))
						continue;

					List<DirectoryInfo> dirList = new List<DirectoryInfo>();
					spList.Select(sp => source ? sp.SourceDir : sp.DestinationDir).ToList().ForEach(d =>
					{
						try
						{
							string dir = d.FullName.Replace(d.Root.FullName, drive.Name);
							dirList.Add(new DirectoryInfo(dir));
						}
						catch (NotSupportedException e)
						{
							Trace.WriteLine(e);
							string dir = d.FullName.Replace(d.Root.Name, drive.Name);
						}
					});

					if (StructureExits(dirList))
					{
						spList.ForEach(sp =>
						{
							DirectoryInfo d = (source ? sp.SourceDir : sp.DestinationDir);
							string dir = d.FullName.Replace(d.Root.Name, drive.Name);
							sp.Update(source, dir);
						});
						return true;
					}
				}
			}
			catch (Exception e)
			{
				Trace.WriteLine("FAILED TO GET DRIVE WITH STRUCTURE " + e);
			}

			return false;
		}

		#region Status

		public void SetSyncFilesCompleted()
		{
			Files.ForEach(sf =>
			{
				string path = string.Format("{0}\\{1}", DestinationDir.FullName, sf.File.Name);
				if (File.Exists(path))
				{
					sf.SetSynced();
				}
				else
				{
					sf.SetError();
				}
			});
		}
		public void SetError()
		{
			Error = true;
		}
		public void SetStatus(SyncStatus status, string desc)
		{
			Status = status;
			ErrorDescription = string.IsNullOrEmpty(desc) ? StatusDescription[status] : desc;

			if (Status == SyncStatus.Error || Status == SyncStatus.FileError)
				SetError();
		}

		public string GetDirNotFoundMessage(bool source)
		{
			return source ? string.Format("Source Dir Not Found [{0}]", SourceDir.FullName) : string.Format("Destination Dir Not Found [{0}]", DestinationDir.FullName);
		}
		public string GetDirCreateMessage()
		{
			return string.Format("Destination Dir Created [{0}]", DestinationDir.FullName);
		}
		public string GetDirFailedCreateMessage()
		{
			return string.Format("Failed to Create Directory [{0}]", DestinationDir.FullName);
		}
		public string GetNotExistingFilesMessage()
		{
			return string.Format("File does not Exist in Source Dir [{0}] \nFiles: {1}",
				SourceDir,
				string.Join("\n", NonExistingFiles.Select(sf => sf.File.FullName)));
		}
		public string GetFailedCopyFilesMessage()
		{
			return string.Format("Files failed to Copy to: [{0}] \nFiles: {1}",
				DestinationDir.FullName,
				HasErrorFiles.Select(sf => sf.File.Name + "\n"));
		}

		#endregion
	}

	public class FileInfoComparer : IEqualityComparer<FileInfo>
	{
		public bool Equals(FileInfo x, FileInfo y)
		{
			return x.Name.ToLower() == y.Name.ToLower();
		}

		public int GetHashCode(FileInfo obj)
		{
			return obj.FullName.GetHashCode();
		}
	}
}
