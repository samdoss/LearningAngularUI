using LearningAccess.Api.Interfaces;
using LearningAccess.Api.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LearningAccess.Api.Services
{
	public class SecondTokenService : ITokenService
	{
		private AuthorizationToken token = new AuthorizationToken();
		private readonly IOptions<SecondAuthorizationSettings> authorizeSettings;

		public SecondTokenService(IOptions<SecondAuthorizationSettings> authorizeSettings)
		{
			this.authorizeSettings = authorizeSettings;
		}

		public async Task<string> GetToken()
		{
			if (!this.token.IsValidAndNotExpiring)
			{
				this.token = await this.GetAccessToken();
			}
			return token.AccessToken;
		}

		private async Task<AuthorizationToken> GetAccessToken()
		{
			var token = new AuthorizationToken();
			var client = new HttpClient();
			var client_id = this.authorizeSettings.Value.ClientId;
			var client_secret = this.authorizeSettings.Value.ClientSecret;
			var clientCreds = System.Text.Encoding.UTF8.GetBytes($"{client_id}:{client_secret}");
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(clientCreds));

			var postMessage = new Dictionary<string, string>();
			postMessage.Add("grant_type", this.authorizeSettings.Value.GrantType);
			postMessage.Add("scope", this.authorizeSettings.Value.Scope);

			var request = new HttpRequestMessage(HttpMethod.Post, this.authorizeSettings.Value.TokenURL)
			{
				Content = new FormUrlEncodedContent(postMessage)
			};

			var response = await client.SendAsync(request);
			if (response.IsSuccessStatusCode)
			{
				var json = await response.Content.ReadAsStringAsync();
				this.token = JsonConvert.DeserializeObject<AuthorizationToken>(json);
				this.token.ExpiresAt = DateTime.UtcNow.AddSeconds(this.token.ExpiresIn);

			}
			else
			{
				throw new ApplicationException("Unable to retrieve access token from API");
			}

			return token;
		}
	}
}
