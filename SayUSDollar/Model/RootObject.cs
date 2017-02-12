using System;
using System.Collections.Generic;

namespace SayUSDollar.Model
{
	public class Currency : BaseModel
	{
		public string Name { get; set; }
		public double Rate { get; set; }
	}

	public class RootObject : BaseModel
	{
		public string @base { get; set; }
		public string date { get; set; }
		public Dictionary<string, double> rates { get; set; }
	}
}
