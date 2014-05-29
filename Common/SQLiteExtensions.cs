using System;
using System.Data.SQLite;

namespace Common
{
	public static class SQLiteExtensions
	{
		public static string GetString(this SQLiteDataReader reader , string column)
		{
			return reader[column].ToString();
		}		
		
		public static Guid GetGuid(this SQLiteDataReader reader , string column)
		{
			return Guid.Parse(reader.GetString(column));
		}

		public static bool GetBoolean(this SQLiteDataReader reader, string column)
		{
			if (reader[column] == null || reader[column] == DBNull.Value)
				return false;

			return Boolean.Parse(reader[column].ToString());
		}

		public static int GetInt32(this SQLiteDataReader reader, string column)
		{
			if (reader[column] == null || reader[column] == DBNull.Value)
				return 0;

			return Int32.Parse(reader[column].ToString());
		}		
		
		public static int? GetNullableInt32(this SQLiteDataReader reader, string column)
		{
			if (reader[column] == null || reader[column] == DBNull.Value)
				return null;

			return Int32.Parse(reader[column].ToString());
		}

		public static DateTime? GetDateTime(this SQLiteDataReader reader, string column)
		{
			try
			{
				string s = reader[column].ToString();
				if (s == string.Empty)
					return null;

				return DateTime.Parse(s);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return DateTime.MinValue;
			}
		}
	}
}
