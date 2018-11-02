using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extension.Base;
using System.Windows;
using System.Runtime.InteropServices;


namespace Extension.Base
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsBaseMensaje")]
    public class clsBaseMensaje
    {
        /// <summary>
        /// Metodo para mostrar el error en un messageBox segun el codigo dado.
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="ex"></param>
        public static void cs_pxMsgEr(string codigo, string ex)
        {
            string contenido = "";
            string titulo = "";
            switch (codigo)
            {
                case "ERR1":
                    contenido = "Se ha producido un error al intentar obtener el archivo de configuración.";
                    titulo = "Error - Archivo de configuración";
                    break;
                case "ERR3":
                    contenido = "Se ha producido un error al intentar actualizar el archivo de configuración.";
                    titulo = "Error - Archivo de configuración";
                    break;
                case "ERR2":
                    contenido = "Se ha producido un error durante la creación de la base de datos; verifique los parámetros de conexión y vuelva a intentarlo.";
                    titulo = "Error - Base de datos";
                    break;
                case "ERR5":
                    contenido = "Se ha producido un error al intentar guardar los cambios.";
                    titulo = "Error - Guardar cambios";
                    break;
                case "ERR6":
                    contenido = "Se ha producido un error al intentar localizar la entidad.";
                    titulo = "Error - Obtener resultado";
                    break;
                case "ERR7":
                    contenido = "Se ha producido un error al intentar eliminar el registro.";
                    titulo = "Error - Eliminar registro";
                    break;
                case "ERR8":
                    contenido = "Se ha producido un error generar los documentos electrónicos.";
                    titulo = "Error - Generar documento electrónico";
                    break;
                case "ERR9":
                    contenido = "Se ha producido un error al intentar activar su producto.\nPor favor vuelva a intentarlo más tarde o ingrese un código de activación válido.";
                    titulo = "Error - Activación de producto";
                    break;
                case "ERR10":
                    contenido = "Se ha producido un error al intentar conectarse a la base de datos.\nVerificar que los parámetros de conexión sean los correctos o contacte con el administrador de base de datos.";
                    titulo = "Error - Conexión a la base de datos";
                    break;
                case "ERR11":
                    contenido = "Se ha producido un error al intentar conectarse al servidor de SUNAT.\nPor favor vuelva a intentarlo más tarde.";
                    titulo = "Error - Conexión con SUNAT";
                    break;
                case "ERR12":
                    contenido = "Ingrese un nombre usuario y/o una contraseña que sean válidos.";
                    titulo = "Error - Inicio de sesión";
                    break;
                case "ERR13":
                    contenido = "No deben existir campos vacíos.";
                    titulo = "Error - Guardar cambios";
                    break;
                case "ERR14":
                    contenido = "Los archivos de reporte no se han generado. \n(Asegúrese que no tenga abierto el archivo)";
                    titulo = "Error - Reporte";
                    break;
                case "ERR15":
                    contenido = "Se ha producido un error desconocido.";
                    titulo = "Error - Envío de documentos electrónicos";
                    break;
                case "ERR16":
                    contenido = "Cierre el documento electrónico que está usando o realice una copia del mismo en otro directorio antes de volver a generar.";
                    titulo = "Error - Generar documentos electrónico";
                    break;
                case "ERR17":
                    contenido = "La ubicación o la contraseña del certificado digital (*.pfx) no es válido.";
                    titulo = "Error - Archivo de configuración";
                    break;
                case "ERR18":
                    contenido = "No dispone de los permisos necesarios para realizar esta acción.";
                    titulo = "Error - Archivo de configuración";
                    break;
                case "ERR19":
                    contenido = "Debe registrar una descripción del item que sea válida.";
                    titulo = "Error - Sistema";
                    break;
                case "ERR20":
                    contenido = "Ya se está ejecutando la aplicación.";
                    titulo = "Error - Sistema";
                    break;
                case "ERR21":
                    contenido = "Se ha detectado un problema con la conexión a internet.";
                    titulo = "Error - Sistema";
                    break;
                case "ERR22":
                    contenido = "No puede eliminar la cuenta principal.";
                    titulo = "Error - Sistema";
                    break;
            }
            //MessageBox.Show(new Form() { TopMost = true }, contenido, titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
            clsBaseLog.cs_pxRegistar(ex);
        }

        /// <summary>
        /// Metodo para mostrar mensaje de exito en operaciones segun codigo.
        /// </summary>
        /// <param name="codigo"></param>
        public static void cs_pxMsgOk(string codigo)
        {
            string contenido = "";
            string titulo = "";
            switch (codigo)
            {
                case "OKE3":
                    contenido = "El archivo de configuración se actualizó correctamente.";
                    titulo = "Correcto - Archivo de configuración";
                    break;
                case "OKE4":
                    contenido = "La base de datos se creó correctamente.";
                    titulo = "Correcto - Base de datos";
                    break;
                case "OKE5":
                    contenido = "Los cambios se han guardado correctamente.";
                    titulo = "Correcto - Guardar cambios";
                    break;
                case "OKE7":
                    contenido = "El registro se ha eliminado correctamente.";
                    titulo = "Correcto - Eliminar registro";
                    break;
                case "OKE8":
                    contenido = "El proceso ha finalizado correctamente.";
                    titulo = "Correcto - Generar documento electrónico";
                    break;
                case "OKE9":
                    contenido = "Su producto se ha activado correctamente.\nDebe volver a iniciar el sistema.";
                    titulo = "Correcto - Activación de producto";
                    break;
                case "OKE10":
                    contenido = "Se ha establecido la conexión con la base de datos correctamente.";
                    titulo = "Correcto - Conexión a la base de datos";
                    break;
                case "OKE12":
                    contenido = "Los archivos de reporte se han generado correctamente.";
                    titulo = "Correcto - Reporte";
                    break;
            }
            //MessageBox.Show(new Form() { TopMost = true }, contenido, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// Metodo para mostrar un MessageBox segun titulo y contenido.
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="contenido"></param>
        public static void cs_pxMsg(string titulo, string contenido)
        {
            //MessageBox.Show(new Form() { TopMost = true }, contenido, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// Metodo para mostrar un MessageBox segun titulo y contenido.
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="contenido"></param>
        public static void cs_pxMsgInformation(string titulo, string contenido)
        {
            //MessageBox.Show(new Form() { TopMost = true }, contenido, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// Metodo para mostrar mensaje de error
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="contenido"></param>
        public static void cs_pxMsgError(string titulo, string contenido)
        {
            //MessageBox.Show(new Form() { TopMost = true }, contenido, titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// Metodo para mostrar mensaje de advertencia
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="contenido"></param>
        public static void cs_pxMsgAdvertencia(string titulo, string contenido)
        {
            //MessageBox.Show(new Form() { TopMost = true }, contenido, titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
