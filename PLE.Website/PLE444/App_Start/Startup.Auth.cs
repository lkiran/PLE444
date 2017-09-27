using AutoMapper;
using Owin;
using PLE444.Models;

namespace PLE444
{
	public partial class Startup
	{
		public void ConfigureAuth(IAppBuilder app) {
			Mapper.Initialize(cfg => cfg.AddProfile<PleWebsiteMappingProfile>());
		}
	}
}