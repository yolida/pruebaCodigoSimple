using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DataLayer.CRUD
{
    public class InternalAccess
    {
        public string Servidor { get; set; }
        public string Usuario { get; set; }
        public string Contrasenia { get; set; }

        public void Read_InternalAccess_kilter()
        {
            string storedProcedure          =   "[dbo].[Read_DataAccess]";
            InternalConnection connection   =   new InternalConnection();
            SqlCommand sqlCommand           =   new SqlCommand();
            sqlCommand.CommandText          =   storedProcedure;
            sqlCommand.CommandType          =   CommandType.StoredProcedure;
            sqlCommand.Connection           =   connection.connectionString;

            connection.Connect();

            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    Servidor    =   reader["Servidor"].ToString();
                    Usuario     =   reader["Usuario"].ToString();
                    Contrasenia =   reader["Contrasenia"].ToString();             
                }
            }

            connection.Disconnect();
        }

        public void Read_InternalAccess()
        {
            try
            {
                var appSettings =   ConfigurationManager.AppSettings;
                Servidor        =   appSettings["Servidor"].ToString()      ??  string.Empty;
                Usuario         =   appSettings["Usuario"].ToString()       ??  string.Empty;
                Contrasenia     =   appSettings["Contrasenia"].ToString()   ??  string.Empty;

            }
            catch (Exception ex)
            {
            }
        }

        public bool Alter_InternalAccess_kilter(string storedProcedure)
        {   // para la escritura y actualización
            InternalConnection connection   = new InternalConnection();
            SqlCommand sqlCommand           = new SqlCommand();
            sqlCommand.CommandText          = storedProcedure;
            sqlCommand.CommandType          = CommandType.StoredProcedure;
            sqlCommand.Connection           = connection.connectionString;

            SqlParameter paramServidor  = new SqlParameter();
            paramServidor.SqlDbType     = SqlDbType.NVarChar;
            paramServidor.ParameterName = "@Servidor";
            paramServidor.Value         = Servidor;
            sqlCommand.Parameters.Add(paramServidor);

            SqlParameter paramUsuario  = new SqlParameter();
            paramUsuario.SqlDbType     = SqlDbType.NVarChar;
            paramUsuario.ParameterName = "@Usuario";
            paramUsuario.Value         = Usuario;
            sqlCommand.Parameters.Add(paramUsuario);

            SqlParameter paramContrasenia  = new SqlParameter();
            paramContrasenia.SqlDbType     = SqlDbType.NVarChar;
            paramContrasenia.ParameterName = "@Contrasenia";
            paramContrasenia.Value         = Contrasenia;
            sqlCommand.Parameters.Add(paramContrasenia);

            SqlParameter paramComprobacion  = new SqlParameter();
            paramComprobacion.Direction     = ParameterDirection.Output;
            paramComprobacion.SqlDbType     = SqlDbType.Bit;
            paramComprobacion.ParameterName = "@Validation";
            sqlCommand.Parameters.Add(paramComprobacion);
            
            connection.Connect();
            sqlCommand.ExecuteNonQuery();
            connection.Disconnect();

            return bool.Parse(sqlCommand.Parameters["@Validation"].Value.ToString());
        }

        public bool Alter_InternalAccess()
        {
            bool exito  =   false;
            try
            {
                var archivoConfiguracion    =   ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var configuraciones         =   archivoConfiguracion.AppSettings.Settings;

                if (configuraciones["Servidor"] == null)
                    configuraciones.Add("Servidor", Servidor);
                else
                    configuraciones["Servidor"].Value =  @Servidor;

                archivoConfiguracion.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(archivoConfiguracion.AppSettings.SectionInformation.Name);

                if (configuraciones["Usuario"] == null)
                    configuraciones.Add("Usuario", Usuario);
                else
                    configuraciones["Usuario"].Value = Usuario;

                archivoConfiguracion.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(archivoConfiguracion.AppSettings.SectionInformation.Name);

                if (configuraciones["Contrasenia"] == null)
                    configuraciones.Add("Contrasenia", Contrasenia);
                else
                    configuraciones["Contrasenia"].Value = Contrasenia;

                archivoConfiguracion.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(archivoConfiguracion.AppSettings.SectionInformation.Name);

                exito   =   true;
            }
            catch (ConfigurationErrorsException ex)
            {
            }
            return exito;
        }
    }
}
