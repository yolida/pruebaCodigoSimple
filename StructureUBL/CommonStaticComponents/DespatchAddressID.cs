using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureUBL.CommonStaticComponents
{
    public class DespatchAddressID: Attributes
    {
        /// <summary>
        /// Catálogo No. 13
        /// </summary>
        public string Value { get; set; }

        public DespatchAddressID()
        {
            SchemeAgencyID  = "PE:INEI";
            SchemeName      = "Ubigeos";
        }
    }
}
