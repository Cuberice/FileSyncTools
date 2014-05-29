using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SyncMobile.Models
{
	public class FileInformation
	{
		[Required]
		public string FileGUID { get; set; }
		[Required]
		public string FileName { get; set; }
		
		[Display(Name = "Synced")]
		public bool IsSynced { get; set; }
		[Display(Name = "Watched")]
		public bool IsWatched { get; set; }
		[Display(Name = "Error")]
		public int Error { get; set; }
		public bool IsMissing { get; set; }

		public string FileDate { get; set; }
		public int Season { get; set; }
		public int Episode { get; set; }

		public bool AllowIsSyncEdit { get; set; }
		public bool AllowIsWatchedEdit { get; set; }
	}
}