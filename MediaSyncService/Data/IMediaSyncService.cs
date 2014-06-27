using System.Collections.Generic;
using System.ServiceModel;
using Common;

namespace MediaSync
{
	[ServiceContract]
	public interface IMediaSyncService
	{
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
	}
}
