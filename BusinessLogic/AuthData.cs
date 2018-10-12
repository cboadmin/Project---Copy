using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBS.BusinessLogic
{
	public class AuthData
	{
		public string Address;
		public string UserName;
		public string AuthKey;
		public Boolean Valid;
		public string Authenticationdata;
		public AuthData(string key)
		{
			//add080618LNPNLMUPPUOT8476B0A9-C34E-4219-A87B-A0CB56AAB5EA

			try
			{
				Address = key.Substring(0, 9);
				AuthKey = key.Substring(9, 12);
				int ln = key.Length - (Address.Length + AuthKey.Length);
				UserName = key.Substring(Address.Length + AuthKey.Length, ln);
				string transKey = TranslateAuthKey(AuthKey);
				DateTime D1 = TranslateToDate(transKey);
				DateTime D2 = DateTime.Now;
				TimeSpan t = D2 - D1;
				double hr = t.TotalHours;
				if (hr > 8) Valid = false;
				if (hr < 9) Valid = true;
				if (hr < 0) Valid = false;
			}
			catch (Exception ex)
			{
				Valid = false;
			}

		}
		public string CreateGetNewKey()
		{
			string skey = DateTime.Now.ToString("yyMMddHHmmss");
			AuthKey = EncryptAuthKey(skey);

			return Address + AuthKey + UserName;
		}
		public string CreateServerKey(string svrAddress, string svrUserName)
		{
			string skey = DateTime.Now.ToString("yyMMddHHmmss");
			AuthKey = EncryptAuthKey(skey);

			return svrAddress + AuthKey + svrUserName;
		}
		public string TranslateAuthKey(string mAuthKey)
		{
			string value = mAuthKey;
			value = value.Replace('P', '0');
			value = value.Replace('L', '1');
			value = value.Replace('U', '2');
			value = value.Replace('T', '3');
			value = value.Replace('O', '4');
			value = value.Replace('M', '5');
			value = value.Replace('S', '6');
			value = value.Replace('A', '7');
			value = value.Replace('N', '8');
			value = value.Replace('G', '9');

			return value;
		}

		public string EncryptAuthKey(string value)
		{
			value = value.Replace('0', 'P');
			value = value.Replace('1', 'L');
			value = value.Replace('2', 'U');
			value = value.Replace('3', 'T');
			value = value.Replace('4', 'O');
			value = value.Replace('5', 'M');
			value = value.Replace('6', 'S');
			value = value.Replace('7', 'A');
			value = value.Replace('8', 'N');
			value = value.Replace('9', 'G');

			return value;

		}
		private DateTime TranslateToDate(string key)
		{
			int yr, mnth, dy, hr, mn, sc;
			yr = System.Convert.ToInt32("20" + key.Substring(0, 2));
			mnth = System.Convert.ToInt32(key.Substring(2, 2));
			dy = System.Convert.ToInt32(key.Substring(4, 2));
			hr = System.Convert.ToInt32(key.Substring(6, 2));
			mn = System.Convert.ToInt32(key.Substring(8, 2));
			sc = System.Convert.ToInt32(key.Substring(10, 2));

			DateTime dt = new DateTime(yr, mnth, dy, hr, mn, sc);
			return dt;
		}
	}
}