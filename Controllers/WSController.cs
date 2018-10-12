using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;
using System.IO;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using BBS.BusinessLogic;
using Newtonsoft.Json;

namespace BBS.Controllers
{
	//test1234

	#region code proper
	public class WSController : ApiController
	{
		public HttpResponseMessage Get(string user, string pass)
		{
			Utilities util = new Utilities();
			string form = "?key=" + "Mercury3356Lorreignmay29" + "&procedurename=" + "UserLoginValidation" + "&" + "Username=" + user + "&Password=" + pass;
			string url = "http://doktrack.com/api/ProcessRequest/Execute" + form;
			string data = util.SQLToJsonToAPI(url);
			dynamic results = JsonConvert.DeserializeObject<dynamic>(data);
			string mkey = results.UserLoginValidation[0].AuthKey;

			AuthData Auth = new AuthData(mkey);
			Auth.Authenticationdata = data;

			if (Auth.Valid)
			{
				int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
				if (HttpContext.Current.IsWebSocketRequest)
				{
					WSHandler handler = new WSHandler(Auth);
					HttpContext.Current.AcceptWebSocketRequest(handler.ProcessWS);
				}
				return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
			}
			else
			{
				return new HttpResponseMessage(HttpStatusCode.Forbidden);
			}
			
		}
	}


	public sealed class Singleton
	{
		static Singleton instance = null;
		static readonly object padlock = new object();
		static List<Socket> Sockets;
		static bool timeronrun;
		static long Counter;
		static int Countdown;
		public long Memory;
		public class Socket
		{
			public int Port;
			public string IP;
			public DateTime d1;
			public string UserName;
			public string AuthKey;
			public string Address;
			public WebSocket ws;
		}
		private class StateObjClass
		{
			// Used to hold parameters for calls to TimerTask. 
			public int SomeValue;
			public System.Threading.Timer TimerReference;
			public bool TimerCanceled;
		}
		public static Singleton Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new Singleton();
						Sockets = new List<Socket>();
						timeronrun = false;
						Counter = 0;
						Countdown = 0;
					}
					return instance;
				}
			}
		}

		#region Properties
		public void AddSocket(Socket sckt)
		{
			var s = Sockets.Where(m => m.ws == sckt.ws);
			if (s.Count() == 0) Sockets.Add(sckt);

		}
		public int SocketCount(string username)
		{
			return Sockets.Count(a => a.UserName == username);
		}
		public int SocketCount()
		{
			return Sockets.Count();
		}
		public void removeSocket(WebSocket wbs)
		{
			Socket sc = Sockets.Where(m => m.ws == wbs).First();
			Sockets.Remove(sc);

		}
		public Socket FindSocket(WebSocket wbs)
		{
			Socket sc = Sockets.Where(m => m.ws == wbs).First();
			return sc;

		}

		#endregion

		public void RunTimer()
		{
			if (timeronrun == false)
			{
				timeronrun = true;

				StateObjClass StateObj = new StateObjClass();
				StateObj.TimerCanceled = false;
				StateObj.SomeValue = 1;
				System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(TimerTask);
				System.Threading.Timer TimerItem = new System.Threading.Timer(TimerDelegate, StateObj, 1000, 1000);
				StateObj.TimerReference = TimerItem;
			}

		}
		private void TimerTask(object StateObj)
		{


			StateObjClass State = (StateObjClass)StateObj;
			System.Threading.Interlocked.Increment(ref State.SomeValue);
			string data = DateTime.Now.ToLongTimeString();


			Evaluate();

			if (timeronrun == false)
			// Dispose Requested.
			{
				State.TimerReference.Dispose();

			}
		}
		private string SocketStatus(long timerCount)
		{
			string status = "TimerCounter" + timerCount + ": SckCount:" + Sockets.Count + " Users :";
			try
			{
				foreach (Singleton.Socket sc in Sockets)
				{
					string data;
					data = sc.UserName + ":" + sc.IP + ":" + sc.Port + "->";
					status = status + data;
				}
			}
			catch (Exception ex)
			{
				string msg = ex.Message;
			}

			return status;
		}
		private void Evaluate()
		{
			BL bl = new BL();
			Countdown++;
			try
			{
				foreach (Singleton.Socket sc in Sockets)
				{
					string message = DateTime.Now.ToLongTimeString();

					try
					{
						if (sc.ws.State == WebSocketState.Open)
						{
							if (Countdown > 29)
							{
								ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
								buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
								sc.ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
								Countdown = 0;

							}


						}
						else
						{
							Sockets.Remove(sc);

						}

					}
					catch (Exception ex)
					{
						string s = ex.Message;
						if (s != "A send operation is already in progress.")
						{
							Sockets.Remove(sc);
							break;
						}

					}


				}

			}
			catch (Exception ex)
			{
				string message = ex.Message;
			}

		}
		private bool SendMessage(string username, string data, int MessengerID)
		{
			bool MessageSent = false;
			var sockets = Sockets.Where(m => m.UserName == username);
			foreach (Singleton.Socket sc in sockets)
			{
				string message = data;
				try
				{
					if (sc.ws.State == WebSocketState.Open)
					{
						MessageSent = true;
						ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
						buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
						sc.ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

					}

				}
				catch (Exception ex)
				{

				}


			}
			return MessageSent;
		}
		private bool SendBroadcast(string data, int MessengerID)
		{
			bool MessageSent = false;

			foreach (Singleton.Socket sc in Sockets)
			{
				string message = data;
				try
				{
					if (sc.ws.State == WebSocketState.Open)
					{
						MessageSent = true;
						ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
						buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
						sc.ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

					}

				}
				catch (Exception ex)
				{

				}
			}


			return MessageSent;
		}
		public bool SendToAll(string data)
		{

			bool MessageSent = false;

			foreach (Singleton.Socket sc in Sockets)
			{
				string message = data;
				try
				{
					if (sc.ws.State == WebSocketState.Open)
					{
						MessageSent = true;
						ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
						buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
						sc.ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

					}

				}
				catch (Exception ex)
				{

				}
			}



			return MessageSent;
		}
		public bool SendToAllExceptUser(string data, string User)
		{

			bool MessageSent = false;

			foreach (Singleton.Socket sc in Sockets)
			{
				string message = data;
				try
				{
					if (sc.ws.State == WebSocketState.Open)
					{
						if (sc.UserName != User)
						{
							MessageSent = true;
							ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
							buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
							sc.ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
						}


					}

				}
				catch (Exception ex)
				{

				}
			}



			return MessageSent;
		}
		public bool SendTargetedPing(string data, string username)
		{
			bool MessageSent = false;
			var sockets = Sockets.Where(m => m.UserName == username);
			foreach (Singleton.Socket sc in sockets)
			{
				string message = data;
				try
				{
					if (sc.ws.State == WebSocketState.Open)
					{
						MessageSent = true;
						ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
						buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
						sc.ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
					}

				}
				catch (Exception ex)
				{

				}


			}
			return MessageSent;
		}

	}

	public enum eDirection
	{
		Echo,
		TargetedPing,
		SendToAll,
		SendToallExceptUser
	};
	public class Dirusme
	{
		public eDirection Direction;
		public string User;
		public string Message;


	}
	public class BL
	{

		private struct Parameters
		{
			public string Field;
			public string Value;
		}
		const char LeftParenthesis = '{';
		const char RightParenthesis = '}';
		bool AreParenthesesBalanced(string text)
		{
			bool found = false;
			for (var i = 1; i < text.Length - 1; i++)
			{
				if (text[i] == '}')
					return false;

				if (text[i] == '{')
				{
					found = true;
					break;
				}

			}
			if (found == false) return false;
			found = false;
			for (var i = text.Length - 1; i > 0; i--)
			{
				if (text[i] == '{')
					return false;

				if (text[i] == '}')
				{
					found = true;
					break;
				}
			}
			if (found == false) return false;
			return true;
		}

		public List<Dirusme> ProcMsg(string data)
		{
			string message = "";
			List<Dirusme> dirusme = new List<Dirusme>();
			try
			{
				Debug.Print(data);

				string mdata = data + ":::";
				string[] rcvData = mdata.Split(new char[] { ':' }, StringSplitOptions.None);
				if (rcvData.Length > 3)
				{

					dirusme.Add(Dmass(eDirection.Echo, "", message));
					dirusme.Add(Dmass(eDirection.SendToallExceptUser, "", message));
					dirusme.Add(Dmass(eDirection.TargetedPing, "", message));
					dirusme.Add(Dmass(eDirection.SendToAll, "", message));

				}

			}
			catch (Exception ex)
			{
				message = "Error," + ex.Message;
				dirusme.Add(Dmass(eDirection.Echo, "", message));

			}


			return dirusme;
		}

		private Dirusme Dmass(eDirection direction, string user, string message)
		{
			Dirusme di = new Dirusme();
			di.Direction = direction;
			di.User = user;
			di.Message = message;

			return di;
		}

	}

	#endregion

}

