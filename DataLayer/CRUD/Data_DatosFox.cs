using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Models.Modelos;

namespace DataLayer.CRUD
{
    public class Data_DatosFox
    {
        public Int16    IdDatosFox      { get; set; }
        public string   NombreModulo    { get; set; }
        public string   CodigoEmpresa   { get; set; }
        public string   Anio            { get; set; }
        public string   Ruta            { get; set; }
        public int      IdEmisor        { get; set; }
        public Data_DatosFox(Int16 idDatosFox)
        {
            IdDatosFox  =   idDatosFox;
        }

        public void Read_DatosFox()
        {
            string storedProcedure  = "[sysfox].[Read_DatosFox]";
            Connection connection   = new Connection();
            SqlCommand sqlCommand   = new SqlCommand();
            sqlCommand.CommandText  = storedProcedure;
            sqlCommand.CommandType  = CommandType.StoredProcedure;
            sqlCommand.Connection   = connection.connectionString;

            SqlParameter paramIdDatosFox  = new SqlParameter();
            paramIdDatosFox.SqlDbType     = SqlDbType.SmallInt;
            paramIdDatosFox.ParameterName = "@IdDatosFox";
            paramIdDatosFox.Value         = IdDatosFox;
            sqlCommand.Parameters.Add(paramIdDatosFox);

            connection.Connect();
            
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    NombreModulo    = reader["NombreModulo"].ToString();
                    CodigoEmpresa   = reader["CodigoEmpresa"].ToString();
                    Ruta            = reader["Ruta"].ToString();
                    IdEmisor        = Convert.ToInt32(reader["IdEmisor"].ToString());
                }
            }

            connection.Disconnect();
        }
    }
}
