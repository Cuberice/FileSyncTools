using System.Collections.Generic;
using System.ServiceModel;

namespace MediaSync
{
	[ServiceContract]
	public interface IMediaSyncService
	{
		[OperationContract]
		List<string> GetAllUsers();
	}
}
