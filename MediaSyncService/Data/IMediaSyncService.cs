﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
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
		List<SyncPath> Data_GetAllCollectionAmount(int amount);
	
		[OperationContract]
		List<SyncPath> Data_SyncedCollection();

		[OperationContract]
		List<SyncPath> Data_GetNotSyncedCollection();

		[OperationContract]
		List<SyncPath> Data_GetWatchCollection();

		[OperationContract]
		List<SyncPath> Data_GetNotSyncedCollection2();

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

		[OperationContract]
		void TestMethod(SyncFile f);
	}
}
