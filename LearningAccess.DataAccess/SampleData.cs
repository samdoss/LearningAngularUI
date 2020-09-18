using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace LearningAccess.DataAccess
{
	public class BookRequest
	{
		public int BookID { get; set; }		
	}

	public interface IBookRepository
	{
		DataTable GetBook(int bookID);
		List<BookResponse> GetBooks();
	}

	public class BookResponse
	{
		public string bookName { get; set; }
		public int bookID { get; set; }
		public DateTime purchasedDate { get; set; }
		public string errorMessage { get; set; }
	}

	public class BookRepository : IBookRepository, IDisposable
	{
		#region Variables

		protected DBConnection dbConnection = new DBConnection();
		public BookResponse book;
		public List<BookResponse> books = new List<BookResponse>();

		#endregion

		public DataTable GetBook(int bookID)
		{
			try
			{
				DatabaseProviderFactory factory = new DatabaseProviderFactory();
				Database db = DatabaseFactory.CreateDatabase(dbConnection.DefaultanotherDB);
				string sqlCommand = "spGetBook";
				DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
				db.AddInParameter(dbCommand, "BookID", DbType.Int32, bookID);
				dbCommand.CommandTimeout = 300;
				DataSet ds = db.ExecuteDataSet(dbCommand);
				return ds.Tables[0];
			}
			catch(Exception ex)
			{
				ErrorLog.LogErrorMessageToDB("SampleData", "BookRepository", "GetBook", ex.Message);
				throw;
			}
		}

		public List<BookResponse> GetBooks()
		{
			try
			{
				DatabaseProviderFactory factory = new DatabaseProviderFactory();
				Database db = DatabaseFactory.CreateDatabase(dbConnection.DefaultanotherDB);
				string sqlCommand = "spGetBooksList";
				DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);				
				dbCommand.CommandTimeout = 300;
				DataSet ds = db.ExecuteDataSet(dbCommand);
				var bookData = ds.Tables[0];

				List<BookResponse> responsesList = new List<BookResponse>();

				if(bookData.Rows.Count > 0)
				{
					foreach (DataRow item in bookData.Rows)
					{
						BookResponse response = new BookResponse();
						response.bookID = Common.CheckIntNull(item["BookID"]);
						response.bookName = Common.CheckNull(item["BookName"]);

						responsesList.Add(response);
					}
				}

				return responsesList;
			}
			catch (Exception ex)
			{
				ErrorLog.LogErrorMessageToDB("SampleData", "BookRepository", "GetBook", ex.Message);
				throw;
			}
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
