using DataLayer;
using DataLayer.CRUD;
using GenerateXML;
using Models.Intercambio;
using Models.Modelos;
using Security;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class GenerarComunicacionBaja
    {
        private readonly IDocumentoXml _documentoXml;
        private readonly ISerializador _serializador;

        public GenerarComunicacionBaja()
        {
            ComunicacionBajaXml documentoElectronico = new ComunicacionBajaXml();
            _documentoXml = (IDocumentoXml)documentoElectronico;

            Serializador serializador = new Serializador();
            _serializador = (ISerializador)serializador;
        }

        public ComunicacionBaja data(List<Data_Documentos> data_Documentos)
        {
            Data_Contribuyente data_Emisor   =   new Data_Contribuyente(data_Documentos[0].IdEmisor);
            data_Emisor.Read_Contribuyente();
            ReadGeneralData  readGeneralData    =   new ReadGeneralData();

            DateTime dateTime   =   DateTime.Now;
            int numeracion      =   0;
            string mes  = dateTime.Month    < 10 ? $"0{dateTime.Month}" : dateTime.Month.ToString();
            string dia  = dateTime.Day      < 10 ? $"0{dateTime.Day}"   : dateTime.Day.ToString();
            int numeradorLinea  =   1;

            numeracion  =   readGeneralData.GetScalarValueINT("[dbo].[Query_Scalar_GetValue_CantidadDocsDia]", "@IdDatosFox", data_Documentos[0].IdDatosFox);

            var comunicacionBaja    =   new ComunicacionBaja() {
                IdDocumento     =   $"RA-{dateTime.Year}{mes}{dia}-{numeracion}",
                FechaEmision    =   dateTime.ToString(),            // Cuando se da de baja
                FechaReferencia =   data_Documentos[0].FechaEmision,// Cuando se crea el documento
                Emisor          =   data_Emisor,
                Bajas           =   new List<DocumentoBaja>()
            };

            foreach (var data_Documento in data_Documentos)
            {
                string[] serieCorrelativo = data_Documento.SerieCorrelativo.Split('-');
                comunicacionBaja.Bajas.Add(new DocumentoBaja()
                {
                    Id              =   numeradorLinea,
                    TipoDocumento   =   data_Documento.TipoDocumento,
                    Serie           =   serieCorrelativo[0].ToString(),
                    Correlativo     =   serieCorrelativo[1].ToString(),
                    MotivoBaja      =   data_Documento.MotivoBaja
                });
                numeradorLinea++;
            }
            
            return comunicacionBaja;
        }

        public DocumentoResponse Post(ComunicacionBaja baja)
        {
            var response    =   new DocumentoResponse();

            try
            {
                var voidedDocument          =   _documentoXml.Generar(baja);
                response.TramaXmlSinFirma   =   _serializador.GenerarXml(voidedDocument, true);
                response.Exito              =   true;
            }
            catch (Exception ex)
            {
                response.MensajeError   =   ex.Message;
                response.Pila           =   ex.StackTrace;
                response.Exito          =   false;
            }

            return response;
        }
    }
}
