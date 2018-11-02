using System;
using System.Collections.Generic;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class Party
    {
        public PartyName            PartyName { get; set; } // Nombre Comercial del emisor [0..*]

        public PartyTaxScheme       PartyTaxScheme { get; set; }

        public PartyLegalEntity     PartyLegalEntity { get; set; }

        public PostalAddress        PostalAddress { get; set; }

        public PartyIdentification  PartyIdentification { get; set; }
        
        public Party()
        {
            PartyName           = new PartyName();
            PartyTaxScheme      = new PartyTaxScheme();
            PostalAddress       = new PostalAddress();
            PartyLegalEntity    = new PartyLegalEntity();
            PartyIdentification = new PartyIdentification();
        }
    }
}