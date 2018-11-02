using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class AccountingContributorParty
    {
        public string CustomerAssignedAccountId { get; set; } // Número de RUC

        /// <summary>
        /// cbc:AdditionalAccountID
        /// CATALOGO No. 06
        /// Tipo de Documento de Identificación
        /// </summary>
        public string AdditionalAccountId { get; set; } // Tipo de documento

        public Party Party { get; set; }
    }
}