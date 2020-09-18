using System;

namespace LearningAccess.DataAccess
{
	
		public enum ScreenMode
		{
			Add = 1,
			Edit = 2,
			Delete = 3,
			View = 4,
			Exists = 5
		}

	public enum TransactionStatus
	{
		Success,
		Failure
	}

	public enum LogLevel
	{
		Fatal,
		Error,
		Warn,
		Info,
		Debug
	}
	
}
