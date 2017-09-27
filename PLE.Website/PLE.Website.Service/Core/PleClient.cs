using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace PLE.Website.Service.Core
{
	public class PleClient
	{
		private readonly HttpClient _client;
		private const string BaseUri = "http://localhost:54020/";

		public PleClient(string AuthToken) {
			_client = new HttpClient { BaseAddress = new Uri(BaseUri) };
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			if (!string.IsNullOrWhiteSpace(AuthToken))
				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);
		}

		public T Post<T>(string url, object content) {
			var request = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8,
				"application/json");
			var response = _client.PostAsync(url, request).Result;
			var resultContent = response.Content.ReadAsStringAsync().Result;
			var result = JsonConvert.DeserializeObject<T>(resultContent);
			return result;
		}

		public T Get<T>(string url) {
			var response = _client.GetAsync(url).Result;
			var resultContent = response.Content.ReadAsStringAsync().Result;
			var result = JsonConvert.DeserializeObject<T>(resultContent);
			return result;
		}
	}
}
