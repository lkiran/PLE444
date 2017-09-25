using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PLE.Contract.DTOs;
using PLE.Contract.DTOs.Requests;
using PLE.Website.Service.Core;

namespace PLE.Website.Service
{
	public class AuthService
	{
		private readonly PleClient _client;

		public AuthService(string token) {
			_client = new PleClient(token);
		}

		public async Task<TokenDto> GetAuthToken(string username, string password) {
			const string url = "http://localhost:54020/oauth/token";
			using (var httpClient = new HttpClient()) {
				HttpContent content = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("grant_type", "password"),
					new KeyValuePair<string, string>("username", username),
					new KeyValuePair<string, string>("password", password)
				});
				var result = httpClient.PostAsync(url, content).Result;
				var resultContent = result.Content.ReadAsStringAsync().Result;

				return JsonConvert.DeserializeObject<TokenDto>(resultContent);
			}
		}

		public UserDto User(string username) {
			var result = _client.Post<UserDto>("api/accounts/GetUser", new GetUserRequest { UserName = username });
			return result;
		}
	}

}
