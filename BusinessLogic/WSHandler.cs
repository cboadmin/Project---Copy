using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;
using BBS.Controllers;

namespace BBS.BusinessLogic
{
	public class MessageHandler
	{
		public string Key { get; set; }
		public string Destination { get; set; }
		public string Header { get; set; }
		public string Message { get; set; }
	}

	public class WSHandler
	{
		public string Key;
		private AuthData auth;
		private Singleton.Socket SC = new Singleton.Socket();
		public WSHandler(AuthData Auth)
		{
			auth = Auth;
		}
		public async Task ProcessWS(AspNetWebSocketContext context)
		{
			WebSocket socket = context.WebSocket;

			Socket_Add_UpdateUserinfo(socket); // This is the initalization point

			await Task.WhenAll(Task_Checkuserifalive(socket), ReadTask(socket)); //Mo pitik ni kung naay transaction else hulat lang

		}
		private void Socket_Add_UpdateUserinfo(WebSocket ws)
		{
			

			string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			int port = System.Convert.ToInt32(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_PORT"]);
			if (string.IsNullOrEmpty(ip)) { ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; }

			SC.IP = ip;
			SC.Port = port;
			SC.ws = ws;
			SC.UserName = auth.UserName;
			SC.AuthKey = auth.AuthKey;
			SC.Address = auth.Address;

			Singleton Handler = Singleton.Instance;
			Handler.AddSocket(SC);

		}
		// MUST read if we want the socket state to be updated
		private async Task ReadTask(WebSocket ws)
		{
			Singleton Handler = Singleton.Instance;

			ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
			while (true)
			{

				if (ws.State != WebSocketState.Open)
				{
					Handler.removeSocket(ws);
					break;
				}

				WebSocketReceiveResult result = await ws.ReceiveAsync(buffer, CancellationToken.None).ConfigureAwait(false);
				string userMessage = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);

				Utilities util = new Utilities();
				string form = "?key=" + "Mercury3356Lorreignmay29" + "&" + userMessage;
				string url = "http://syncserver.brgybudgetdvo.com/api/ProcessRequest/Execute" + form;
				string sjson = util.SQLToJsonToAPI(url);
				var buff = Encoding.UTF8.GetBytes(sjson);
				var sendTask = ws.SendAsync(new ArraySegment<byte>(buff), WebSocketMessageType.Text, true, CancellationToken.None);
				await sendTask.ConfigureAwait(false);
			}
		}
		private async Task Task_Checkuserifalive(WebSocket ws)
		{
			Singleton Handler = Singleton.Instance;
			while (true)
			{

				string sjson = auth.Authenticationdata;


				var buffer = Encoding.UTF8.GetBytes(sjson);
				if (ws.State != WebSocketState.Open)
				{
					Handler.removeSocket(ws);
					break;
				}

				var sendTask = ws.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
				await sendTask.ConfigureAwait(false);

				if (ws.State != WebSocketState.Open)
				{
					Handler.removeSocket(ws);
					break;
				}

				int hr = 1;
				int TotalSec = ((hr * 60) * 60) * 1000;
				await Task.Delay(TotalSec).ConfigureAwait(false); // Checks every 1 hr
			}
		}

	}
}