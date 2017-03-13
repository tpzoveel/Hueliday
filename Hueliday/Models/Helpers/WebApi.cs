using System;
using System.Net;

namespace Hueliday
{
	public static class WebApi
	{
		private static string Request(string uri, string data, string method)
		{
			string result;
			using (var client = new WebClient())
			{
				result = client.UploadString(uri, method, data);
			}
			return result;
		}

		public static string Get(string uri)
		{
			string jsonString;
			using (var client = new WebClient())
			{
				jsonString = client.DownloadString(uri);
			}
			return jsonString;
		}

		public static string Delete(string uri, string data) => Request(uri, data, "DELETE");

		public static string Put(string uri, string data) => Request(uri, data, "PUT");

		public static string Post(string uri, string data) => Request(uri, data, "POST");
	}
}
