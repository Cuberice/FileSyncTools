using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGrease.Css.Extensions;

namespace SyncMobile.Models
{
	public class SyncGroup
	{
		public IList<SyncInformation> SyncInformations { get; set; }

		public void AllowEditWatch()
		{
			SyncInformations.ForEach(si =>
			{
				si.AllowIsWatchedEdit = true;
			});
		}
	}
}