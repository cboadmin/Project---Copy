using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace BBS.BusinessLogic
{
	public class DataAccess
	{
		#region "Constructor"
		private static readonly DataAccess instance = new DataAccess();

		private DataAccess()
		{
			try
			{

				string connString = ConnectionString;
				sqlconn = new SqlConnection(connString);
				sqlconn.Open();
			}
			catch (Exception ex)
			{
				string s = ex.Message;
			}

		}

		public static DataAccess Instance
		{
			get
			{
				return instance;
			}
		}
		#endregion
		private string _ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DKConnectionstring"].ConnectionString;
		SqlConnection sqlconn = new SqlConnection();
		public string ConnectionString
		{
			get
			{
				return _ConnectionString;
			}
			set
			{
				_ConnectionString = value;
			}
		}
		public SqlConnection SqlConn
		{
			get { return sqlconn; }
		}
		public SqlDataReader GetDataReader(string _storedprocedure, params SqlParameter[] sqlparams)
		{
			SqlDataReader sqlreader = null;
			if (sqlconn.State == ConnectionState.Open)
			{
				SqlCommand sqlcomm = new SqlCommand();
				int i;
				try
				{

					sqlcomm.Connection = sqlconn;
					sqlcomm.CommandType = CommandType.StoredProcedure;
					sqlcomm.CommandText = _storedprocedure;

					for (i = 0; i < sqlparams.Length; i++)
					{
						sqlcomm.Parameters.Add(sqlparams[i]);

						if (sqlparams[i].Direction == ParameterDirection.Output)
						{
							sqlparams[i].Direction = ParameterDirection.Output;
						}

					}


					sqlreader = sqlcomm.ExecuteReader();

				}
				catch (Exception ex)
				{
					Reconnect(ex.Message);
					throw ex;

				}
				finally
				{
					sqlcomm = null;

				}

			}
			else
			{
				if (sqlconn.State == ConnectionState.Closed) Reconnect("");
				if (sqlconn.State == ConnectionState.Broken) Reconnect("");
				throw new System.ArgumentException("Database Error in Connection");
			}


			return sqlreader;
		}

		public int SqlExecuteNonQuery(string _storeprocedure, params SqlParameter[] myparams)
		{
			int ret = 0;
			if (sqlconn.State == ConnectionState.Open)
			{
				SqlCommand sqlcomm = new SqlCommand();

				try
				{
					sqlcomm.Connection = sqlconn;
					sqlcomm.CommandType = CommandType.StoredProcedure;
					sqlcomm.CommandText = _storeprocedure;

					for (int i = 0; i < myparams.Length; i++)
					{
						sqlcomm.Parameters.Add(myparams[i]);

						if (myparams[i].Direction == ParameterDirection.Output)
						{
							myparams[i].Direction = ParameterDirection.Output;
						}

					}

					return sqlcomm.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					Reconnect(ex.Message);
					throw ex;
				}
				finally
				{
					sqlcomm = null;
				}

			}
			else
			{
				if (sqlconn.State == ConnectionState.Closed) Reconnect("");
				if (sqlconn.State == ConnectionState.Broken) Reconnect("");
			}

			return ret;

		}
		public object SqlExecuteScalar(string _storeprocedure, params SqlParameter[] myparams)
		{
			object obj = 0;
			if (sqlconn.State == ConnectionState.Open)
			{

				SqlCommand sqlcomm = new SqlCommand();

				try
				{
					if (sqlconn.State == ConnectionState.Closed)
						sqlconn.Open();

					sqlcomm.Connection = sqlconn;
					sqlcomm.CommandType = CommandType.StoredProcedure;
					sqlcomm.CommandText = _storeprocedure;
					for (int i = 0; i < myparams.Length; i++)
					{
						sqlcomm.Parameters.Add(myparams[i]);

						if (myparams[i].Direction == ParameterDirection.Output)
						{
							myparams[i].Direction = ParameterDirection.Output;
						}

					}
					return sqlcomm.ExecuteScalar();
				}
				catch (Exception ex)
				{
					Reconnect(ex.Message);
					throw ex;
				}
				finally
				{
					sqlcomm = null;
				}
			}
			else
			{
				if (sqlconn.State == ConnectionState.Closed) Reconnect("");
				if (sqlconn.State == ConnectionState.Broken) Reconnect("");
				return obj;
			}


		}
		private void Reconnect(string ex)
		{

			sqlconn = null;
			string connString = ConnectionString;
			try
			{
				sqlconn = new SqlConnection(connString);
				sqlconn.Open();
			}
			catch
			{

			}

		}
		public string SqlReturnToJson(string storeprocedure, List<SqlParameter> myparams)
		{
			SqlDataReader sqlreader = null;

			string sjson = "";
			string coma = "";

			object obj = 0;
			if (sqlconn.State == ConnectionState.Open)
			{

				SqlCommand sqlcomm = new SqlCommand();

				try
				{
					if (sqlconn.State == ConnectionState.Closed)
						sqlconn.Open();

					sqlcomm.Connection = sqlconn;
					sqlcomm.CommandType = CommandType.StoredProcedure;
					sqlcomm.CommandText = storeprocedure;

					for (int i = 0; i < myparams.Count; i++)
					{
						sqlcomm.Parameters.Add(myparams[i]);

						if (myparams[i].Direction == ParameterDirection.Output)
						{
							myparams[i].Direction = ParameterDirection.Output;
						}
					}

					sqlreader = sqlcomm.ExecuteReader();

					while (sqlreader.Read() == true)
					{
						List<Dictionary<string, string>> cmdLD = new List<Dictionary<string, string>>();
						string srow = "";
						string fldcoma = "";
						for (int col = 0; col < sqlreader.FieldCount; col++)
						{
							string fn = sqlreader.GetName(col).ToString();
							string vn = sqlreader[fn].ToString();

							srow = srow + fldcoma + "\"" + fn + "\":\"" + vn + "\"";
							fldcoma = ",";
						}
						sjson = sjson + coma + "{" + srow + "}";
						coma = ",";
					}
					sqlreader.Close();
					sjson = "{\"" + storeprocedure + "\":[" + sjson + "]}";

				}
				catch (Exception ex)
				{
					sjson = "{\"error\":\"" + ex.Message + "\"}";
					throw ex;
				}
				finally
				{
					sqlcomm = null;
					//sjson = "{\"empty\":\"empty\"}";
				}
			}
			else
			{
				string constate = "cannot connect ";
				if (sqlconn.State == ConnectionState.Closed) { constate = constate + "Closed"; }
				if (sqlconn.State == ConnectionState.Broken) { constate = constate + "Broken"; }
				if (sqlconn.State == ConnectionState.Connecting) { constate = constate + "Connecting"; }
				if (sqlconn.State == ConnectionState.Executing) { constate = constate + "Executing"; }
				if (sqlconn.State == ConnectionState.Fetching) { constate = constate + "Fetching"; }


				sjson = "{\"Connection Failed\":\"" + constate + "\"}";
			}

			return sjson;
		}
	}
}