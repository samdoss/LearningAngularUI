using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningAccess.Api.Models
{
	public class FirstAuthorizationSettings
	{
		public string TokenURL { get; set; }

		public string ClientId { get; set; }

		public string ClientSecret { get; set; }

		public string GrantType { get; set; }

		public string Scope { get; set; }

		public string Authority { get; set; }
	}
}
