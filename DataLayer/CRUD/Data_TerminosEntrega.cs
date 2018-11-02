using Models.Modelos;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer.CRUD
{
    public class Data_TerminosEntrega: TerminosEntrega
    {
        public Data_TerminosEntrega(int idTerminosEntrega)
        {
            IdTerminosEntrega = idTerminosEntrega;
        }

        public int IdTerminosEntrega { get; set; }

        public void Read_TerminosEntrega()
        {
            string storedProcedure  = "[dbo].[Read_TerminosEntrega]";
            Connection connection   = new Connection();
            SqlCommand sqlCommand   = new SqlCommand();
            sqlCommand.CommandText  = storedProcedure;
            sqlCommand.CommandType  = CommandType.StoredProcedure;
            sqlCommand.Connection   = connection.connectionString;

            SqlParameter paramIdTerminosEntrega = new SqlParameter();
            paramIdTerminosEntrega.SqlDbType     = SqlDbType.Int;
            paramIdTerminosEntrega.ParameterName = "@IdTerminosEntrega";
            paramIdTerminosEntrega.Value         = IdTerminosEntrega;
            sqlCommand.Parameters.Add(paramIdTerminosEntrega);

            connection.Connect();

            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    NumeroRegistro  =   reader["NumeroRegistro"].ToString();
                    TipoMoneda      =   reader["TipoMoneda"].ToString();
                    Monto           =   Convert.ToDecimal(reader["Monto"].ToString());
                    Direccion       =   reader["Direccion"].ToString();
                    Urbanizacion    =   reader["Urbanizacion"].ToString();
                    Provincia       =   reader["Provincia"].ToString();
                    Departamento    =   reader["Departamento"].ToString();
                    Ubigeo          =   reader["Ubigeo"].ToString();
                    Distrito        =   reader["Distrito"].ToString();
                    Alfa2           =   reader["Alfa2"].ToString().Trim();
                }
            }

            connection.Disconnect();
        }
    }
}
