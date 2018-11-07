using System;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer.CRUD
{
    public class Data_DocumentoFigurativo
    {
        public string   XMLFirmado  { get; set; }
        public string   CDR         { get; set; }
        public string   IdDocumento { get; set; }
        public int      IdDocumentoFigurativo   { get; set; }
        public int      SCOPE_IDENTITY_VALUE    { get; set; }

        /// <summary>
        /// Registrar un documento figurativo, el cual puede contener una comunicación de baja o un resumen diario, estos
        /// se relacionan con la tabla Documentos en base a la tabla intermedia Figurativo_Documento
        /// </summary>
        /// <returns></returns>
        public bool Create_DocumentoFigurativo()
        {
            string storedProcedure = "[dbo].[Create_DocumentoFigurativo]";
            Connection connection   =   new Connection();
            SqlCommand sqlCommand   =   new SqlCommand() {
                CommandText =   storedProcedure,
                CommandType =   CommandType.StoredProcedure,
                Connection  =   connection.connectionString
            };

            SqlParameter parameterXMLFirmado    =   new SqlParameter() {
                SqlDbType       =   SqlDbType.NVarChar,
                ParameterName   =   "@XMLFirmado",
                Value           =   XMLFirmado
            };
            sqlCommand.Parameters.Add(parameterXMLFirmado);

            SqlParameter parameterCDR   =   new SqlParameter() {
                SqlDbType       =   SqlDbType.NVarChar,
                ParameterName   =   "@CDR",
                Value           =   CDR
            };
            sqlCommand.Parameters.Add(parameterCDR);

            SqlParameter parameterValidation    =   new SqlParameter() {
                SqlDbType       =   SqlDbType.Bit,
                Direction       =   ParameterDirection.Output,
                ParameterName   =   "@Validation"
            };
            sqlCommand.Parameters.Add(parameterValidation);

            SqlParameter parameterSCOPE_IDENTITY_VALUE  =   new SqlParameter() {
                SqlDbType       =   SqlDbType.Int,
                ParameterName   =   "@SCOPE_IDENTITY_VALUE",
                Direction       =   ParameterDirection.Output
            };
            sqlCommand.Parameters.Add(parameterSCOPE_IDENTITY_VALUE);

            connection.Connect();
            sqlCommand.ExecuteNonQuery();
            connection.Disconnect();

            SCOPE_IDENTITY_VALUE    =   int.Parse(sqlCommand.Parameters["@SCOPE_IDENTITY_VALUE"].Value.ToString());
            return bool.Parse(sqlCommand.Parameters["@Validation"].Value.ToString());
        }

        /// <summary>
        /// Registra un Figurativo_Documento con el cual se puede identificar que Documento pertenece a cierto resumen diario o comunicación(DocumentoFigurativo)
        /// </summary>
        /// <returns></returns>
        public bool Create_Figurativo_Documentos()
        {
            string storedProcedure  =   "[dbo].[Create_Figurativo_Documentos]";
            Connection connection   =   new Connection();
            SqlCommand sqlCommand   =   new SqlCommand() {
                CommandText =   storedProcedure,
                CommandType =   CommandType.StoredProcedure,
                Connection  =   connection.connectionString
            };

            SqlParameter parameterIdDocumentoFigurativo =   new SqlParameter() {
                SqlDbType       =   SqlDbType.Int,
                ParameterName   =   "@IdDocumentoFigurativo",
                Value           =   IdDocumentoFigurativo
            };
            sqlCommand.Parameters.Add(parameterIdDocumentoFigurativo);

            SqlParameter parameterIdDocumento   =   new SqlParameter() {
                SqlDbType       =   SqlDbType.UniqueIdentifier,
                ParameterName   =   "@IdDocumento",
                Value           =   IdDocumento
            };
            sqlCommand.Parameters.Add(parameterIdDocumento);

            SqlParameter parameterValidation    =   new SqlParameter() {
                SqlDbType       =   SqlDbType.Bit,
                Direction       =   ParameterDirection.Output,
                ParameterName   =   "@Validation"
            };
            sqlCommand.Parameters.Add(parameterValidation);

            connection.Connect();
            sqlCommand.ExecuteNonQuery();
            connection.Disconnect();

            return bool.Parse(sqlCommand.Parameters["@Validation"].Value.ToString());
        }
    }
}
