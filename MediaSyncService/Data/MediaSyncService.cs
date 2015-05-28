using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;
using Common;
using Core;
using Core.Data;
using Core.Service;
using Models;

namespace MediaSync
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class MediaSyncService : DataService, IMediaSyncService
	{
		protected List<SyncPath> CachedPaths { get { return Cache.ContainsKey("SyncPath") ? Cache["SyncPath"].OfType<SyncPath>().ToList() : null; } } 

		public List<SyncPath> Domain_SelectAllSyncPath()
		{
			return SelectForModel<SyncPath>();
		}		
		public List<SyncPath> GetSyncPathCache()
		{
			if (CachedPaths.IsNull())
			{
				IList list = Domain_SelectAllSyncPath();
				Cache.Add("SyncPath", list);
			}

			return CachedPaths;
		}

		public List<SyncPath> Data_GetAllCollection()
		{
			IList<SyncPath> list = Domain_SelectAllSyncPath();
			foreach (SyncPath sp in list)
			{
				sp.SetFiles(() => sp.Files.OrderBy(sf => sf.Season).ThenBy(sf => sf.Episode).ToList());
			}
			return list.Where(sp => sp.Files.Any()).OrderBy(sp => sp.Name).ToList();
		}
		public List<SyncPath> Data_GetAllCollectionAmount(int amount)
		{
			List<SyncPath> data = Data_GetAllCollection();
			return data.Take(amount).ToList();
		}

		public List<SyncPath> Data_SyncedCollection()
		{
			List<SyncPath> list = Domain_SelectAllSyncPath();
			DateTime? last = SyncUtils.GetLastSyncDate(list);

			list = last.HasValue ? list.Where(sp => sp.LastSyncDate >= last.Value.Subtract(TimeSpan.FromHours(2))).ToList() : list;
			foreach (SyncPath sp in list)
			{
				sp.SetFiles(() => sp.GetSyncedFiles(last));
			}

			return list.Where(sp => sp.Files.Any()).OrderByDescending(sp => sp.LastSyncDate).ToList();
		}		
		public List<SyncPath> Data_GetNotSyncedCollection()
		{
			List<SyncPath> list = Domain_SelectAllSyncPath();
			foreach (SyncPath sp in list)
			{
				sp.SetFiles(() => sp.GetNotSyncedOrMissingFiles());
			}

			return list.Where(sp => sp.Files.Any()).OrderByDescending(sp => sp.LastFileDate).ToList();
		}		
		public List<SyncPath> Data_GetNotSyncedCollection2()
		{
			Table tblSyncPath = Table.Get(typeof(SyncPath));

//			Command cmd = new Command();
//			cmd.AddExpression(tblSyncPath.GetColumn("TvDbID"), OperatorType.Equal, 72227);				
//			IEnumerable list = SelectForModel<SyncPath>(cmd);			
			
			Command cmd2 = new Command();
			cmd2.AddExpression(tblSyncPath.GetColumn("TvDbID"), OperatorType.In, new List<object> { 72227, 248880 });				
			IEnumerable list2 = SelectForModel<SyncPath>(cmd2);

			return new List<SyncPath>();
		}
		public List<SyncPath> Data_GetWatchCollection()
		{
			List<SyncPath> list = Domain_SelectAllSyncPath();
			foreach (SyncPath sp in list)
			{
				sp.SetFiles(() => sp.GetWatchFiles());
			}

			return list.Where(sp => sp.Files.Any()).OrderBy(sp => sp.LastWatchDate).ToList();
		}
		public List<SyncPath> Data_GetNotWatchedCollection()
		{
			List<SyncPath> list = Domain_SelectAllSyncPath();
			foreach (SyncPath sp in list)
			{
				sp.SetFiles(() => sp.GetNotWatchedFiles());
			}

			return list.Where(sp => sp.Files.Any()).OrderBy(sp => sp.LastSyncDate).ToList();
		
		}
		public List<SyncPath> Data_GetErrorCollection()
		{
			List<SyncPath> list = Domain_SelectAllSyncPath();
			foreach (SyncPath sp in list)
			{
				sp.SetFiles(() => sp.GetErrorSyncFiles());
			}

			return list.Where(sp => sp.Files.Any()).OrderBy(sp => sp.Name).ToList();
		}

		public void SubmitFilesWatch(string id, bool value)
		{
			throw new NotImplementedException();
		}
		public void SubmitFileUpdate(SyncFile file)
		{
			throw new NotImplementedException();
		}
		public void SubmitFileDelete(Guid id)
		{
			throw new NotImplementedException();
		}

		public void TestMethod(SyncFile f)
		{
			
		}
	}
}
