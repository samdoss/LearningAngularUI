using System;
using System.Collections.Generic;
using System.Text;

namespace LearningAccess.DataAccess
{
	public class TransactionResult
	{
		#region Constructor(s)

		//Constructor taking the TransactionStatus Enum
		public TransactionResult(TransactionStatus status)
		{
			Status = status;
		}

		// Constructor taking the TransactionStatus enum, and an Message for the client to inspect
		public TransactionResult(TransactionStatus status, string message)
		{
			Status = status;
			Message = message;
		}

		#endregion

		#region Public Properties

		public string Message { get; set; }
		public TransactionStatus Status { get; set; }

		#endregion

		
	}
}
