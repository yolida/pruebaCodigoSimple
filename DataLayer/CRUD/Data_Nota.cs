using Models.Modelos;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.CRUD
{
    public class Data_Nota: Nota
    {
        public Data_Nota(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Id es el Id de Cabecera de documento cuando se quiera ver la nota(s) de todo el documento
        /// Id es el Id de DetalleDocumento cuando se quiera ver la nota(s) de una linea o un detalle de documento
        /// </summary>
        public int Id { get; set; }
        
        public List<Nota> Read(int tipo)
        {
            List<Nota>  notas                   =   new List<Nota>();
            ReadGeneralData readGeneralData     =   new ReadGeneralData();
            DataTable dataTable;

            switch (tipo)
            {   //  Sí es de tipo 1 es porque se trata de la nota del documento, sí de tipo 2 es porque se trata de la nota por cada detalle de documento
                case 1:
                    dataTable   =   readGeneralData.GetDataTable("[dbo].[Read_Nota_Total]", "@IdCabeceraDocumento", Id);
                    break;
                case 2:
                    dataTable   =   readGeneralData.GetDataTable("[dbo].[Read_Nota_Linea]", "@IdDocumentoDetalle",  Id);
                    break;
                default:
                    dataTable   =   new DataTable();
                    break;
            }

            Nota nota;
            DataRow row;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                row     =   dataTable.Rows[i];
                nota    =   new Nota() {
                    IdNota      =   Convert.ToInt32(row["IdNota"].ToString()),
                    Descripcion =   row["Descripcion"].ToString(),
                    Codigo      =   row["Codigo"].ToString()
                };
                notas.Add(nota);
            }

            return notas;
        }
    }
}
