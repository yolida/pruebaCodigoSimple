using System;
using System.Collections.Generic;
using StructureUBL.CommonAggregateComponents;
using StructureUBL.CommonBasicComponents;

namespace StructureUBL.SunatAggregateComponents
{
    [Serializable]
    public class VoidedDocumentsLine
    {
        /// <summary>
        /// Número de ítem
        /// </summary>
        public int      LineId              { get; set; }
        /// <summary>
        /// Tipo de documento - Catálogo 1
        /// </summary>
        public string   DocumentTypeCode    { get; set; }
        /// <summary>
        /// Serie del documento que se va dar de baja
        /// </summary>
        public string   DocumentSerialID    { get; set; }
        /// <summary>
        /// Número correlativo del documento que se va a dar de baja
        /// </summary>
        public string   DocumentNumberID    { get; set; }
        /// <summary>
        /// Motivo de la baja
        /// </summary>
        public string   VoidReasonDescription   { get; set; }
        // A partir de aqui son los datos para el resumen diario.
        /// <summary>
        /// Identificador de la comunicacdión RA-Fecha-####
        /// </summary>
        public string   Id  { get; set; }
        public int      StartDocumentNumberId   { get; set; }
        public int      EndDocumentNumberId     { get; set; }
        public PayableAmount        TotalAmount { get; set; }
        public List<BillingPayment> BillingPayments { get; set; }
        public AllowanceCharge      AllowanceCharge { get; set; }
        public List<TaxTotal>       TaxTotals       { get; set; }
        public AccountingContributorParty AccountingCustomerParty { get; set; }
        public BillingReference BillingReference    { get; set; }
        public int?             ConditionCode       { get; set; }

        public VoidedDocumentsLine()
        {
            TotalAmount             =   new PayableAmount();
            BillingPayments         =   new List<BillingPayment>();
            AllowanceCharge         =   new AllowanceCharge();
            TaxTotals               =   new List<TaxTotal>();
            AccountingCustomerParty =   new AccountingContributorParty();
            BillingReference        =   new BillingReference();
        }
    }
}
