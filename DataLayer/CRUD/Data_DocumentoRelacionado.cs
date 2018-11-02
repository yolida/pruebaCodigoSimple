using Models.Modelos;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.CRUD
{
    public class Data_DocumentoRelacionado: DocumentoRelacionado
    {
        public int IdCabeceraDocumento { get; set; }
        ReadGeneralData readGeneralData = new ReadGeneralData();

        public Data_DocumentoRelacionado(int idCabeceraDocumento)
        {
            IdCabeceraDocumento = idCabeceraDocumento;
        }

        public List<DocumentoRelacionado> Read_DocumentoRelacionado()
        {
            List<DocumentoRelacionado> documentoRelacionados    =   new List<DocumentoRelacionado>();
            DataTable dataTable =   readGeneralData.GetDataTable("[dbo].[Read_DocumentoRelacionado]", "@IdCabeceraDocumento", IdCabeceraDocumento);
            DataRow row;

            DocumentoRelacionado documentoRelacionado;

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                row =   dataTable.Rows[i];
                documentoRelacionado    =   new DocumentoRelacionado();
                documentoRelacionado.IdDocumentoRelacionado     =   Convert.ToInt32(row["IdDocumentoRelacionado"].ToString());
                documentoRelacionado.NroDocumento               =   row["NroDocumento"].ToString();

                if (string.IsNullOrEmpty(row["IdCat_DocRelacionados"].ToString()))
                    documentoRelacionado.TipoDocumentoRelacionado   =   "00";
                else
                {
                    documentoRelacionado.TipoDocumentoRelacionado   =   readGeneralData.GetScalarValueSTRING("[dbo].[Query_Scalar_GetValue_TipoDocumentoRelacionado]",
                                                                        "@IdCat_DocRelacionados", Convert.ToInt32(row["IdCat_DocRelacionados"].ToString()));
                }

                documentoRelacionado.TipoOperacion              =   row["IdTipoOperacion"].ToString() ?? string.Empty;

                string temp = row["IdTipoDocumento"].ToString();

                if (string.IsNullOrEmpty(row["IdTipoDocumento"].ToString()))
                    documentoRelacionado.TipoDocumento  =   "00";
                else
                {
                    documentoRelacionado.TipoDocumento  =   readGeneralData.GetScalarValueSTRING("[dbo].[Query_Scalar_GetValue_TipoDocumento]",
                                                                "@IdTipoDocumento", Convert.ToInt32(row["IdTipoDocumento"].ToString()));
                }
                
                documentoRelacionados.Add(documentoRelacionado);
            }

            return documentoRelacionados;
        }
    }
}
