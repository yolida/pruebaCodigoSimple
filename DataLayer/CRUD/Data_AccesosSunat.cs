using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataLayer.CRUD
{
    public class Data_AccesosSunat
    {
        public Data_AccesosSunat(int idAccesosSunat)
        {
            IdAccesosSunat = idAccesosSunat;
        }

        public int      IdAccesosSunat { get; set; }
        public string   CertificadoDigital { get; set; }
        public string   ClaveCertificado { get; set; }
        public string   UsuarioSol { get; set; }
        public string   ClaveSol { get; set; }
        public string   IdUsuario { get; set; }
        public Int16    IdDatosFox { get; set; }

        private string PassContrasenia = "w6nun3rTcDspjUCkc6Z6Sqf2wYHAR8xfAWWn5bdTHU2TaUUZ";

        public bool Create_AccesosSunat()
        {
            string storedProcedure  = "[dbo].[Create_AccesosSunat]";
            Connection connection   = new Connection();
            SqlCommand sqlCommand   = new SqlCommand
            {
                CommandText = storedProcedure,
                CommandType = CommandType.StoredProcedure,
                Connection  = connection.connectionString
            };

            SqlParameter paramCertificadoDigital = new SqlParameter
            {
                SqlDbType       = SqlDbType.NVarChar,
                ParameterName   = "@CertificadoDigital",
                Value           = CertificadoDigital
            };
            sqlCommand.Parameters.Add(paramCertificadoDigital);

            SqlParameter paramClaveCertificado  = new SqlParameter
            {
                SqlDbType       = SqlDbType.NVarChar,
                ParameterName   = "@ClaveCertificado",
                Value           = ClaveCertificado
            };
            sqlCommand.Parameters.Add(paramClaveCertificado);

            SqlParameter paramUsuarioSol        = new SqlParameter
            {
                SqlDbType       = SqlDbType.NVarChar,
                ParameterName   = "@UsuarioSol",
                Value           = UsuarioSol
            };
            sqlCommand.Parameters.Add(paramUsuarioSol);

            SqlParameter paramClaveSol          = new SqlParameter
            {
                SqlDbType       = SqlDbType.NVarChar,
                ParameterName   = "@ClaveSol",
                Value           = ClaveSol
            };
            sqlCommand.Parameters.Add(paramClaveSol);

            SqlParameter paramPassContrasenia   = new SqlParameter
            {
                SqlDbType       = SqlDbType.NVarChar,
                ParameterName   = "@PassContrasenia",
                Value           = PassContrasenia
            };
            sqlCommand.Parameters.Add(paramPassContrasenia);

            SqlParameter paramIdUsuario         = new SqlParameter
            {
                SqlDbType       = SqlDbType.NVarChar,
                ParameterName   = "@IdUsuario",
                Value           = IdUsuario
            };
            sqlCommand.Parameters.Add(paramIdUsuario);

            SqlParameter paramIdDatosFox        = new SqlParameter
            {
                SqlDbType       = SqlDbType.SmallInt,
                ParameterName   = "@IdDatosFox",
                Value           = IdDatosFox
            };
            sqlCommand.Parameters.Add(paramIdDatosFox);

            SqlParameter paramComprobacion      = new SqlParameter
            {
                Direction       = ParameterDirection.Output,
                SqlDbType       = SqlDbType.Bit,
                ParameterName   = "@Validation"
            };
            sqlCommand.Parameters.Add(paramComprobacion);
            
            connection.Connect();
            sqlCommand.ExecuteNonQuery();
            connection.Disconnect();

            return bool.Parse(sqlCommand.Parameters["@Validation"].Value.ToString());
        }

        public void Read_AccesosSunat()
        {
            Connection connection   =   new Connection();
            SqlCommand sqlCommand   =   new SqlCommand
            {
                CommandText =   "[dbo].[Read_AccesosSunat]",
                CommandType =   CommandType.StoredProcedure,
                Connection  =   connection.connectionString
            };

            SqlParameter paramIdAccesosSunat    =   new SqlParameter
            {
                SqlDbType       =   SqlDbType.Int,
                ParameterName   =   "@IdAccesosSunat",
                Value           =   IdAccesosSunat
            };
            sqlCommand.Parameters.Add(paramIdAccesosSunat);

            SqlParameter paramPassContrasenia   =   new SqlParameter
            {
                SqlDbType       =   SqlDbType.NVarChar,
                ParameterName   =   "@PassContrasenia",
                Value           =   PassContrasenia
            };
            sqlCommand.Parameters.Add(paramPassContrasenia);

            connection.Connect();
            using (SqlDataReader reader =   sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    CertificadoDigital  =   reader["CertificadoDigital"].ToString();
                    ClaveCertificado    =   reader["ClaveCertificado"].ToString();
                    UsuarioSol          =   reader["UsuarioSol"].ToString();
                    ClaveSol            =   reader["ClaveSol"].ToString();
                }
            }

            sqlCommand.ExecuteNonQuery();
            connection.Disconnect();
        }
    }
}
