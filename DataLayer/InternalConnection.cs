using DataLayer.CRUD;
using System;
using System.Data.SqlClient;
using System.IO;

namespace DataLayer
{
    public class InternalConnection
    {
        //(LocalDB)\\MSSQLLocalDB
        public SqlConnection connectionString = new SqlConnection("Data Source=.; Integrated Security=true; AttachDbFilename=|DataDirectory|\\InternalDB\\InternalDB.mdf;Trusted_Connection = True; User Instance=false; ");

        /// <summary>
        /// Método para obtener el path de la instancia de FEI2, la instalación donde se encuentre
        /// </summary>
        /// <returns></returns>
        public string GetPathMDF()
        {
            DirectoryInfo   directory   =   new DirectoryInfo(Environment.CurrentDirectory);
            DirectoryInfo   parentDirectory;

            string ruta         =   string.Empty;
            string directorio   =   "FEICONT";
            
            if (directory.Name == directorio)
                ruta    =   directory.FullName;
            else
            {
                if (directory.Parent.Name == directorio)
                {
                    parentDirectory =   directory.Parent;
                    ruta            =   parentDirectory.FullName;
                }
                else
                {
                    if (directory.Parent.Parent.Name == directorio)
                    {
                        parentDirectory =   directory.Parent.Parent;
                        ruta            =   parentDirectory.FullName;
                    }
                    else
                    {
                        if (directory.Parent.Parent.Parent.Name == directorio)
                        {
                            parentDirectory =   directory.Parent.Parent.Parent;
                            ruta            =   parentDirectory.FullName;
                        }
                        else
                            ruta    =   "No se puede encontrar la ruta.";
                    }
                }
            }
            return ruta;
        }

        public string[] GetDataFile()
        {
            string ruta =   string.Empty;
            string[] Datos;

            string LecturaDatos = string.Empty;
            try
            {
                ruta                        =   Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\FEICONT\access.txt";
                StreamReader streamReader   =   new StreamReader(ruta);
                LecturaDatos                =   streamReader.ReadLine();
                streamReader.Close();
            }
            catch (Exception ex)
            {
                // No se pudo acceder al archivo txt (Agregar notificación)
            }

            try
            {
                Datos           = LecturaDatos.Split('|');
            }
            catch (Exception ex)
            {
                //  Agregar validación
                Datos   = new string[3] { string.Empty, string.Empty, string.Empty };
            }
            return Datos;
        }

        public bool Create_DataAccess(string[] datos)
        {
            InternalAccess internalAccess = new InternalAccess() {
                Servidor    = datos[0].Trim(),
                Usuario     = datos[1].Trim(),
                Contrasenia = datos[2].Trim()
            };

            //return internalAccess.Alter_InternalAccess("[dbo].[Create_DataAccess]");
            return internalAccess.Alter_InternalAccess();
        }
        
        public void Connect()
        {
            try
            {
                connectionString.Open();
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public void Disconnect()
        {
            try
            {
                connectionString.Close();
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
    }
}
