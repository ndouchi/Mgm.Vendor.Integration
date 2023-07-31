using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Mgm.VI.Common.Db
{
    public class CommandInSql 
    {
        string ConnectionString = "";
        SqlConnection con;

        public void AddParameter(string name, string value, SqlDbType dbType)
        {
            SqlParameter sqlParam = new SqlParameter();
            sqlParam.SqlDbType = dbType;
            sqlParam.ParameterName = name;
            switch (dbType)
            {
                case SqlDbType.SmallInt:
                case SqlDbType.BigInt:
                case SqlDbType.Int:
                case SqlDbType.Float:
                    break;
                default:
                    break;
            }
            sqlParam.Value = value;
        }
        public void OpenConection()
        {
            con = new SqlConnection(ConnectionString);
            con.Open();
        }
        public void CloseConnection()
        {
            con.Close();
        }
        public void ExecuteQueries(string query)
        {
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }
        public SqlDataReader DataReader(string query)
        {
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }
        public object ShowDataInGridView(string query)
        {
            object dataum = null;
            using (SqlDataAdapter dr = new SqlDataAdapter(query, ConnectionString))
            {
                DataSet ds = new DataSet();
                dr.Fill(ds);
                dataum = ds.Tables[0];
            }
            return dataum;
        }
    }
}
