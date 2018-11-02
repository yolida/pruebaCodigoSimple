using Models.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer.CRUD
{
    public class Data_SubTotalImpuesto: SubTotalImpuestos
    {
        public Data_SubTotalImpuesto(int idTotalImpuestos)
        {
            IdTotalImpuestos = idTotalImpuestos;
        }

        public int IdTotalImpuestos { get; set; }

        public List<SubTotalImpuestos> Read_SubTotalImpuesto()
        {
            List<SubTotalImpuestos> subTotalImpuestos   =   new List<SubTotalImpuestos>();
            ReadGeneralData readGeneralData =   new ReadGeneralData();
            DataTable dataTable =   readGeneralData.GetDataTable("[dbo].[Read_SubTotalImpuesto]", "@IdTotalImpuestos", IdTotalImpuestos);

            DataRow row;
            SubTotalImpuestos subTotalImpuesto;
            
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                row = dataTable.Rows[i];
                subTotalImpuesto = new SubTotalImpuestos()
                {
                    IdTotalImpuestos    =   Convert.ToInt32(row["IdTotalImpuestos"].ToString()),
                    BaseImponible       =   Convert.ToDecimal(row["BaseImponible"].ToString()),
                    TipoMonedaBase      =   row["MBMoneda"].ToString(),
                    MontoTotal          =   Convert.ToDecimal(row["MontoTotal"].ToString()),
                    TipoMonedaTotal     =   row["MTMoneda"].ToString(),
                    PorcentajeImp       =   Convert.ToDecimal(row["PorcentajeImp"].ToString()),
                    TipoAfectacion      =   row["TipoAfectacion"].ToString(),
                    TipoSistemaISC      =   string.Empty,   // Se considera irrelevante por parte de operaciones
                    CodigoTributo       =   row["CodigoTributo"].ToString(), 
                    CodigoInternacional =   row["CodigoInternacional"].ToString(),
                    NombreTributo       =   row["NombreTributo"].ToString()
                };
                subTotalImpuestos.Add(subTotalImpuesto);
            }

            return subTotalImpuestos;
        }
    }
}
