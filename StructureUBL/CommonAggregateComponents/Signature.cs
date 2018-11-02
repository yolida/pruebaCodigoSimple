using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class Signature
    {
        public string Id { get; set; }

        public SignatoryParty SignatoryParty { get; set; }

        public DigitalSignatureAttachment DigitalSignatureAttachment { get; set; }

        public Signature()
        {
            SignatoryParty = new SignatoryParty();
            DigitalSignatureAttachment = new DigitalSignatureAttachment();
        }
    }
}
