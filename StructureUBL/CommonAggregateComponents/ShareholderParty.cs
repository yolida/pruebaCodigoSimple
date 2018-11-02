using System;

namespace StructureUBL.CommonAggregateComponents
{
    public class ShareholderParty
    {
        public Party Party { get; set; }

        public ShareholderParty()
        {
            Party   = new Party();
        }
    }
}
