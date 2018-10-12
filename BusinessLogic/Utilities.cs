using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace BBS.BusinessLogic
{
	public class Utilities
	{
		HttpClient client = new HttpClient();
		public Func<HttpClient, string, string> getServiceJson = (clnt, srvcUrl) =>
		{
			var task = clnt.GetStringAsync(srvcUrl);
			var jsonData = task.Result;
			return jsonData;

		};
		public string SQLToJsonToAPI(string URL)
		{
			return getServiceJson(client, URL);
		}
	}
}