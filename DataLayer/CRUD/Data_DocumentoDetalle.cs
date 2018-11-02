using Models.Modelos;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.CRUD
{
    public class Data_DocumentoDetalle: DetalleDocumento
    {
        ReadGeneralData readGeneralData = new ReadGeneralData();
        public Data_DocumentoDetalle(int idCabeceraDocumento)
        {
            IdCabeceraDocumento = idCabeceraDocumento;
        }

        public int IdCabeceraDocumento { get; set; }
        
        public List<DetalleDocumento> Read_DocumentoDetalle()
        {
            List<DetalleDocumento> detalleDocumentos = new List<DetalleDocumento>();
            
            DataTable dataTableDocumentoDetalle =   readGeneralData.GetDataTable("[dbo].[Read_DocumentoDetalle]", "@IdCabeceraDocumento", IdCabeceraDocumento);
            DataRow row;
            
            DetalleDocumento detalleDocumento;

            for (int i = 0; i < dataTableDocumentoDetalle.Rows.Count; i++)
            {
                row =   dataTableDocumentoDetalle.Rows[i];
                detalleDocumento    =   new DetalleDocumento() {
                    IdDocumentoDetalle  =   Convert.ToInt32(row["IdDocumentoDetalle"].ToString()),
                    NumeroOrden         =   Convert.ToInt32(row["NumeroOrden"].ToString()),
                    ValorVenta          =   Convert.ToDecimal(row["ValorVenta"].ToString()),
                    Moneda              =   row["Moneda"].ToString(),
                    Cantidad            =   Convert.ToDecimal(row["Cantidad"]),
                    UnidadMedida        =   row["UnidadMedida"].ToString(),
                    CodigoItem          =   row["CodigoItem"].ToString() ?? string.Empty,
                    TipoPrecio          =   row["TipoPrecio"].ToString(),
                    Descuento           =   Convert.ToDecimal(row["Descuento"].ToString()),
                    TotalVenta          =   Convert.ToDecimal(row["TotalVenta"].ToString()),
                    CodigoProductoSunat =   row["CodigoProductoSunat"].ToString() ?? string.Empty
                };
                detalleDocumentos.Add(detalleDocumento);
            }

            return detalleDocumentos;
        }

        public List<Descripcion> Read_Descripcion(int IdDocumentoDetalle)
        {
            List<Descripcion> descripciones = new List<Descripcion>();
            DataTable dataTable =   readGeneralData.GetDataTable("[dbo].[Read_Descripcion]", "@IdDocumentoDetalle", IdDocumentoDetalle);
            DataRow row;

            Descripcion descripcion;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                row         =   dataTable.Rows[i];
                descripcion =   new Descripcion() {
                    IdDescripcion   =   Convert.ToInt32(row["IdDescripcion"].ToString()),
                    Detalle         =   row["Detalle"].ToString()
                };
                descripciones.Add(descripcion);
            }
            return descripciones;
        }
    }
}
