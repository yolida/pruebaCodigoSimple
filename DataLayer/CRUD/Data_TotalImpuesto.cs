using Models.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer.CRUD
{
    public class Data_TotalImpuesto: TotalImpuesto
    {
        public Data_TotalImpuesto(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Id es el Id de Cabecera de documento cuando se quiera ver el impuesto total de todo el documento
        /// Id es el Id de DetalleDocumento cuando se quiera ver el impuesto total de todo una linea o un detalle de documento
        /// </summary>
        public int Id { get; set; }

        public List<TotalImpuesto> Read_TotalImpuestos(int tipo)
        {
            List<TotalImpuesto> totalImpuestos  =   new List<TotalImpuesto>();
            ReadGeneralData readGeneralData     =   new ReadGeneralData();
            DataTable dataTable;

            switch (tipo)
            {   //  Sí es de tipo 1 es porque se trata del impuesto del documento, sí de tipo 2 es porque se trata de el impuesto por cada detalle de documento
                case 1:
                    dataTable   =   readGeneralData.GetDataTable("[dbo].[Read_TotalImpuestos_Total]",   "@IdCabeceraDocumento", Id);
                    break;
                case 2:
                    dataTable   =   readGeneralData.GetDataTable("[dbo].[Read_TotalImpuestos_Linea]",   "@IdDocumentoDetalle",  Id);
                    break;
                default:
                    dataTable   =   new DataTable();
                    break;
            }

            DataRow row;
            TotalImpuesto totalImpuesto;

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                row     =   dataTable.Rows[i];
                totalImpuesto =   new TotalImpuesto() {
                    IdTotalImpuestos    =   Convert.ToInt32(row["IdTotalImpuestos"].ToString()),
                    MontoTotal          =   Convert.ToDecimal(row["MontoTotal"].ToString()),
                    TipoMonedaTotal     =   row["Moneda"].ToString()
                };
                totalImpuestos.Add(totalImpuesto);
            }

            return totalImpuestos;
        }
    }
}
