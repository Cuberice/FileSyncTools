using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace Common
{
	public static class Queries
	{
		#region Strings

		public const string Select_Path_All = "SELECT ID, NAME, SOURCEPATH, DESTINATIONPATH, TvDbID FROM TBL_SYNCPATH";

		public const string Select_Path = "SELECT ID FROM TBL_SYNCPATH WHERE SOURCEPATH LIKE '{0}'";
		public const string Select_PathDest = "SELECT ID FROM TBL_SYNCPATH WHERE DESTINATIONPATH LIKE '{0}'";

		public const string Insert_Path = "INSERT INTO TBL_SYNCPATH(ID, NAME, SOURCEPATH, DESTINATIONPATH, ERROR)	VALUES(@id, @name, @sourcepath, @destinationpath, @error)";

		public const string Path_UpdateError = "UPDATE TBL_SYNCPATH SET ERROR = @error WHERE ID = @id";
		public const string Path_Update = "UPDATE TBL_SYNCPATH SET NAME = @name, SOURCEPATH = @sourcepath, DESTINATIONPATH = @destinationpath, ERROR = @error, TvDbID = @tvdb  WHERE ID = @id";

		public const string Select_FilesForPath = "SELECT f.ID, f.FILENAME, f.SYNCDATE, f.FILEDATE, f.ISSYNCED, f.ISWATCHED, f.WATCHDATE, f.ISMISSING, f.SEASON, f.EPISODE FROM TBL_SYNCDATA f WHERE f.SYNCPATHID = @pathid";

		public const string Select_FileByName = "SELECT ID FROM TBL_SYNCDATA WHERE FILENAME LIKE '{0}'";
		public const string Select_File = "SELECT ID FROM TBL_SYNCDATA WHERE FILENAME = @filename OR ID = @id";
//		public const string Select_File = "SELECT ID FROM TBL_SYNCDATA WHERE FILENAME LIKE '{0}' OR ID = '{1}'";

		public const string Insert_File = "INSERT INTO TBL_SYNCDATA(ID, SYNCPATHID, FILENAME, FILEDATE, SYNCDATE, ISSYNCED, ISWATCHED, SEASON, EPISODE, ISMISSING ) VALUES(@id, @syncpathid, @filename, @filedate, @syncdate, @issynced, @iswatched, @season, @episode, @ismissing)";

		public const string Update_FileSync = "UPDATE TBL_SYNCDATA SET ISSYNCED = @issynced, SYNCDATE = @syncdate, ISMISSING = @ismissing, FILENAME = @filename, FILEDATE = @filedate WHERE ID = @id";
		public const string Update_FileWatch = "UPDATE TBL_SYNCDATA SET ISWATCHED = @iswatched , WATCHDATE = @watchdate WHERE ID = @id";
		public const string Update_File = "UPDATE TBL_SYNCDATA SET ISWATCHED = @iswatched , WATCHDATE = @watchdate, ISSYNCED = @issynced, SYNCDATE = @syncdate, SEASON = @season, EPISODE = @episode WHERE ID = @id";

		public const string Select_Settings_All = "SELECT TerraCopyPath, ProcessWaitTime, NotificationTime, NotificationsEnabled, SkipExtensions FROM TBL_SETTINGS";

		public const string Param_PathID = "@id";
		public const string Param_PathName = "@name";
		public const string Param_PathSource = "@sourcepath";
		public const string Param_PathDestination = "@destinationpath";
		public const string Param_PathError = "@error";
		public const string Param_PathTvDb = "@tvdb";
		public const string C_PathID = "ID";
		public const string C_PathName = "NAME";
		public const string C_PathSource = "SOURCEPATH";
		public const string C_PathDestination = "DESTINATIONPATH";
		public const string C_PathTvDbID = "TvDbID";

		public const string Param_FileID = "@id";
		public const string Param_FilePathID = "@syncpathid";
		public const string Param_FileName = "@filename";
		public const string Param_FileDate = "@filedate";
		public const string Param_FileIsSynced = "@issynced";
		public const string Param_FileSyncDate = "@syncdate";
		public const string Param_FileIsWatched = "@iswatched";
		public const string Param_FileWatchDate = "@watchdate";
		public const string Param_FileSeason = "@season";
		public const string Param_FileEpisode = "@episode";
		public const string Param_FileIsMissing = "@ismissing";
		

		public const string C_FileID = "ID";
		public const string C_FilePathID = "SYNCPATHID";
		public const string C_FileName = "FILENAME";
		public const string C_FileSyncDate = "SYNCDATE";
		public const string C_FileFileDate = "FILEDATE";
		public const string C_FileWatchDate = "WATCHDATE";
		public const string C_FileIsSynced = "ISSYNCED";
		public const string C_FileIsWatched = "ISWATCHED";
		public const string C_FileIsMissing = "ISMISSING";
		public const string C_FileSeason = "SEASON";
		public const string C_FileEpisode = "EPISODE";
		

		#endregion

		/// <summary>
		/// Reads All the Sync Path and File data and populates a List of SyncPath class
		/// </summary>
		/// <param name="conn"></param>
		/// <returns></returns>
		public static List<SyncPath> ReadFileData(SQLiteConnection conn)
		{
			List<SyncPath> list = new List<SyncPath>();
			using (SQLiteConnection c = new SQLiteConnection(conn.ConnectionString))
			{
				c.Open();
				using (SQLiteCommand cmdPath = new SQLiteCommand(Select_Path_All, c))
				{
					using (SQLiteDataReader r = cmdPath.ExecuteReader())
					{
						if (!r.HasRows)
							return list;

						while (r.Read())
						{
							SyncPath p = new SyncPath(r.GetGuid(C_PathID), r.GetString(C_PathName), r.GetString(C_PathSource), r.GetString(C_PathDestination), r.GetNullableInt32(C_PathTvDbID));
							list.Add(p);

							using (SQLiteCommand cmdFile = new SQLiteCommand(Select_FilesForPath, c))
							{
								cmdFile.Parameters.AddWithValue("@pathid", p.ID);
								using (SQLiteDataReader fr = cmdFile.ExecuteReader())
								{
									while (fr.Read())
									{
										string file = string.Format("{0}\\{1}", p.SourceDir, fr.GetString(C_FileName));
										SyncFile f = new SyncFile(fr.GetGuid(C_FileID), fr.GetDateTime(C_FileSyncDate), fr.GetDateTime(C_FileFileDate), fr.GetBoolean(C_FileIsSynced), fr.GetBoolean(C_FileIsWatched), fr.GetDateTime(C_FileWatchDate), fr.GetBoolean(C_FileIsMissing), fr.GetInt32(C_FileSeason), fr.GetInt32(C_FileEpisode), file);
										p.AddFile(f);
									}
								}
							}
						}
					}
				}
			}

			return list;
		}

		#region Path

		public static bool UpdatePathDatabase(SQLiteConnection conn, SyncPath sp)
		{
			try
			{
				if (SyncPath_ExistInDb(conn, sp))
					return SyncPath_Update(conn, sp);

				return SyncPath_Insert(conn, sp);
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
				sp.SetError();
				return false;
			}
		}

		public static bool SyncPath_ExistInDb(SQLiteConnection conn, SyncPath sp)
		{
			return ExistInDb(conn, string.Format(Select_Path, sp.SourceDir));
		}

		public static Guid SyncPath_GetPathID(SQLiteConnection conn, string dir)
		{
			object cpath = ReadFromDb(conn, string.Format(Select_PathDest, dir), C_PathID);
			return Guid.Parse(cpath.ToString());
		}

		public static bool SyncPath_All(SQLiteConnection conn)
		{
			return ExistInDb(conn, Select_Path_All);
		}		
		
		public static string SyncPath_AllString(SQLiteConnection conn)
		{
			return SelectFromDb(conn, Select_Path_All);
		}

		public static bool SyncPath_Insert(SQLiteConnection conn, SyncPath sp)
		{
			using (SQLiteConnection c = new SQLiteConnection(conn.ConnectionString))
			{
				c.Open();
				using (SQLiteCommand cmd = new SQLiteCommand(Insert_Path, c))
				{
					cmd.Parameters.AddWithValue(Param_PathID, sp.ID);
					cmd.Parameters.AddWithValue(Param_PathName, sp.Name);
					cmd.Parameters.AddWithValue(Param_PathSource, sp.SourceDir);
					cmd.Parameters.AddWithValue(Param_PathDestination, sp.DestinationDir);
					cmd.Parameters.AddWithValue(Param_PathError, sp.Error);

					return cmd.ExecuteNonQuery() > 0;
				}
			}
		}

		public static bool SyncPath_UpdateError(SQLiteConnection conn, SyncPath sp)
		{
			try
			{
				using (SQLiteConnection c = new SQLiteConnection(conn.ConnectionString))
				{
					c.Open();
					using (SQLiteCommand cmd = new SQLiteCommand(Path_UpdateError, c))
					{
						cmd.Parameters.AddWithValue(Param_PathID, sp.ID);
						cmd.Parameters.AddWithValue(Param_PathError, sp.Error);
						return cmd.ExecuteNonQuery() > 0;
					}
				}
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
				return false;
			}
		}		

		public static bool SyncPath_Update(SQLiteConnection conn, SyncPath sp)
		{
			try
			{
				using (SQLiteConnection c = new SQLiteConnection(conn.ConnectionString))
				{
					c.Open();
					using (SQLiteCommand cmd = new SQLiteCommand(Path_Update, c))
					{
						cmd.Parameters.AddWithValue(Param_PathID, sp.ID);
						cmd.Parameters.AddWithValue(Param_PathName, sp.Name);
						cmd.Parameters.AddWithValue(Param_PathSource, sp.SourceDir);
						cmd.Parameters.AddWithValue(Param_PathDestination, sp.DestinationDir);
						cmd.Parameters.AddWithValue(Param_PathError, sp.Error);
						cmd.Parameters.AddWithValue(Param_PathTvDb, sp.TvDbID);
						return cmd.ExecuteNonQuery() > 0;
					}
				}
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
				return false;
			}
		}

		#endregion

		#region Files

		public static bool UpdateFileDatabase(SQLiteConnection conn, SyncPath sp, SyncFile sf)
		{
			try
			{
				if (SyncFiles_ExistInDb(conn, sf))
					return SyncFiles_UpdateSync(conn, sf);

				return SyncFiles_Insert(conn, sp, sf);
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
				sp.SetError();
				return false;
			}
		}

		/// <summary>
		/// Query Database by File Nam to determine if the File exists in Table
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="fi"></param>
		/// <returns></returns>
		public static bool SyncFiles_ExistInDb(SQLiteConnection conn, FileInfo fi)
		{
			return ExistInDb(conn, string.Format(Select_FileByName, fi.Name));
		}

		/// <summary>
		/// Query Database by FileNam or ID to determine if the File exists in Table
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="sf"></param>
		/// <returns></returns>
		public static bool SyncFiles_ExistInDb(SQLiteConnection conn, SyncFile sf)
		{
			SQLiteCommand cmd = new SQLiteCommand(Select_File, conn);
			cmd.Parameters.AddWithValue(Param_FileID, sf.ID);
			cmd.Parameters.AddWithValue(Param_FileName, sf.File.Name);
			return ExistInDb(conn, cmd );  //string.Format(Select_File, sf.File.Name, sf.ID));
		}

		/// <summary>
		/// If the File does not Exist it is Inserted otherwize return False
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="sp"></param>
		/// <param name="sf"></param>
		/// <returns></returns>
		public static bool SyncFiles_Insert(SQLiteConnection conn, SyncPath sp, SyncFile sf)
		{
			using (SQLiteConnection c = new SQLiteConnection(conn.ConnectionString))
			{
				c.Open();
				using (SQLiteCommand cmd = new SQLiteCommand(Insert_File, c))
				{
					cmd.Parameters.AddWithValue(Param_FileID, sf.ID);
					cmd.Parameters.AddWithValue(Param_FilePathID, sp.ID);
					cmd.Parameters.AddWithValue(Param_FileName, sf.File.Name);
					cmd.Parameters.AddWithValue(Param_FileDate, sf.File.CreationTime);
					cmd.Parameters.AddWithValue(Param_FileSyncDate, sf.SyncDate);
					cmd.Parameters.AddWithValue(Param_FileIsSynced, sf.IsSynced);
					cmd.Parameters.AddWithValue(Param_FileIsWatched, sf.IsWatched);
					cmd.Parameters.AddWithValue(Param_FileSeason, sf.Season);
					cmd.Parameters.AddWithValue(Param_FileEpisode, sf.Episode);
					cmd.Parameters.AddWithValue(Param_FileIsMissing, sf.IsMissing);

					return cmd.ExecuteNonQuery() > 0;
				}
			}
		}

		/// <summary>
		/// Updates the database with values from the Collection
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="sp"></param>
		/// <param name="sf"></param>
		/// <returns></returns>
		public static bool SyncFiles_UpdateSync(SQLiteConnection conn, SyncFile sf)
		{
			using (SQLiteConnection c = new SQLiteConnection(conn.ConnectionString))
			{
				c.Open();
				using (SQLiteCommand cmd = new SQLiteCommand(Update_FileSync, c))
				{
					cmd.Parameters.AddWithValue(Param_FileID, sf.ID);
					cmd.Parameters.AddWithValue(Param_FileSyncDate, sf.IsSynced ? sf.SyncDate : null);
					cmd.Parameters.AddWithValue(Param_FileIsSynced, sf.IsSynced);
					cmd.Parameters.AddWithValue(Param_FileSeason, sf.Season.HasValue ? sf.Season.Value : 0);
					cmd.Parameters.AddWithValue(Param_FileEpisode, sf.Episode.HasValue ? sf.Episode.Value : 0);
					cmd.Parameters.AddWithValue(Param_FileIsMissing, sf.IsMissing);

					cmd.Parameters.AddWithValue(Param_FileName, sf.File.Name);
					cmd.Parameters.AddWithValue(Param_FileDate, sf.File.CreationTime);
					return cmd.ExecuteNonQuery() > 0;
				}
			}
		}		
		
		/// <summary>
		/// Updates the database with values from the Collection
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="sp"></param>
		/// <param name="sf"></param>
		/// <returns></returns>
		public static bool SyncFiles_UpdateWatch(SQLiteConnection conn, SyncFile sf)
		{
			using (SQLiteConnection c = new SQLiteConnection(conn.ConnectionString))
			{
				c.Open();
				using (SQLiteCommand cmd = new SQLiteCommand(Update_FileWatch, c))
				{
					cmd.Parameters.AddWithValue(Param_FileID, sf.ID);
					cmd.Parameters.AddWithValue(Param_FileIsWatched, sf.IsWatched);
					cmd.Parameters.AddWithValue(Param_FileWatchDate, sf.IsWatched ? sf.WatchDate : null );
					return cmd.ExecuteNonQuery() > 0;
				}
			}
		}		
		
		/// <summary>
		/// Updates the database with values from the Collection
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="sp"></param>
		/// <param name="sf"></param>
		/// <returns></returns>
		public static bool SyncFiles_Update(SQLiteConnection conn, SyncFile sf)
		{
			using (SQLiteConnection c = new SQLiteConnection(conn.ConnectionString))
			{
				c.Open();
				using (SQLiteCommand cmd = new SQLiteCommand(Update_File, c))
				{
					cmd.Parameters.AddWithValue(Param_FileID, sf.ID);
					cmd.Parameters.AddWithValue(Param_FileIsWatched, sf.IsWatched);
					cmd.Parameters.AddWithValue(Param_FileWatchDate, sf.IsWatched ? sf.WatchDate : null );
					cmd.Parameters.AddWithValue(Param_FileSyncDate, sf.IsSynced ? sf.SyncDate : null);
					cmd.Parameters.AddWithValue(Param_FileIsSynced, sf.IsSynced);
					cmd.Parameters.AddWithValue(Param_FileSeason, sf.Season);
					cmd.Parameters.AddWithValue(Param_FileEpisode, sf.Episode);
					return cmd.ExecuteNonQuery() > 0;
				}
			}
		}

		#endregion

		public static object GetSettings(SQLiteConnection conn, string settingName)
		{
			using (SQLiteConnection c = new SQLiteConnection(conn.ConnectionString))
			{
				c.Open();
				using (SQLiteCommand cmd_select = new SQLiteCommand(Select_Settings_All, c))
				{
					using (SQLiteDataReader reader = cmd_select.ExecuteReader(CommandBehavior.SingleRow))
					{
						reader.Read();
						return reader[settingName];
					}
				}
			}
		}

		public static bool ExistInDb(SQLiteConnection conn, string command)
		{
			try
			{
				using (SQLiteConnection c = new SQLiteConnection(conn.ConnectionString))
				{
					c.Open();
					using (SQLiteCommand cmd_select = new SQLiteCommand(command, c))
					{
						using (SQLiteDataReader reader = cmd_select.ExecuteReader(CommandBehavior.SingleRow))
						{
							return reader.HasRows;
						}
					}
				}
			}
			catch (Exception e)
			{
				Trace.WriteLine(string.Format("ERROR ON ExistInDB() Command: [{0}]\n{1}", command, e));
				return false;
			}
		}

		public static bool ExistInDb(SQLiteConnection conn, SQLiteCommand command)
		{
			try
			{
				using (SQLiteConnection c = new SQLiteConnection(conn))
				{
					c.Open();
					using (command)
					{
						command.Connection = c;
						using (SQLiteDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
						{
							return reader.HasRows;
						}
					}
				}
			}
			catch (Exception e)
			{
				Trace.WriteLine(string.Format("ERROR ON ExistInDB() Command: [{0}]\n{1}",command.CommandText, e));
				return false;
			}
		}			
		
		public static object ReadFromDb(SQLiteConnection conn, string command, string value)
		{
			try
			{
				using (SQLiteConnection c = new SQLiteConnection(conn.ConnectionString))
				{
					c.Open();
					using (SQLiteCommand cmd_select = new SQLiteCommand(command, c))
					{
						using (SQLiteDataReader reader = cmd_select.ExecuteReader(CommandBehavior.SingleRow))
						{
							reader.Read();
							return reader[value];
						}
					}
				}
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
				return false;
			}
		}		
		
		public static string SelectFromDb(SQLiteConnection conn, string command)
		{
			string str = string.Empty;
			try
			{
				using (SQLiteConnection c = new SQLiteConnection(conn.ConnectionString))
				{
					c.Open();
					using (SQLiteCommand cmd_select = new SQLiteCommand(command, c))
					{
						using (SQLiteDataReader r = cmd_select.ExecuteReader())
						{
							
							while (r.Read())
							{
								str += string.Format("{0}, {1}, {2}, {3} <br/>", r.GetGuid(C_PathID), r.GetString(C_PathName), r.GetString(C_PathSource), r.GetString(C_PathDestination));
							}
							return str;
						}
					}
				}
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
				return "Exception...";
			}
		}
	}
}
