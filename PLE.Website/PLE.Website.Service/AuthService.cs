using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PLE.Contract.DTOs;
using PLE.Contract.DTOs.Requests;
using PLE.Contract.DTOs.Responses;
using PLE.Website.Service.Core;

namespace PLE.Website.Service
{
	public class AuthService
	{
		private readonly PleClient _client;

		public AuthService() {
			_client = new PleClient();
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

				var token = JsonConvert.DeserializeObject<TokenDto>(resultContent);
				if (token?.access_token == null)
					throw new Exception("Access token is Null");
				return token;
			}
		}

		public UserDto GetActiveUser() {
			var result = _client.Get<UserDto>("api/accounts/User");
			return result;
		}

		public UserDto GetUser(string userId = "") {
			var result = _client.Get<UserDto>($"api/accounts/User?userId={userId}");
			return result;
		}

		public RegisterUserResponseDto RegisterUser(UserDto user) {
			var result = _client.Post<RegisterUserResponseDto>("api/accounts/User", user);
			return result;
		}
	}
}
