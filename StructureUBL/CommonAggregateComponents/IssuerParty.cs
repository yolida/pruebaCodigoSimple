using System;
using System.Collections.Generic;

namespace StructureUBL.CommonAggregateComponents
{
    public class IssuerParty
    {
        /// <summary>
        /// cac:PartyIdentification [0..*]  
        /// </summary>
        public List<PartyIdentification> PartyIdentifications { get; set; }
    }
}
