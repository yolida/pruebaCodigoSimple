using System;
using System.Collections.Generic;
using StructureUBL.CommonBasicComponents;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class DocumentLine
    {
        public int              IdDocumentLine { get; set; } // Número de orden del Ítem
        public InvoicedQuantity CreditedQuantity { get; set; }
        public InvoicedQuantity InvoicedQuantity { get; set; } // Cantidad de unidades del ítem
        public InvoicedQuantity DebitedQuantity { get; set; }

        /// Valor de venta del ítem
        /// currencyID => Código de tipo de moneda del valor de venta del ítem
        public PayableAmount    LineExtensionAmount { get; set; }

        public PricingReference PricingReference { get; set; } // PricingReference
        public List<AllowanceCharge>  AllowanceCharges { get; set; }
        public List<TaxTotal>   TaxTotals { get; set; }
        public Item             Item { get; set; }
        public Price            Price { get; set; }
        public string           TaxPointDate { get; set; }
        public List<Delivery>   Deliveries { get; set; }

        public DocumentLine()
        {
            CreditedQuantity    = new InvoicedQuantity();
            InvoicedQuantity    = new InvoicedQuantity();
            DebitedQuantity     = new InvoicedQuantity();
            LineExtensionAmount = new PayableAmount();
            PricingReference    = new PricingReference();
            AllowanceCharges    = new List<AllowanceCharge>(); 
            TaxTotals           = new List<TaxTotal>();
            Item                = new Item();
            Price               = new Price();
            Deliveries          = new List<Delivery>();
        }
    }
}
