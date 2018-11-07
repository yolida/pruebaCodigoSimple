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
            FacturaXml documentoElectronico = new FacturaXml();
            _documentoXml = (IDocumentoXml)documentoElectronico;

            Serializador serializador = new Serializador();
            _serializador = (ISerializador)serializador;
        }

        public ComunicacionBaja data(List<Data_Documentos> data_Documentos)
        {
            Data_Contribuyente data_Contribuyente   =   new Data_Contribuyente(data_Documentos[0].IdDatosFox);
            data_Contribuyente.Read_Contribuyente();
            ReadGeneralData  readGeneralData    =   new ReadGeneralData();

            DateTime dateTime   =   DateTime.Now;
            int numeracion      =   0;

            //numeracion  =   readGeneralData.GetScalarValueINT("", "", );

            var comunicacionBaja    =   new ComunicacionBaja() {
                IdDocumento =   $"RA-{dateTime.Year}{dateTime.Month}{dateTime.Day}-{numeracion}",

            };


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
