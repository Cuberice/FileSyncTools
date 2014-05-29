using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using WebGrease.Css.Extensions;

namespace SyncMobile.Models
{
	public class PathInformation
	{
		[Required]
		public bool IsPath { get; set; }

		[Required]
		public string PathName { get; set; }

		public int PathFileCount { get; set; }

		public IList<FileInformation> FileInformations { get; set; }

		public void SetAllowEdit(bool allowSync, bool allowWatch)
		{
			FileInformations.ForEach(si =>
			{
				si.AllowIsSyncEdit = allowSync;
				si.AllowIsWatchedEdit = allowWatch;
			});
		}
	}
}