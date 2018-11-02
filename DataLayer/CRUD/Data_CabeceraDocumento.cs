using Models.Modelos;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer.CRUD
{
    public class Data_CabeceraDocumento: DocumentoElectronico
    {
        public Data_CabeceraDocumento(int idCabeceraDocumento)
        {
            IdCabeceraDocumento = idCabeceraDocumento;
        }

        public int IdCabeceraDocumento { get; set; }

        public int IdReceptor { get; set; }

        public void Read_CabeceraDocumento()
        {
            string storedProcedure  =   "[dbo].[Read_CabeceraDocumento]";
            Connection connection   =   new Connection();
            SqlCommand sqlCommand   =   new SqlCommand();
            sqlCommand.CommandText  =   storedProcedure;
            sqlCommand.CommandType  =   CommandType.StoredProcedure;
            sqlCommand.Connection   =   connection.connectionString;

            SqlParameter paramIdCabeceraDocumento   =   new SqlParameter();
            paramIdCabeceraDocumento.SqlDbType      =   SqlDbType.Int;
            paramIdCabeceraDocumento.ParameterName  =   "@IdCabeceraDocumento";
            paramIdCabeceraDocumento.Value          =   IdCabeceraDocumento;
            sqlCommand.Parameters.Add(paramIdCabeceraDocumento);
            
            connection.Connect();

            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    IdReceptor          =   Convert.ToInt32(reader["IdReceptor"].ToString());
                    FechaEmision        =   Convert.ToDateTime(reader["FechaEmision"].ToString());
                    HoraEmision         =   reader["HoraEmision"].ToString();
                    try
                    {
                        FechaVencimiento    =   Convert.ToDateTime(reader["FechaVencimiento"].ToString());
                    }
                    catch (Exception)
                    {
                        FechaVencimiento    = Convert.ToDateTime("1900-01-01");
                    }
                    OrdenCompra         =   reader["OrdenCompra"].ToString();
                    Moneda              =   reader["CodigoMoneda"].ToString();
                    TipoOperacion       =   reader["CodigoTipoOperacion"].ToString();
                    CantidadItems       =   Convert.ToInt16(reader["CantidadItems"].ToString());
                    TotalValorVenta     =   Convert.ToDecimal(reader["TotalValorVenta"].ToString());
                    TotalPrecioVenta    =   Convert.ToDecimal(reader["TotalPrecioVenta"].ToString());
                    TotalDescuento      =   Convert.ToDecimal(reader["TotalDescuento"].ToString());
                    TotalOtrosCargos    =   Convert.ToDecimal(reader["TotalOtrosCargos"].ToString());
                    TotalAnticipos      =   Convert.ToDecimal(reader["TotalAnticipos"].ToString());
                    ImporteTotalVenta   =   Convert.ToDecimal(reader["ImporteTotalVenta"].ToString());
                }
            }

            connection.Disconnect();
        }
    }
}
