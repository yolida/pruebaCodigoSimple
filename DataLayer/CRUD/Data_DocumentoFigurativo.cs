using System;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer.CRUD
{
    public class Data_DocumentoFigurativo
    {
        public string   XMLFirmado  { get; set; }
        public string   CdrSunat    { get; set; }

        public string   Tipo        { get; set; }
        public string   ComentarioDocumento { get; set; }

        /// <summary>
        /// Es el ID que se le asigna al xml como identificador único del documento RA-Fecha-#####
        /// </summary>
        public string   Identificador       { get; set; }

        /// <summary>
        /// El ID del documento de la tabla Documentos, de tipo UniqueIdentifier
        /// </summary>
        public string   IdDocumento         { get; set; }

        /// <summary>
        /// Es el ID del documento figurativo, que podría ser un resumen diario o una comunicación de baja
        /// pertenece a la tabla DocumentoFigurativo
        /// </summary>
        public int      IdDocumentoFigurativo   { get; set; }

        /// <summary>
        /// Es el id resultante de la creación de un documento  figurativo, se emplea para crear registro en la tabla
        /// Figurativo_Documentos, debido a que un documento figurativo puede estar relacionado con varios documentos
        /// </summary>
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

            SqlParameter parameterCdrSunat  =   new SqlParameter() {
                SqlDbType       =   SqlDbType.NVarChar,
                ParameterName   =   "@CdrSunat",
                Value           =   CdrSunat
            };
            sqlCommand.Parameters.Add(parameterCdrSunat);

            SqlParameter parameterTipo  =   new SqlParameter() {
                SqlDbType       =   SqlDbType.NVarChar,
                ParameterName   =   "@Tipo",
                Value           =   Tipo
            };
            sqlCommand.Parameters.Add(parameterTipo);

            SqlParameter parameterComentarioDocumento   =   new SqlParameter() {
                SqlDbType       =   SqlDbType.NVarChar,
                ParameterName   =   "@ComentarioDocumento",
                Value           =   ComentarioDocumento
            };
            sqlCommand.Parameters.Add(parameterComentarioDocumento);

            SqlParameter parameterIdentificador =   new SqlParameter() {
                SqlDbType       =   SqlDbType.NVarChar,
                ParameterName   =   "@Identificador",
                Value           =   Identificador
            };
            sqlCommand.Parameters.Add(parameterIdentificador);

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
