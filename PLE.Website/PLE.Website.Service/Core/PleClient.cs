using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace PLE.Website.Service.Core
{
	public class PleClient
	{
		#region Fields
		private readonly HttpClient _client;
		private string defaultUri = "http://localhost/PLE.Service/";
		public Uri BaseUri {
			get => _client.BaseAddress;
			set => _client.BaseAddress = value;
		}
		#endregion

		#region Ctor
		public PleClient(Uri uri = null) {
			_client = new HttpClient { BaseAddress = uri ?? new Uri(defaultUri) };
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var authToken = Common.Common.Token?.access_token;
			UpdateToken(authToken);
		}

		public PleClient(string authToken, Uri uri = null) {
			_client = new HttpClient { BaseAddress = uri ?? new Uri(defaultUri) };
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			UpdateToken(authToken);
		}
		#endregion

		public T Get<T>(string url) {
			var response = _client.GetAsync(url).Result;
			var resultContent = response.Content.ReadAsStringAsync().Result;

			if (!response.IsSuccessStatusCode)
				throw new Exception(resultContent);

			var result = JsonConvert.DeserializeObject<T>(resultContent);
			return result;
		}

		public T Post<T>(string url, object content) {
			var request = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8,
				"application/json");
			var response = _client.PostAsync(url, request).Result;

			var resultContent = response.Content.ReadAsStringAsync().Result;
			if (!response.IsSuccessStatusCode)
				throw new Exception(resultContent);

			var result = JsonConvert.DeserializeObject<T>(resultContent);
			return result;
		}

		public void UpdateToken(string authToken) {
			if (!string.IsNullOrWhiteSpace(authToken))
				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
		}
	}
}
