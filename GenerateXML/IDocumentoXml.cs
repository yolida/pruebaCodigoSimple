using Common;
using Models.Contratos;

namespace GenerateXML
{
    public interface IDocumentoXml
    {
        IEstructuraXml Generar(IDocumentoElectronico request);
    }
}
