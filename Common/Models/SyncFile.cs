using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Core;
using Core.Data;

namespace Models
{
	[Table("TBL_SYNCDATA")]
	public class SyncFile : ITestObject
	{
		public SyncFile()
		{
		}

		[Column("ID", Column.DataType.Guid, PrimaryKey = true)]
		public Guid ID { get; set; }

		[Column("SYNCPATHID", Column.DataType.Guid, NotNull = true)]
		public Guid SyncPathId { get; set; }

		[Column("FILENAME", Column.DataType.String, NotNull = true)]
		public string FileName { get; set; }

		[Column("SYNCDATE", Column.DataType.DateTime)]
		public DateTime? SyncDate { get; set; }				//Date Synced

		[Column("FILEDATE", Column.DataType.DateTime)]
		public DateTime? FileDate { get; set; }				//Download Complete

		[Column("WATCHDATE", Column.DataType.DateTime)]
		public DateTime? WatchDate { get; set; }

		[Column("SEASON", Column.DataType.Integer)]
		public int? Season { get; set; }

		[Column("EPISODE", Column.DataType.Integer)]
		public int? Episode { get; set; }

		[Column("ISSYNCED", Column.DataType.Boolean)]
		public bool IsSynced { get; set; }						//New Synched files

		[Column("ISWATCHED", Column.DataType.Boolean)]
		public bool IsWatched { get; set; }

		[Column("ISMISSING", Column.DataType.Boolean)]
		public bool IsMissing { get; set; }

		[BackReference("SOURCEPATH")]
		public DirectoryInfo Directory { get; set; }

		public FileInfo file;
		public FileInfo File
		{
			get
			{
				if (file.IsNull())
					file = new FileInfo(Path.Combine(Directory.FullName, FileName));

				return file;
			}
		}

		public bool AllowIsSyncEdit { get; set; }
		public bool AllowIsWatchedEdit { get; set; }
		public int Error { get; set; }

		#region Construction

		//Used with Reading Data from the database
		public SyncFile(Guid id, DateTime? syncdate, DateTime? filedate, bool issynced, bool iswatched, DateTime? watchdate,
			bool ismissing, int season, int episode, string filepath)
		{
			ID = id;
			SyncDate = syncdate;
			FileDate = filedate;
			IsSynced = issynced;
			IsWatched = iswatched;
			Season = season;
			Episode = episode;
			IsMissing = ismissing;
			WatchDate = watchdate;
			file = new FileInfo(filepath);
		}

		//Used with SyncInformation swopping for View
		public SyncFile(string guid, bool iswatched, bool isSynced, int season, int episode, DateTime? watchdate)
		{
			ID = Guid.Parse(guid);
			IsWatched = iswatched;
			WatchDate = watchdate;
			IsSynced = isSynced; //Dependent on AllowIsSyncEdit - otherwize view returns false for checkbox
			Season = season;
			Episode = episode;
		}

		//Used with Watch Ajax Call
		public SyncFile(string guid, bool iswatched)
		{
			ID = Guid.Parse(guid);
			IsWatched = iswatched;
			WatchDate = iswatched ? DateTime.Now : (DateTime?) null;
		}

		//Used with Update Ajax Call
		public SyncFile(string guid, bool isSynced, bool isMissing, int season, int episode)
		{
			ID = Guid.Parse(guid);
			IsSynced = isSynced;
			IsMissing = isMissing;
			Season = season;
			Episode = episode;
		}

		//Used with Adding new Files
		public SyncFile(FileInfo fi)
		{
			ID = Guid.NewGuid();
			file = fi;
			FileDate = fi.CreationTime;
		}

		public SyncFile(SyncPath sp, int season, int episode, bool ismissing)
		{
			ID = Guid.NewGuid();
			Season = season;
			Episode = episode;
			IsMissing = ismissing;
			file = new FileInfo(string.Format("MissingFile.{2}.S{0}E{1}", Season, Episode, sp.Name.Replace(" ", "")));
		}

		#endregion
		
		public void UpdateFromMissingFile(SyncFile syncFile)
		{
			IsMissing = false;
			file = syncFile.File;
			FileDate = syncFile.File.CreationTime;
		}

		public void SetError()
		{
			Error = 1;
		}

		public void SetSynced()
		{
			IsSynced = true;
			SyncDate = DateTime.Now;
		}

		public void DetermineSeasonEpisode()
		{
			List<string> regexes = new List<string>() { @"S(\d{1,2})E(\d{1,2})", @"(\d{1,2})x(\d{1,2})", @".(\d\d)(\d{1,2}).hdtv", @".(\d\d)(\d{1,2}).X264", @".(\d)(\d{1,2}).hdtv", @".(\d)(\d{1,2}).X264", @"(\d)(\d\d)" };
			Season = Episode = 0;
			int season = 0, episode = 0;
			foreach (string r in regexes)
			{
				Regex regex_SddEdd = new Regex(r, RegexOptions.IgnoreCase);
				Match match = regex_SddEdd.Match(File.Name);

				if (!match.Success)
					continue;

				string s = match.Groups[1].Value;
				string e = match.Groups[2].Value;

				if (!(Int32.TryParse(s, out season) && Int32.TryParse(e, out episode)))
					continue;

				Season = season;
				Episode = episode;
				break;
			}
		}

		public override string ToString()
		{
			return string.Format("{0}: [{1}-{2}]", File.Name, Season, Episode);
		}

		public object CreateTestObject(params object[] parameters)
		{
			ID = Guid.NewGuid();
			SyncPathId = (Guid) parameters.First();
			FileName = Extentions.GetRandomString(30);
			SyncDate = Extentions.GetRandomDate();
			FileDate = Extentions.GetRandomDate();
			Season = Extentions.GetRandomInt();
			Episode = Extentions.GetRandomInt();
			IsSynced = Extentions.GetRandomBool();
			IsWatched = Extentions.GetRandomBool();
			IsMissing = Extentions.GetRandomBool();

			Directory = new DirectoryInfo(@"C:\Stuff\SyncTest\Destination");
			return this;
		}
	}
}
