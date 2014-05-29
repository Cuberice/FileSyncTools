using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGrease.Css.Extensions;

namespace SyncMobile.Models
{
	public class PathGroup
	{
		public IList<PathInformation> PathInformations { get; set; }

		public void SetAllowEdit(bool allowSync, bool allowWatch)
		{
			PathInformations.ForEach(pi => pi.SetAllowEdit(allowSync, allowWatch));
		}
	}
}