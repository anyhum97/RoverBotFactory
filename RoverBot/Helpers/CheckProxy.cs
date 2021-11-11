using System;
using System.Net;
using System.Net.Security;
using System.IO;

namespace RoverBot
{
	public static class CheckProxy
	{
		public const string ProxyFile = "Proxy.txt";

		public const string DefaultAddress = "217.8.127.18";

		public const int RequestTimeOut = 10000;

		public static string ProxyAddress { get; private set; }

		public static Random StaticRandom = new Random();

		static CheckProxy()
		{
			try
			{
				IgnoreCertificate();

				ServicePointManager.DefaultConnectionLimit = 128;
			}
			catch(Exception exception)
			{
				Logger.Write("CheckProxy: " + exception.Message);
			}
		}

		public static bool ReadProxy()
		{
			try
			{
				if(File.Exists(ProxyFile))
				{
					FileInfo fileInfo = new FileInfo(ProxyFile);

					const int MaxFileSize = 1024;

					if(fileInfo.Length >= MaxFileSize)
					{
						Logger.Write("CheckProxy.ReadProxy: Invalid File Size");

						return false;
					}

					string str = File.ReadAllText(ProxyFile);
					
					str = str.Replace("\r", "");
					str = str.Replace(" ", "");
					
					string[] lines = str.Split('\n');
					
					if(lines.Length == default)
					{
						Logger.Write("CheckProxy.ReadProxy: Invalid Format[1]");
					
						return false;
					}

					if(lines[default].Length == default)
					{
						Logger.Write("CheckProxy.ReadProxy: Invalid Format[2]");
					
						return false;
					}

					ProxyAddress = lines[default];
					
					return true;
				}
				else
				{
					Logger.Write("CheckProxy.ReadProxy: Invalid File");

					return false;
				}
			}
			catch(Exception exception)
			{
				Logger.Write("CheckProxy.ReadProxy: " + exception.Message);

				return false;
			}
		}

		public static bool CheckIp()
		{
			try
			{
				string[] addresses = new string[]
				{
					"https://api.my-ip.io/ip",
					"http://api.ipaddress.com/myip",
					"https://api.ipify.org?format=json",
				};

				Shuffle(addresses);

				for(int i=default; i<addresses.Length; ++i)
				{
					if(CheckIp(addresses[i]))
					{
						return true;
					}
				}

				Logger.Write("CheckProxy.CheckIp: Invalid Connection");

				return default;
			}
			catch(Exception exception)
			{
				Logger.Write("CheckProxy.CheckIp: " + exception.Message);

				return false;
			}
		}

		private static bool CheckIp(string address)
		{
			try
			{
				if(GetString(address, out string str))
				{
					if(str.Contains(DefaultAddress))
					{
						Logger.Write("CheckProxy.CheckIp: Invalid Proxy");

						return false;
					}

					if(ProxyAddress == default)
					{
						throw new Exception("Invalid Proxy Address");
					}

					if(str.Contains(ProxyAddress))
					{
						return true;
					}
				}

				return default;
			}
			catch(Exception exception)
			{
				Logger.Write(string.Format("CheckProxy.CheckIp({0}): {1}", address, exception.Message));

				return false;
			}
		}

		private static bool GetString(string address, out string str)
		{
			str = default;

			try
			{
				if(address != default)
				{
					WebRequest request = WebRequest.Create(address);

					request.Timeout = RequestTimeOut;

					WebResponse response = request.GetResponse();

					using(Stream stream = response.GetResponseStream())
					{
						StreamReader streamReader = new StreamReader(stream);

						str = streamReader.ReadToEnd();

						streamReader.Close();
					}

					response.Close();

					return true;
				}
				else
				{
					Logger.Write("CheckProxy.DownLoadString: Invalid Address");

					return false;
				}
			}
			catch(Exception exception)
			{
				Logger.Write("CheckProxy.DownLoadString: " + exception.Message);

				return false;
			}
		}

		private static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

		private static void IgnoreCertificate()
		{
			try
			{
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AcceptAllCertifications);
			}
			catch(Exception exception)
			{
				Logger.Write("CheckProxy.IgnoreCertificate: " + exception.Message);
			}
		}

		private static void Shuffle(string[] buffer)
		{
			try
			{
				for(int i=buffer.Length-1; i>=1; i=i-1)
				{
					int j = StaticRandom.Next(i+1);

					string str = buffer[j];

					buffer[j] = buffer[i];
					
					buffer[i] = str;
				}
			}
			catch(Exception exception)
			{
				Logger.Write("CheckProxy.Shuffle: " + exception.Message);
			}
		}
	}
}

