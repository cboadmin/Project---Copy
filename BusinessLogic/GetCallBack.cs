using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBS.BusinessLogic
{
    public class GetCallBack
    {
        // Returns JSON data from SQL
        public string SQLToJson(string storedProc, Dictionary<string, string> mParam)
        {
            string message = "";

			DataAccess da = DataAccess.Instance;

            List<SqlParameter> sqlparamsList = new List<SqlParameter>();

            //Convert dictionary values into SqlParamerter List
            foreach (KeyValuePair<string, string> entry in mParam)
            {
                SqlParameter sqlParameter = SqlHelper.Param(entry.Key, entry.Value);
                sqlparamsList.Add(sqlParameter);
            }



            try
            {

                message = da.SqlReturnToJson(storedProc, sqlparamsList);

            }
            catch (Exception ex)
            {
                message = "{\"error\":\"" + ex.Message + "\"}";
            }



            return message;
        }

    }

	public class SqlHelper
	{
		public static DataAccess da = DataAccess.Instance;
		public static SqlParameter Param(string _name, object _value)
		{
			return new SqlParameter(_name, _value);
		}
		public static SqlParameter Param(string _name, object _value, bool isoutput)
		{
			return new SqlParameter(_name, SqlDbType.Int, 0, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Current, _value);
		}
	}
}
