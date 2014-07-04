using System;
using System.Collections.Generic;
using System.ServiceModel;
using Common;
using Core.Service;
using Models;

namespace MediaSync
{
	[ServiceContract]
	public interface IMediaSyncService : IDataService
	{
		[OperationContract]
		List<SyncPath> Domain_SelectAllSyncPath();

		[OperationContract]
		List<SyncPath> GetSyncPathCache();

		[OperationContract]
		List<SyncPath> Data_GetAllCollection();
	
		[OperationContract]
		List<SyncPath> Data_SyncedCollection();

		[OperationContract]
		List<SyncPath> Data_GetNotSyncedCollection();

		[OperationContract]
		List<SyncPath> Data_GetWatchCollection();

		[OperationContract]
		List<SyncPath> Data_GetNotWatchedCollection();

		[OperationContract]
		List<SyncPath> Data_GetErrorCollection();


		[OperationContract]
		void SubmitFilesWatch(string id, bool value);

		[OperationContract]
		void SubmitFileUpdate(SyncFile file);

		[OperationContract]
		void SubmitFileDelete(Guid id);
	}
}
