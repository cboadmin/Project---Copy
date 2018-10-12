using DokTrack.BusinessLogic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DokTrack.API
{
    public class DataAccessController : ApiController
    {
		[HttpGet]
		public JObject ProcessRequest()
		{
			try
			{
				var qs = HttpUtility.ParseQueryString(Request.RequestUri.Query);
				string key = qs["key"];
				string storedProc = qs["procedurename"];

				Dictionary<string, string> paramdictionary = new Dictionary<string, string>();

				foreach (var keys in qs)
				{
					string tmpkey = keys.ToString();
					if ((tmpkey != "key") && (tmpkey != "procedurename"))
					{
						paramdictionary.Add(tmpkey, qs[tmpkey].ToString());
					}

				}
				GetCallBack getCallBack = new GetCallBack();
				string data = getCallBack.SQLToJson(storedProc, paramdictionary);
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
