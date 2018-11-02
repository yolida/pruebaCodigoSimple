using StructureUBL.CommonStaticComponents;
using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class DiscrepancyResponse : IEquatable<DiscrepancyResponse>
    {
        /// <summary>
        /// [0..1]  
        /// </summary>
        public string       ReferenceID { get; set; }

        /// <summary>
        ///  [0..1] | Código de tipo de nota de crédito
        ///  discrepancia.Tipo de la tabla DiscrepanciaDocumento ref TipoDiscrepancia
        /// </summary>
        public ResponseCode ResponseCode { get; set; }

        /// <summary>
        /// Motivo o Sustento | /CreditNote/cac:DiscrepancyResponse/cbc:Description
        /// discrepancia.Descripcion
        /// </summary>
        public string Description { get; set; }

        public DiscrepancyResponse()
        {
            ReferenceID     =   string.Empty;
            ResponseCode    =   new ResponseCode();
        }

        public bool Equals(DiscrepancyResponse other)
        {
            if (string.IsNullOrEmpty(ReferenceID))
                return false;

            return ReferenceID.Equals(other.ReferenceID);
        }

        public override int GetHashCode()
        {
            if (string.IsNullOrEmpty(ReferenceID))
                return base.GetHashCode();

            return ReferenceID.GetHashCode();
        }
    }
}
