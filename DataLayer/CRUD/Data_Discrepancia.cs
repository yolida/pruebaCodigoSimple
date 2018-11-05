using Models.Modelos;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.CRUD
{
    public class Data_Discrepancia
    {
        ReadGeneralData readGeneralData = new ReadGeneralData();
        public Data_Discrepancia(int idCabeceraDocumento)
        {
            IdCabeceraDocumento = idCabeceraDocumento;
        }
        public int IdCabeceraDocumento { get; set; }

        public List<Discrepancia> Read_DiscrepanciaDocumento()
        {
            List<Discrepancia> discrepancias    =   new List<Discrepancia>();
            DataTable dataTable =   readGeneralData.GetDataTable("[dbo].[Read_DiscrepanciaDocumento]", "@IdCabeceraDocumento", IdCabeceraDocumento);
            DataRow row;

            Discrepancia discrepancia;

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                row =   dataTable.Rows[i];
                discrepancia    =   new Discrepancia() {
                    IdDiscrepanciaDocumento =   Convert.ToInt32(row["IdDiscrepanciaDocumento"].ToString()),
                    Tipo                    =   row["TDCodigo"].ToString(),
                    Descripcion             =   row["TDDescripcion"].ToString()
                };
                discrepancias.Add(discrepancia);
            }

            return discrepancias;
        }
    }
}
