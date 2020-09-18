using System;
using System.Collections.Generic;
using System.Text;

namespace LearningAccess.DataAccess
{
	public class DBConnection
	{
		public DBConnection()
		{
			DefaultDB = "ConnectionStrings:LearningDB";
			DefaultanotherDB = "ConnectionStrings:LearningAnotherDB";
		}

		public string DefaultDB { get; set; }
		public string DefaultanotherDB { get; set; }

	}
}
