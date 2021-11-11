using System;
using System.Threading;

namespace RoverBot
{
	public class Program
	{
		public const string CheckLine = "******************************************************************************";

		public static void Main()
		{
			try
			{
				Logger.Write(CheckLine);

				if(ApiAgent.ReadApiKey())
				{
					Thread.Sleep(10000);
					
					while(true)
					{
						if(CheckProxy.ReadProxy())
						{
							if(CheckProxy.CheckIp())
							{
								break;
							}
						}
						
						Thread.Sleep(60000);
					}

					BinanceFutures.StartRoverBotAsync().Wait();

					while(BinanceFutures.IsValid())
					{
						Thread.Sleep(1000);
					}
				}
			}
			catch(Exception exception)
			{
				Logger.Write(exception.Message);
			}
		}
	}
}

