using PLE.Website.Service.Core;

namespace PLE.Website.Service
{
	public class BaseService
	{
		protected readonly PleClient Client;

		public BaseService() {
			Client = new PleClient();
		}
	}
}