using Models.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer.CRUD
{
    public class Data_Descuento
    {
        public Data_Descuento(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
        ReadGeneralData ReadGeneralData =   new ReadGeneralData();

        public List<Descuento> Read_Descuento_Total()
        {
            List<Descuento> descuentos  =   new List<Descuento>();
            DataTable dataTable =   ReadGeneralData.GetDataTable("[dbo].[Read_Descuento_Total]", "@IdCabeceraDocumento", Id);

            Descuento descuento;
            DataRow row;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                row         =   dataTable.Rows[i];
                descuento   =   new Descuento() {
                    Indicador       =   Convert.ToBoolean(row["Indicador"].ToString()),
                    CodigoMotivo    =   row["CodigoMotivo"].ToString(),
                    Factor          =   Convert.ToDecimal(row["Factor"].ToString()),
                    Monto           =   Convert.ToDecimal(row["Monto"].ToString()),
                    Moneda          =   row["Moneda"].ToString(),
                    MontoBase       =   Convert.ToDecimal(row["MontoBase"].ToString()),
                    MonedaBase      =   row["MonedaBase"].ToString()
                };
                descuentos.Add(descuento);
            }

            return descuentos;
        }
    }
}
