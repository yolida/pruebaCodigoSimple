
using System.Collections.Generic;

namespace StructureUBL.CommonExtensionComponents
{
    public class UBLExtensions
    {
        /// <summary>
        /// ext:UBLExtension
        /// [1..*]
        /// </summary>
        public List<UBLExtension> UBLExtension_List { get; set; }

        public UBLExtensions() {
            UBLExtension_List = new List<UBLExtension>();
        }
    }
}
