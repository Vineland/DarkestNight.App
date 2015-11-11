using System;
using System.Linq;

namespace Vineland.DarkestNight.Core.Services
{
	public static class LogHelper
	{
		public static void Log(string area, string message)
		{
#if DEBUG
            Console.WriteLine(string.Format("{0} - {1}", area, message));
#endif
		}

		public static void Log(string area, string messageFormat, object[] args)
		{
#if DEBUG
            var message = string.Format(messageFormat, args);

            Console.WriteLine(string.Format("{0} - {1}", area, message));
#endif
		}
	}
}
