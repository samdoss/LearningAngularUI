using LearningAccess.Api.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LearningAccess.Api.Services
{
	public class DefaultApiService : IApiService
	{
		private HttpClient client = new HttpClient();
		private readonly ITokenService tokenService;

		public DefaultApiService(ITokenService tokenService)
		{
			this.tokenService = tokenService;
		}

		public async Task<IList<string>> GetValues()
		{
			List<string> values = new List<string>();

			var token = tokenService.GetToken();
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.ToString());
			var res = await client.GetAsync("http://localhost/api/default");
			if(res.IsSuccessStatusCode)
			{
				var json = res.Content.ReadAsStringAsync().Result;
				values = JsonConvert.DeserializeObject<List<string>>(json);

			}
			else
			{
				values = new List<string>
				{
					res.StatusCode.ToString(), res.ReasonPhrase
				};
			}

			return values;
		}
	}
}
