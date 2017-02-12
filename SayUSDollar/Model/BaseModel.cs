using System;
using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace SayUSDollar
{
	public class BaseModel
	{
		[PrimaryKey, AutoIncrement, JsonIgnore]
		public int SqlId { get; set; }

		[Default(true), JsonIgnore]
		public DateTime Creation { get; set; } = DateTime.UtcNow;

		[Default(true), JsonIgnore]
		public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
	}
}
