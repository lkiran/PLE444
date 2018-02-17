using System;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using PLE.Contract.DTOs;
using PLE.Website.Service.Core;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.InteropServices;
using PLE.Contract.DTOs.Responses;

namespace PLE.Website.Service {
	public class AuthService : IDisposable {
		private readonly PleClient _client;
		private const string AuthTokenName = "PLE_Auth_Token";

		public AuthService() {
			_client = new PleClient();
		}

		#region Auth Token
		public TokenDto GetAuthToken(string username, string password) {
			var url = ConfigurationManager.AppSettings["AuthTokenUrl"];

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

		public void UpdateClientToken(TokenDto token) {
			Common.Common.Token = token;
			_client.UpdateToken(token.access_token);
		}
		#endregion

		public UserDto GetActiveUser() {
			var result = _client.Get<UserDto>("api/accounts/User");
			result.Token = Common.Common.Token;
			return result;
		}

		public UserDto GetUser(string userId = "", string email = "") {
			var result = _client.Get<UserDto>($"api/accounts/User?userId={userId}&email={email}");
			return result;
		}

		public RegisterUserResponseDto RegisterUser(UserDto user) {
			var result = _client.Post<RegisterUserResponseDto>("api/accounts/User", user);
			return result;
		}

		#region Email Verification
		public string GetEmailVerificationCode(string userId) {
			var result = _client.Get<string>($"api/accounts/EmailVerificationCode?userId={userId}");
			return result;
		}

		public bool VerifyEmail(string userId, string code) {
			var result = _client.Get<bool>($"api/accounts/ConfirmEmail?userId={userId}&code={code}");
			return result;
		}

		public bool IsEmailConfirmed(string userId) {
			var result = _client.Get<bool>($"api/accounts/IsEmailConfirmed?userId={userId}");
			return result;
		}
		#endregion


		#region Forgot Password 
		public string GetPasswordResetCode(string userId) {
			var result = _client.Get<string>($"api/accounts/PasswordResetCode?userId={userId}");
			return result;
		}

		public bool ResetPassword(string userId, string newPassword, string code) {
			var result = _client.Get<bool>($"api/accounts/PasswordReset?userId={userId}&code={code}&newPassword={newPassword}");
			return result;
		}
		#endregion

		#region Cookie
		public void SetAuthCookie([Optional]TokenDto token) {
			if (token == null)
				token = Common.Common.Token;

			HttpContext.Current.Response.Cookies.Add(new HttpCookie(AuthTokenName) {
				Value = JsonConvert.SerializeObject(token),
				Expires = DateTime.Now + TimeSpan.FromSeconds(token.expires_in)
			});
		}

		public TokenDto GetAuthCookie() {
			var cookie = HttpContext.Current.Request.Cookies.Get(AuthTokenName);
			return cookie == null ? null : JsonConvert.DeserializeObject<TokenDto>(cookie.Value);
		}

		public void DeleteAuthCookie() {
			var cookie = HttpContext.Current.Request.Cookies.Get(AuthTokenName);
			if (cookie == null) return;
			cookie.Expires = DateTime.Now.AddDays(-1);
			HttpContext.Current.Response.Cookies.Add(cookie);
		}

		public bool CheckAuthCookieExist() => HttpContext.Current.Request.Cookies.Get(AuthTokenName) != null;

		#endregion

		public void Dispose() { }
	}
}