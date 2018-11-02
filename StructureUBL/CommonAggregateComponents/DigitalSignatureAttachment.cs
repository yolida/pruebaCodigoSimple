using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class DigitalSignatureAttachment
    {
        public ExternalReference ExternalReference { get; set; }

        public DigitalSignatureAttachment()
        {
            ExternalReference = new ExternalReference();
        }
    }
}
