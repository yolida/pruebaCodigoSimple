using System;
using System.IO;
using System.Windows.Forms;

namespace Common
{
    public class DirectorySaveFiles
    {
        public static string CarpetaXml => "./XML";
        public static string CarpetaCdr => "./CDR";
        public DirectorySaveFiles()
        {
            try
            {
                if (!Directory.Exists(CarpetaXml))
                    Directory.CreateDirectory(CarpetaXml);
                if (!Directory.Exists(CarpetaCdr))
                    Directory.CreateDirectory(CarpetaCdr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.InnerException?.Message, Application.ProductName);
            }
        }
    }
}
