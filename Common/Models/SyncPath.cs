using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Common;
using Core;
using Core.Data;

namespace Models
{
	[Table("TBL_SYNCPATH")]
	[DataContract]
	public partial class SyncPath : ITestObject
	{
		public SyncPath()
		{
			//Default Constructor for Model
		}

		/// <summary>
		/// Used for Reading the Path data from the Database
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="sourceDir"></param>
		/// <param name="destDir"></param>
		/// <param name="tvDbId"></param>
		public SyncPath(Guid id, string name, string sourceDir, string destDir, int? tvDbId) 
		{
			Files = new List<SyncFile>();
			SourceDir = string.IsNullOrEmpty(sourceDir) ? null : new DirectoryInfo(sourceDir);
			DestinationDir = new DirectoryInfo(destDir);
			Status = SyncStatus.New;
			TvDbID = tvDbId;

			Name = string.IsNullOrEmpty(name) ? SourceDir.FullName.Replace(SourceDir.Root.Name, "").Replace('\\', '.') : name;
			ID = id;
		}

		public SyncPath(string name, string sourceDir, string destDir) : this(Guid.NewGuid(), name, sourceDir, destDir, null)
		{
		}
	
		#region Properties

		[DataMember]
		[Column("ID", Column.DataType.Guid, PrimaryKey = true)]
		public Guid ID { get; set; }

		[DataMember]
		[Column("NAME", Column.DataType.String, NotNull = true)]
		public string Name { get; set; }

		[DataMember]
		[Column("SOURCEPATH", Column.DataType.String)]
		public DirectoryInfo SourceDir { get; set; }

		[DataMember]
		[Column("DESTINATIONPATH", Column.DataType.String)]
		public DirectoryInfo DestinationDir { get; set; }

		[DataMember]
		[Column("ERROR", Column.DataType.Boolean)]
		public bool Error { get; set; }

		[DataMember]
		[Column("TVDBID", Column.DataType.Integer)]
		public int? TvDbID { get; set; }

		[DataMember]
		[Containment(typeof(SyncFile), "SYNCPATHID")]
		public List<SyncFile> Files { get; set; }

		[DataMember]
		public SyncStatus Status { get; set; }
		[DataMember]
		public string ErrorDescription { get; set; } //Status Description
		#endregion
			
		#region Enums

		public enum SyncStatus
		{
			/// <summary>
			/// No Status is set
			/// </summary>
			New,

			/// <summary>
			/// Path was Passed TeraCopy
			/// </summary>
			Copied,

			/// <summary>
			/// Files in the SyncPath was skipped because File is Locked.
			/// </summary>
			FileError,

			/// <summary>
			/// Some Error Occured, Either copy failed or some event prevented copy to proceed successfully
			/// </summary>
			Error,

			/// <summary>
			/// A Update of this Path is available - new files found 
			/// </summary>
			UpdateAvailable,

			/// <summary>
			/// The Destination Directory has been Created
			/// </summary>
			DirInfo
		}

		public static readonly Dictionary<SyncStatus, Color> StatusColor = new Dictionary<SyncStatus, Color>
		{
			{SyncStatus.New, Color.White},
			{SyncStatus.Copied, Color.LightGreen},
			{SyncStatus.FileError, Color.Orange},
			{SyncStatus.Error, Color.IndianRed},
			{SyncStatus.UpdateAvailable, Color.LightBlue},
			{SyncStatus.DirInfo, Color.MediumPurple}
		};

		public static readonly Dictionary<SyncStatus, string> StatusDescription = new Dictionary<SyncStatus, string>
		{
			{SyncStatus.New, "None"},
			{SyncStatus.Copied, "Files Copied"},
			{SyncStatus.FileError, "File Error"},
			{SyncStatus.Error, "Error Occured"},
			{SyncStatus.UpdateAvailable, "Update Available"},
			{SyncStatus.DirInfo, "New Destination Directory Created"}
		};

		public Dictionary<SyncStatus, int> StatusPriority = new Dictionary<SyncStatus, int>
		{
			{SyncStatus.New, 1},
			{SyncStatus.Copied, 2},
			{SyncStatus.UpdateAvailable, 3},
			{SyncStatus.FileError, 4},
			{SyncStatus.DirInfo, 5},
			{SyncStatus.Error, 6},
		};

		#endregion

		public object CreateTestObject(params object[] parameters)
		{
			ID = Guid.NewGuid();
			Name = Extentions.GetRandomString(20);
			Files = ModelExtensions.CreateTestInstances<SyncFile>(1000, ID);
			SourceDir = new DirectoryInfo(@"C:\Stuff\SyncTest\Destination");
			DestinationDir = new DirectoryInfo(@"C:\Stuff\SyncTest\Destination");
			return this;
		}
	}
}
