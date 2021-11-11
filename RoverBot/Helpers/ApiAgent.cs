using System;
using System.IO;

namespace RoverBot
{
	public static class ApiAgent
	{
		public const string ApiKeyFile = "ApiKey.txt";

		public static string ApiKey { get; private set; }

		public static string SecretKey { get; private set; }

		public static bool ReadApiKey()
		{
			try
			{
				if(File.Exists(ApiKeyFile) == false)
				{
					Logger.Write("ReadApiKey: Invalid File");

					return false;
				}

				var lines = File.ReadAllLines(ApiKeyFile);

				if(lines.Length < 2)
				{
					Logger.Write("ReadApiKey: Invalid Format[1]");

					return false;
				}

				if(lines[0].Length != 64 || lines[1].Length != 64)
				{
					Logger.Write("ReadApiKey: Invalid Format[2]");

					return false;
				}

				ApiKey = lines[0];

				SecretKey = lines[1];

				return true;
			}
			catch(Exception exception)
			{
				Logger.Write("ReadApiKey: " + exception.Message);

				return false;
			}
		}
	}
}

