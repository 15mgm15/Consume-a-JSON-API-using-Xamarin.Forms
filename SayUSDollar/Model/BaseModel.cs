using System;
using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace SayUSDollar
{
	public class BaseModel
	{
		[PrimaryKey, AutoIncrement, JsonIgnore]
		public int Id { get; set; }

		[Default(true), JsonIgnore]
		public DateTime Created { get; set; } = DateTime.UtcNow;

		[Default(true), JsonIgnore]
		public DateTime Updated { get; set; } = DateTime.UtcNow;
	}
}
