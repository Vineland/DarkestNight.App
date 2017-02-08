using System;
using System.Collections.Generic;

namespace Vineland.Necromancer.Repository
{
	public interface IRepository
	{
		IEnumerable<T> GetAll<T>() where T : class;
	}
}
