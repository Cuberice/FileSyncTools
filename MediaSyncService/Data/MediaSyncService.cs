using System.Collections.Generic;
using Common;
using Core.Service;

namespace MediaSync
{
	public class MediaSyncService : DataService, IMediaSyncService
	{
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
	}
}
