using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningAccess.Api.Models
{
	public class AuthorizationToken
	{
		[JsonProperty(PropertyName = "access_token")]
		public string AccessToken { get; set; }

		[JsonProperty(PropertyName = "expires_in")]
		public int ExpiresIn { get; set; }

		public DateTime ExpiresAt { get; set; }

		[JsonProperty(PropertyName = "token_type")]
		public string TokenType { get; set; }

		public string Scope { get; set; }

		public bool IsValidAndNotExpiring
		{
			get
			{
				return !String.IsNullOrEmpty(this.AccessToken) && this.ExpiresAt > DateTime.UtcNow.AddSeconds(30);
			}
		}
	}
}
