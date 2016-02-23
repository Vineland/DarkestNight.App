using System;
using Vineland.DarkestNight.UI;

namespace Vineland.Necromancer.UI
{
	public class LogHelper
	{
		#if DEBUG
		private const LogLevel _level = LogLevel.Verbose;
		#else
		private const LogLevel _level = LogLevel.Error;
		#endif

		public static void Error (Exception ex)
		{
			if (_level >= LogLevel.Error)
				Log (ex.Message + Environment.NewLine + ex.StackTrace);
		}

		public static void Info (string message)
		{
			if (_level >= LogLevel.Info)
				Log (message);
		}

		public static void Verbose (string message)
		{
			if (_level >= LogLevel.Verbose)
				Log (message);
		}

		private static void Log (string message)
		{
			Console.Write (AppConstants.AppName + " - " + message);
		}
	}

	public enum LogLevel
	{
		Error,
		Info,
		Verbose
	}
}

