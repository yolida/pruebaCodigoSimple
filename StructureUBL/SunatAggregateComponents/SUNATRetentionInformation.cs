using System;
using StructureUBL.CommonAggregateComponents;
using StructureUBL.CommonBasicComponents;

namespace StructureUBL.SunatAggregateComponents
{
    [Serializable]
    public class SunatRetentionInformation
    {
        public PayableAmount SunatRetentionAmount { get; set; }

        public string SunatRetentionDate { get; set; }

        public PayableAmount SunatNetTotalPaid { get; set; }

        public ExchangeRate ExchangeRate { get; set; }

        public SunatRetentionInformation()
        {
            SunatRetentionAmount = new PayableAmount();
            SunatNetTotalPaid = new PayableAmount();
            ExchangeRate = new ExchangeRate();
        }
    }
}
