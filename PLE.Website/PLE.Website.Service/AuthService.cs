using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PLE.Contract.DTOs;
using PLE.Contract.DTOs.Requests;

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

		public AuthService() {
			_client = new HttpClient { BaseAddress = new Uri(BaseUri) };
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<TokenDto> Login(string username, string password) {
			TokenDto token = null;
			var url = BaseUri + "oauth/token";
			using (var httpClient = new HttpClient()) {
				HttpContent content = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("grant_type", "password"),
					new KeyValuePair<string, string>("username", username),
					new KeyValuePair<string, string>("password", password)
				});
				var result = httpClient.PostAsync(url, content).Result;

				var resultContent = result.Content.ReadAsStringAsync().Result;

				token = JsonConvert.DeserializeObject<TokenDto>(resultContent);
			}

			return token;
		}

		public UserDto User(string username) {
			var request = new StringContent(JsonConvert.SerializeObject(new GetUserRequest {UserName = username}), Encoding.UTF8,
				"application/json");
			var response = _client.PostAsync("api/accounts/GetUser", request).Result;
			var resultContent = response.Content.ReadAsStringAsync().Result;
			var result = JsonConvert.DeserializeObject<UserDto>(resultContent);
			return result;
		}
	}

}
