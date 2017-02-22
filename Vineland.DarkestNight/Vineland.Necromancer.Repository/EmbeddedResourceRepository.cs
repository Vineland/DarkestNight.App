using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace Vineland.Necromancer.Repository
{
	public class EmbeddedResourceRepository :IRepository
	{
		public EmbeddedResourceRepository()
		{
		}

		public IEnumerable<T> GetAll<T>() where T : class
		{
			var resourceFile = string.Format("Vineland.Necromancer.Repository.Resources.{0}.json", typeof(T).Name.ToLower());
			return JsonConvert.DeserializeObject<List<T>>(ReadEmbeddedResource(resourceFile), new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Objects
			});
		}

		private string ReadEmbeddedResource(string resourceId)
		{
			var assembly = typeof(EmbeddedResourceRepository).GetTypeInfo().Assembly;

			using (var reader = new System.IO.StreamReader(assembly.GetManifestResourceStream(resourceId)))
			{
				return reader.ReadToEnd();
			}
		}
	}
}
