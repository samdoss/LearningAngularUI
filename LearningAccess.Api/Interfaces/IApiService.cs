﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningAccess.Api.Interfaces
{
	public interface IApiService
	{
		Task<IList<string>> GetValues();
	}
}
