using System;
using StructureUBL.CommonAggregateComponents;
using StructureUBL.CommonBasicComponents;

namespace StructureUBL.SunatAggregateComponents
{
    [Serializable]
    public class SunatEmbededDespatchAdvice
    {
        public PostalAddress DeliveryAddress { get; set; }

        public PostalAddress OriginAddress { get; set; }

        public AccountingContributorParty SunatCarrierParty { get; set; }

        public AgentParty DriverParty { get; set; }

        public SunatRoadTransport SunatRoadTransport { get; set; }

        public string TransportModeCode { get; set; }

        public InvoicedQuantity GrossWeightMeasure { get; set; }

        public SunatEmbededDespatchAdvice()
        {
            DeliveryAddress = new PostalAddress();
            OriginAddress = new PostalAddress();
            SunatCarrierParty = new AccountingContributorParty();
            DriverParty = new AgentParty();
            SunatRoadTransport = new SunatRoadTransport();
            GrossWeightMeasure = new InvoicedQuantity();
        }
    }
}
