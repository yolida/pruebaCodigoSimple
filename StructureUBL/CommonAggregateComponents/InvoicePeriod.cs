using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class InvoicePeriod
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
