using System;
using SQLite.Net.Interop;

namespace SayUSDollar
{
	public class DataLayer
	{
		public static DataLayer Instance { get; } = new DataLayer();

		Database Database;

		public void SetDataBasePlatform(ISQLitePlatform platform)
		{
			if (Database == null)
			{
				Database = new Database(platform);
				Database.Open();
			}
		}
	}
}
