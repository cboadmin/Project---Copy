using BBS.BusinessLogic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BBS.API
{
    public class ProcessRequestController : ApiController
    {
		HttpClient client = new HttpClient();
		private Func<HttpClient, string, string> getServiceJson = (clnt, srvcUrl) =>
		{
			var task = clnt.GetStringAsync(srvcUrl);
			var jsonData = task.Result;
			return jsonData;

		};
		private string SQLToJsonToAPI(string URL)
		{
			return getServiceJson(client, URL);
		}

		[HttpGet]
		public JObject Execute()
		{
			try
			{
				var qs = HttpUtility.ParseQueryString(Request.RequestUri.Query);
				string key = qs["key"];
				string storedProc = qs["procedurename"];

				Dictionary<string, string> paramdictionary = new Dictionary<string, string>();

				string url = "http://doktrack.com/api/ProcessRequest/Execute" + Request.RequestUri.Query;

				string data = SQLToJsonToAPI(url);
				var jsonObject = JObject.Parse(data);

				return jsonObject;


			}
			catch (Exception ex)
			{
				string message = "{\"error\":\"" + ex.Message + "\"}";
				var jsonObject = JObject.Parse(message);
				return jsonObject;
			}
		}
	}
}
