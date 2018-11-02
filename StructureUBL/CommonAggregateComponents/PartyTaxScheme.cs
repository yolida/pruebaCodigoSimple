using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class PartyTaxScheme
    {
        /*-----------------------------------------------------------------------------------------------------
        AccountingSupplierParty
            RegistrationName = Nombre o razón social del emisor

        AccountingCustomerParty
            RegistrationName = Número de RUC del adquirente o usuario 
        -----------------------------------------------------------------------------------------------------*/
        public string RegistrationName { get; set; }

        public CompanyID CompanyID { get;set; }

        public RegistrationAddress RegistrationAddress { get; set; }

        public PartyTaxScheme()
        {
            CompanyID           = new CompanyID();
            RegistrationAddress = new RegistrationAddress();
        }
    }
}
