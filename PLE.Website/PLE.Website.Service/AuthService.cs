using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PLE.Website.Service
{
	public class TokenRequest
	{
		public string grant_type { get; set; } = "password";
		public string username { get; set; } = "leventkran@gmail.com";
		public string password { get; set; } = "Asd'12";

	}

	public class AuthService
	{
		private const string BaseUri = "http://localhost:54020/";
		private HttpClient _client;

		public AuthService()
		{
			_client = new HttpClient { BaseAddress = new Uri(BaseUri) };
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<string> Login(string username, string password)
		{
			var client = new HttpClient { BaseAddress = new Uri(BaseUri) };
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
			var theContent = new StringContent($"grant_type=password&username={username}&password={password}", Encoding.UTF8, "application/x-www-form-urlencoded");
			var response = await client.PostAsync("oauth/token", theContent);
			var content = await response.Content.ReadAsStringAsync();
			return content;
		}

		public async Task<string> Users()
		{
			//var json =JsonConvert.SerializeObject(new TokenRequest(), Formatting.Indented);
			var response = await _client.GetAsync("api/accounts/users");
			var content = await response.Content.ReadAsStringAsync();
			return content;
		}
	}

}
