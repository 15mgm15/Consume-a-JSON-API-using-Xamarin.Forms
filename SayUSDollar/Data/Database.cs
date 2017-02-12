using System;
using System.IO;
using SQLite.Net;
using SQLite.Net.Interop;

using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SayUSDollar.Model;

namespace SayUSDollar
{
	public class Database
	{
		readonly ISQLitePlatform _platform;
		string _databasePath;
		const string DATA_FOLDER = "Data";
		const string DATABASE_NAME = "currencies.sqlite";

		public SQLiteConnection Connection { get; set; }
		
		public Database(ISQLitePlatform platform)
		{
			_platform = platform;
		}

		public void Open()
		{
			var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var directoryname = Path.Combine(appData, DATA_FOLDER);

			if (!Directory.Exists(directoryname))
			{
				Directory.CreateDirectory(directoryname);
			}

			_databasePath = Path.Combine(directoryname, DATABASE_NAME);

			Connection = new SQLiteConnection(_platform, _databasePath, 
			                                  SQLiteOpenFlags.ReadWrite | 
			                                  SQLiteOpenFlags.Create | 
			                                  SQLiteOpenFlags.FullMutex, true);

			//Create our tables
			Connection.CreateTable<RootObject>();
			Connection.CreateTable<Currency>();

		}

		#region Extended Methods

		/// <summary>
		/// Save the object and its children. the insert could be cancelled if the task is cancelled.
		/// </summary>
		/// <param name="collection">Object to save</param>
		/// <param name="token">Use to cancel this task</param>
		public void SetObjectWithChildren(object collection, CancellationToken token)
		{
			lock (this)
			{
				try
				{
					Connection.BeginTransaction();
					Connection.InsertAllWithChildren(new List<object>() { collection }, true);
					if (token.IsCancellationRequested)
					{
						Connection.Rollback();
					}
					Connection.Commit();
				}
				catch
				{
					Connection.Rollback();
				}
			}
		}

		/// <summary>
		/// Save or replace the object and its children.
		/// </summary>
		/// <param name="collection"> Objecto to replace</param>
		public void SetObjectWithChildrenReplace(object collection)
		{

			Connection.InsertOrReplaceWithChildren(collection, true);
		}


		/// <summary>
		/// Delete object and its children.
		/// </summary>
		/// <param name="collection">Collerction to delete</param>
		/// <param name="token">Use to cancel this Task</param>
		public void DeleteObjectWithChildren(object collection, CancellationToken token)
		{
			lock (this)
			{
				try
				{
					Connection.BeginTransaction();
					Connection.Delete(collection, true);
					if (token.IsCancellationRequested)
					{
						Connection.Rollback();
					}
					Connection.Commit();
				}
				catch (Exception)
				{
					Connection.Rollback();
				}
			}
		}

		/// <summary>
		/// Insert only the object that was sent.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity">T object to insert</param>
		public void Insert<T>(T entity) where T : BaseModel
		{
			Connection.Insert(entity);
		}

		/// <summary>
		/// Get the object T with its children
		/// </summary>
		/// <typeparam name="T">Object to get</typeparam>
		/// <returns></returns>
		public IQueryable<T> Fetch<T>() where T : BaseModel
		{
			//Always it will return current data user.
			return Connection.GetAllWithChildren<T>(recursive: true).AsQueryable();
		}

		/// <summary>
		/// Update the object in the database.
		/// </summary>
		/// <typeparam name="T">Type.</typeparam>
		/// <param name="entity">Object to update.</param>
		public void Update<T>(T entity) where T : BaseModel
		{
			Connection.Update(entity);
		}

		/// <summary>
		/// Get Object by id.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id">Type of object to get information.</param>
		/// <returns></returns>
		public T GetByID<T>(int id) where T : BaseModel
		{
			return Connection.Table<T>()
				.AsQueryable()
				.FirstOrDefault(e => e.SqlId == id);
		}

		#endregion
	}
}
