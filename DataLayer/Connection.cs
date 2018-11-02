using System;
using System.Data;
using System.Data.SqlClient;
using DataLayer.CRUD;

namespace DataLayer
{
    public class Connection
    {
        public SqlConnection connectionString = new SqlConnection(GetConnectionString());
        //new SqlConnection(@"data source=DESKTOP-R5K38U6\MSSQLSERVER01" + "" + "; initial catalog=FeiContDB; user id=sa" + "" + "; password=1qaz2wsxM@123" + "" + ";");

        public bool CheckConnection(string cadena)
        {
            bool resultado      =   false;
            SqlConnection sqlConnection     =   new SqlConnection(cadena);

            try
            {
                sqlConnection.Open();
                if (sqlConnection.State ==  ConnectionState.Open )
                    resultado   =   true;
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                resultado   =   false;
            }

            return resultado;
        }

        public static string GetConnectionString()
        {
            InternalAccess internalAccess   =   new InternalAccess();
            internalAccess.Read_InternalAccess();
            return @"data source=" + internalAccess.Servidor + "; initial catalog=FeiContDB; user id=" + internalAccess.Usuario + "; password=" + internalAccess.Contrasenia + ";";
        }

        public void Connect()
        {
            try
            {
                connectionString.Open();
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public void Disconnect()
        {
            try
            {
                connectionString.Close();
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
