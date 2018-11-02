using Models.Modelos;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.CRUD
{
    public class Data_Anticipo: Anticipo
    {
        public Data_Anticipo(int idCabeceraDocumento)
        {
            IdCabeceraDocumento = idCabeceraDocumento;
        }

        public int IdCabeceraDocumento { get; set; }

        public List<Anticipo> Read_Anticipo()
        {
            List<Anticipo> anticipos        =   new List<Anticipo>();
            ReadGeneralData readGeneralData =   new ReadGeneralData();
            DataTable dataTable             =   readGeneralData.GetDataTable("[dbo].[Read_Anticipo]", "@IdCabeceraDocumento", IdCabeceraDocumento);

            DataRow row;
            Anticipo anticipo;

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                row         =   dataTable.Rows[i];
                anticipo    =  new Anticipo() {
                    IdAnticipo          =   Convert.ToInt32(row["IdAnticipo"].ToString()),
                    ComprobanteAnticipo =   row["ComprobanteAnticipo"].ToString(),
                    TipoDocumento       =   row["TipoDocumento"].ToString(),
                    Monto               =   Convert.ToDecimal(row["Monto"].ToString()),
                    Moneda              =   row["Moneda"].ToString(),
                    FechaPago           =   Convert.ToDateTime(row["FechaPago"].ToString())
                };
                anticipos.Add(anticipo);
            }

            return anticipos;
        }
    }
}
