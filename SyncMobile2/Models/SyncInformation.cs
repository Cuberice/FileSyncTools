using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SyncMobile.Models
{
	public class SyncInformation
	{
		[Required]
		public string FileGUID { get; set; }
		[Required]
		public bool IsPath { get; set; }

		[Required]
		[Display(Name = "Path Name: ")]
		public string PathName { get; set; }
		public int PathFileCount { get; set; }

		public string FileName { get; set; }
		public string FileDate { get; set; }

		[Display(Name = "Synced")]
		public bool IsSynced { get; set; }
		[Display(Name = "Watched")]
		public bool IsWatched { get; set; }
		[Display(Name = "Error")]
		public int Error { get; set; }
		public bool IsMissing { get; set; }

		public int Season { get; set; }
		public int Episode { get; set; }

		public bool AllowIsSyncEdit { get; set; }
		public bool AllowIsWatchedEdit { get; set; }
	}
}
