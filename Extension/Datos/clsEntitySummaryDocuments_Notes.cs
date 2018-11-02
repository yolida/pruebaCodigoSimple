using Extension.Base;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Datos
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsEntitySummaryDocuments_Notes")]
    public class clsEntitySummaryDocuments_Notes : clsBaseEntidad
    {
        public string Cs_pr_SummaryDocuments_Notes_Id { get; set; }
        public string Cs_pr_SummaryDocuments_Id { get; set; }
        public string Cs_tag_Note { get; set; }
        //private clsEntityDatabaseLocal localDB;
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_SummaryDocuments_Notes_Id);
            cs_cmValores.Add(Cs_pr_SummaryDocuments_Id);
            cs_cmValores.Add(Cs_tag_Note);
        }

        public clsEntitySummaryDocuments_Notes(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_SummaryDocuments_Notes";
            cs_cmCampos.Add("cs_SummaryDocuments_Notes_Id");
            cs_cmCampos.Add("cs_SummaryDocuments_Id");
            for (int i = 1; i <= 1; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
            cs_cmTabla_min = "cs_SummaryDocuments_Notes";
            cs_cmCampos_min.Add("cs_SummaryDocuments_Notes_Id");
            cs_cmCampos_min.Add("cs_SummaryDocuments_Id");
            for (int i = 1; i <= 1; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }

        public clsEntitySummaryDocuments_Notes()
        {
           // localDB = local;
            cs_cmTabla = "cs_SummaryDocuments_Notes";
            cs_cmCampos.Add("cs_SummaryDocuments_Notes_Id");
            cs_cmCampos.Add("cs_SummaryDocuments_Id");
            for (int i = 1; i <= 1; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
            cs_cmTabla_min = "cs_SummaryDocuments_Notes";
            cs_cmCampos_min.Add("cs_SummaryDocuments_Notes_Id");
            cs_cmCampos_min.Add("cs_SummaryDocuments_Id");
            for (int i = 1; i <= 1; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        public clsEntitySummaryDocuments_Notes cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_SummaryDocuments_Notes_Id = valores[0];
                Cs_pr_SummaryDocuments_Id = valores[1];
                Cs_tag_Note = valores[2];
                return this;
            }
            else
            {
                return null;
            }
            
        }

        public List<clsEntitySummaryDocuments_Notes> cs_fxObtenerTodoPorSummaryId(string id)
        {
            var resultado = new List<clsEntitySummaryDocuments_Notes>();
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_SummaryDocuments_Id =" + id.ToString().Trim() + " ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    int contador = 0;
                    var item = new clsEntitySummaryDocuments_Notes(localDB);
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        prop.SetValue(item, Convert.ChangeType(datos[contador].ToString(), prop.PropertyType), null);
                        contador++;
                    }
                    resultado.Add(item);
                }
                cs_pxConexion_basedatos.Close();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments_Notes cs_fxObtenerTodoPorSummaryId " + ex.ToString());
            }
            return resultado;
        }
    }
}
