using Extension.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Datos
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsEntityRange")]
    public class clsEntityRange : clsBaseEntidad
    {
        public string Cs_pr_Range_Id { get; set; }
        public string Cs_pr_Periodo { get; set; }
        public string Cs_pr_Declarant_Id { get; set; }
        //private clsEntityDatabaseLocal localDB;
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Range_Id);
            cs_cmValores.Add(Cs_pr_Periodo);
            cs_cmValores.Add(Cs_pr_Declarant_Id);
        }

        public clsEntityRange(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Range";
            cs_cmCampos.Add("cs_Range_Id");
            for (int i = 1; i <= 2; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Range";
            cs_cmCampos_min.Add("cs_Range_Id");
            for (int i = 1; i <= 2; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }

        public clsEntityRange cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Range_Id = valores[0];
                Cs_pr_Periodo = valores[1];
                Cs_pr_Declarant_Id = valores[2];
                return this;
            }
            else
            {
                return null;
            }
           
        }
    }
}
