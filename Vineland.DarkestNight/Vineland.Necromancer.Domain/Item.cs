using System;
using System.Dynamic;

namespace Vineland.Necromancer.Domain
{
	public class Item
	{
		public int Id {get;set;}
		public string Name {get;set;}
		public Expansion Expansion { get; set; }
	}
}

