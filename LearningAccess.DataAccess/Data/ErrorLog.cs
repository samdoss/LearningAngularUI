using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LearningAccess.DataAccess
{
	public static class ErrorLog
	{
		public static void LogErrorMessageToDB(string pageName, string className, string eventName, string errorMessage)
		{
			try
			{
				Logging.LogErrorMessageToDB(pageName, className, eventName, errorMessage);
			}
			catch { }
		}
	}
}
