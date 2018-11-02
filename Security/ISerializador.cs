using System.Threading.Tasks;
using Common;
using Models.Intercambio;

namespace Security
{
    public interface ISerializador
    {
        Task<string> GenerarXml<T>(T objectToSerialize) where T : IEstructuraXml;
        string GenerarXml(IEstructuraXml objectToSerialize, bool sincrono);
        Task<string> GenerarZip(string tramaXml, string nombreArchivo);
        string GenerarZip(string tramaXml, string nombreArchivo, bool sincrono);
        Task<EnviarDocumentoResponse> GenerarDocumentoRespuesta(string constanciaRecepcion);
        EnviarDocumentoResponse GenerarDocumentoRespuesta(string constanciaRecepcion, bool sincrono);
    }
}
