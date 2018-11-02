using Models.Modelos;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer.CRUD
{
    public class Data_Contribuyente: Contribuyente
    {
        public int IdContribuyente { get; set; }

        public Data_Contribuyente(int idContribuyente)
        {
            IdContribuyente     =   idContribuyente;
        }

        public void Read_Contribuyente()
        {
            string storedProcedure  = "[dbo].[Read_Contribuyente]";
            Connection connection   = new Connection();

            SqlCommand sqlCommand = new SqlCommand
            {
                CommandText =   storedProcedure,
                CommandType =   CommandType.StoredProcedure,
                Connection  =   connection.connectionString
            };

            SqlParameter paramIdContribuyente  = new SqlParameter();
            paramIdContribuyente.SqlDbType     = SqlDbType.Int;
            paramIdContribuyente.ParameterName = "@IdContribuyente";
            paramIdContribuyente.Value         = IdContribuyente;
            sqlCommand.Parameters.Add(paramIdContribuyente);
            
            connection.Connect();
            
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    NroDocumento        =   reader["NroDocumento"].ToString();
                    TipoDocumento       =   reader["TipoDocumentoIdentidad"].ToString();
                    NombreComercial     =   reader["NombreComercial"].ToString();
                    NombreLegal         =   reader["NombreLegal"].ToString();
                    Ubigeo              =   reader["IdUbigeo"].ToString();
                    Direccion           =   reader["Direccion"].ToString();
                    Urbanizacion        =   reader["Urbanizacion"].ToString();
                    Provincia           =   reader["Provincia"].ToString();
                    Departamento        =   reader["Departamento"].ToString();
                    Distrito            =   reader["Distrito"].ToString();
                    Pais                =   reader["IdIdentificacionPais"].ToString();
                    CorreoElectronico   =   reader["CorreoElectronico"].ToString();
                    RegistroMTC         =   reader["RegistroMTC"].ToString();
                }
            }

            connection.Disconnect();
        }
    }
}
