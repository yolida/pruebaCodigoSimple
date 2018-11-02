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
    [ProgId("clsEntityDocument_Advance")]
    public class clsEntityDocument_Advance : clsBaseEntidad
    {
        public string Cs_pr_Document_Advance_Id { get; set; }
        public string Cs_pr_Document_Id { get; set; }
        public string Cs_pr_TagId { get; set; }
        public string Cs_pr_TagPaidAmount { get; set; }
        public string Cs_pr_InstructionID { get; set; }
        public string Cs_pr_TagPrepaidAmount { get; set; }
        public string Cs_pr_Schema_ID { get; set; }
        public string Cs_pr_Currency_ID { get; set; }
        public string Cs_pr_Instruction_Schema_ID { get; set; }

        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Document_Advance_Id);
            cs_cmValores.Add(Cs_pr_Document_Id);
            cs_cmValores.Add(Cs_pr_TagId);
            cs_cmValores.Add(Cs_pr_TagPaidAmount);
            cs_cmValores.Add(Cs_pr_InstructionID);
            cs_cmValores.Add(Cs_pr_TagPrepaidAmount);
            cs_cmValores.Add(Cs_pr_Schema_ID);
            cs_cmValores.Add(Cs_pr_Currency_ID);
            cs_cmValores.Add(Cs_pr_Instruction_Schema_ID);
        }

        public clsEntityDocument_Advance(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Document_Advance";
            cs_cmCampos.Add("cs_Document_Advance_Id");
            cs_cmCampos.Add("cs_Document_Id");
            for (int i = 1; i <= 7; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Document_Advance";
            cs_cmCampos_min.Add("cs_Document_Advance_Id");
            cs_cmCampos_min.Add("cs_Document_Id");
            for (int i = 1; i <= 7; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityDocument_Advance()
        {
            //localDB = local;
            cs_cmTabla = "cs_Document_Advance";
            cs_cmCampos.Add("cs_Document_Advance_Id");
            cs_cmCampos.Add("cs_Document_Id");
            for (int i = 1; i <= 7; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Document_Advance";
            cs_cmCampos_min.Add("cs_Document_Advance_Id");
            cs_cmCampos_min.Add("cs_Document_Id");
            for (int i = 1; i <= 7; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }

        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }

        public clsEntityDocument_Advance cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Document_Advance_Id = valores[0];
                Cs_pr_Document_Id = valores[1];
                Cs_pr_TagId = valores[2];
                Cs_pr_TagPaidAmount = valores[3];
                Cs_pr_InstructionID = valores[4];
                Cs_pr_TagPrepaidAmount = valores[5];
                Cs_pr_Schema_ID=valores[6];
                Cs_pr_Currency_ID = valores[7];
                Cs_pr_Instruction_Schema_ID = valores[8];
                return this;
            }
            else
            {
                return null;
            }

        }

        public bool cs_fxVerificarExistencia(string id)
        {
            try
            {
                int cantidad = 0;
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Document_Id =" + id.ToString().Trim() + ";";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    cantidad++;
                }
                cs_pxConexion_basedatos.Close();

                if (cantidad > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_AdditionalComments cs_fxVerificarExistencia" + ex.ToString());
                return false;
            }
        }

        public List<clsEntityDocument_Advance> cs_fxObtenerRaizXML(string id)
        {
            List<clsEntityDocument_Advance> resultado = new List<clsEntityDocument_Advance>();
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Document_Id =" + id.ToString().Trim() + " AND cp1='';";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                clsEntityDocument_Advance item;
                while (datos.Read())
                {
                    item = new clsEntityDocument_Advance(localDB);
                    item.Cs_pr_Document_Advance_Id = datos[0].ToString();
                    item.Cs_pr_Document_Id = datos[1].ToString();
                    item.Cs_pr_TagId = datos[2].ToString();
                    item.Cs_pr_TagPaidAmount = datos[3].ToString();
                    item.Cs_pr_InstructionID = datos[4].ToString();
                    item.Cs_pr_TagPrepaidAmount = datos[5].ToString();
                    item.Cs_pr_Schema_ID = datos[6].ToString();
                    item.Cs_pr_Currency_ID = datos[7].ToString();
                    item.Cs_pr_Instruction_Schema_ID = datos[8].ToString();
                    resultado.Add(item);
                }
                cs_pxConexion_basedatos.Close();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_Advance cs_fxObtenerRaizXML" + ex.ToString());
            }
            return resultado;
        }

        public List<clsEntityDocument_Advance> cs_fxObtenerTodoPorIdReferencia(string id)
        {
            List<clsEntityDocument_Advance> resultado = new List<clsEntityDocument_Advance>();
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cp1 ='" + id.ToString().Trim() + "';";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                clsEntityDocument_Advance item;
                while (datos.Read())
                {
                    item = new clsEntityDocument_Advance(localDB);
                    item.Cs_pr_Document_Advance_Id = datos[0].ToString();
                    item.Cs_pr_Document_Id = datos[1].ToString();
                    item.Cs_pr_TagId = datos[2].ToString();
                    item.Cs_pr_TagPaidAmount = datos[3].ToString();
                    item.Cs_pr_InstructionID = datos[4].ToString();
                    item.Cs_pr_TagPrepaidAmount = datos[5].ToString();
                    item.Cs_pr_Schema_ID = datos[6].ToString();
                    item.Cs_pr_Currency_ID = datos[7].ToString();
                    item.Cs_pr_Instruction_Schema_ID = datos[8].ToString();
                    resultado.Add(item);
                }
                cs_pxConexion_basedatos.Close();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_AdditionalComments cs_fxObtenerTodoPorIdReferencia " + ex.ToString());
            }
            return resultado;
        }

        public List<clsEntityDocument_Advance> cs_fxObtenerTodoPorCabeceraId(string id)
        {
            List<clsEntityDocument_Advance> resultado = new List<clsEntityDocument_Advance>();
            try
            {

                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Document_Id =" + id.ToString().Trim() + ";";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                clsEntityDocument_Advance item;
                while (datos.Read())
                {
                    item = new clsEntityDocument_Advance(localDB);
                    item.Cs_pr_Document_Advance_Id = datos[0].ToString();
                    item.Cs_pr_Document_Id = datos[1].ToString();
                    item.Cs_pr_TagId = datos[2].ToString();
                    item.Cs_pr_TagPaidAmount = datos[3].ToString();
                    item.Cs_pr_InstructionID = datos[4].ToString();
                    item.Cs_pr_TagPrepaidAmount = datos[5].ToString();
                    item.Cs_pr_Schema_ID = datos[6].ToString();
                    item.Cs_pr_Currency_ID = datos[7].ToString();
                    item.Cs_pr_Instruction_Schema_ID = datos[8].ToString();
                    resultado.Add(item);
                }
                cs_pxConexion_basedatos.Close();

            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_Advance cs_fxObtenerTodoPorCabeceraId " + ex.ToString());
            }
            return resultado;
        }
    }
}
