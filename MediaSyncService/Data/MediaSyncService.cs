using System;
using System.Collections.Generic;
using Common;
using Core.Service;
using Models;

namespace MediaSync
{
	public class MediaSyncService : DataService, IMediaSyncService
	{
		public List<SyncPath> Domain_GetAllForSyncPath()
		{
			return SelectForModel<SyncPath>();
		}

		public List<SyncPath> Data_GetAllCollection()
		{
			throw new System.NotImplementedException();
		}

		public List<SyncPath> Data_SyncedCollection()
		{
			throw new System.NotImplementedException();
		}

		public List<SyncPath> Data_GetNotSyncedCollection()
		{
			throw new System.NotImplementedException();
		}

		public List<SyncPath> Data_GetWatchCollection()
		{
			throw new System.NotImplementedException();
		}

		public List<SyncPath> Data_GetNotWatchedCollection()
		{
			throw new System.NotImplementedException();
		}

		public List<SyncPath> Data_GetErrorCollection()
		{
			throw new System.NotImplementedException();
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
	}
}
