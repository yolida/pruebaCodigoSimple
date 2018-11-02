using Models.Modelos;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.CRUD
{
    public class Data_PrecioAlternativo: PrecioAlternativo
    {
        ReadGeneralData readGeneralData = new ReadGeneralData();

        public Data_PrecioAlternativo(int idDocumentoDetalle)
        {
            IdDocumentoDetalle = idDocumentoDetalle;
        }

        public int IdDocumentoDetalle { get; set; }

        public List<PrecioAlternativo> Read_PrecioAlternativo() {
            List<PrecioAlternativo> precioAlternativos  =   new List<PrecioAlternativo>();
            DataTable dataTable =   readGeneralData.GetDataTable("[dbo].[Read_PrecioAlternativo]", "@IdDocumentoDetalle", IdDocumentoDetalle);

            DataRow row;
            PrecioAlternativo precioAlternativo;

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                row     =   dataTable.Rows[i];
                precioAlternativo   =   new PrecioAlternativo() {
                    IdPrecioAlternativo     =   Convert.ToInt32(row["IdPrecioAlternativo"].ToString()),
                    Monto                   =   Convert.ToDecimal(row["Monto"].ToString()),
                    TipoMoneda              =   row["TipoMoneda"].ToString(),
                    TipoPrecio              =   row["TipoPrecio"].ToString()                        
                };
                precioAlternativos.Add(precioAlternativo);
            }
            
            return precioAlternativos;
        }
    }
}
