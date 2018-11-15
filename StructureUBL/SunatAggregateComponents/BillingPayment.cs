using System;
using StructureUBL.CommonBasicComponents;

namespace StructureUBL.SunatAggregateComponents
{
    [Serializable]
    public class BillingPayment
    {
        // Serie y número de comprobante del anticipo (para el caso de reorganización de empresas, incluye el RUC)
        public PartyIdentificationId Id     { get; set; }

        public PayableAmount    PaidAmount  { get; set; }

        /// <summary>
        /// Identificación del tipo de importe total
        /// <para>01: Valor de venta de las operaciones gravadas con el IGV</para>
        /// <para>02: Valores de venta de las operaciones exoneradas del IGV</para> 
        /// <para>03: Valores de venta de las operaciones inafectas del IGV</para> 
        /// <para>04: Valor de venta de las exportaciones del item</para>
        /// <para>05: Valor de venta de las operaciones gratuitas</para>
        /// </summary>
        public string       InstructionId   { get; set; }

        public BillingPayment()
        {
            PaidAmount  =   new PayableAmount();
            Id          =   new PartyIdentificationId();
        }
    }
}
