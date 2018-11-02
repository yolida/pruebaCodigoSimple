using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer.CRUD
{
    public class Data_Usuario
    {
        public string IdUsuario { get; set; }
        public string Contrasenia { get; set; }
        public Int16 IdRol { get; set; }  // Aún no se le dará uso
        public Int16 IdDatosFox { get; set; }
        private string PassContrasenia  = "8xfCkc6Z6SnTHU2TaUUZqf2wYHAAWWn5un3rTcDspjURbdw6";
        public int IdUser_Empresa { get; set; }
        public int IdAccesosSunat { get; set; }

        public bool Alter_Usuario(string storedProcedure)
        {
            Connection connection   =   new Connection();
            SqlCommand sqlCommand   =   new SqlCommand
            {
                CommandText =   storedProcedure,
                CommandType =   CommandType.StoredProcedure,
                Connection  =   connection.connectionString
            };

            SqlParameter paramIdUsuario         =   new SqlParameter
            {
                SqlDbType       =   SqlDbType.NVarChar,
                ParameterName   =   "@IdUsuario",
                Value           =   IdUsuario
            };
            sqlCommand.Parameters.Add(paramIdUsuario);

            SqlParameter paramPassContrasenia   =   new SqlParameter
            {
                SqlDbType       =   SqlDbType.NVarChar,
                ParameterName   =   "@PassContrasenia",
                Value           =   PassContrasenia
            };
            sqlCommand.Parameters.Add(paramPassContrasenia);

            SqlParameter paramContrasenia       =   new SqlParameter
            {
                SqlDbType       =   SqlDbType.NVarChar,
                ParameterName   =   "@Contrasenia",
                Value           =   Contrasenia
            };
            sqlCommand.Parameters.Add(paramContrasenia);

            SqlParameter paramIdRol         =   new SqlParameter
            {
                SqlDbType       =   SqlDbType.SmallInt,
                ParameterName   =   "@IdRol",
                Value           =   IdRol
            };
            sqlCommand.Parameters.Add(paramIdRol);

            SqlParameter paramComprobacion  =   new SqlParameter
            {
                Direction       =   ParameterDirection.Output,
                SqlDbType       =   SqlDbType.Bit,
                ParameterName   =   "@Validation"
            };
            sqlCommand.Parameters.Add(paramComprobacion);

            connection.Connect();
            sqlCommand.ExecuteNonQuery();
            connection.Disconnect();

            return bool.Parse(sqlCommand.Parameters["@Validation"].Value.ToString());
        }

        public bool Security_Authenticate_Usuario()
        {
            Connection connection   =   new Connection();
            SqlCommand sqlCommand   =   new SqlCommand
            {
                CommandText =   "[dbo].[Security_Authenticate_Usuario]",
                CommandType =   CommandType.StoredProcedure,
                Connection  =   connection.connectionString
            };

            SqlParameter paramIdUsuario =   new SqlParameter
            {
                SqlDbType       =   SqlDbType.NVarChar,
                ParameterName   =   "@IdUsuario",
                Value           =   IdUsuario
            };
            sqlCommand.Parameters.Add(paramIdUsuario);

            SqlParameter paramPassContrasenia   =   new SqlParameter
            {
                SqlDbType       =   SqlDbType.NVarChar,
                ParameterName   =   "@PassContrasenia",
                Value           =   PassContrasenia
            };
            sqlCommand.Parameters.Add(paramPassContrasenia);

            SqlParameter paramContrasenia       =   new SqlParameter
            {
                SqlDbType       =   SqlDbType.NVarChar,
                ParameterName   =   "@Contrasenia",
                Value           =   Contrasenia
            };
            sqlCommand.Parameters.Add(paramContrasenia);

            SqlParameter paramComprobacion      =   new SqlParameter
            {
                Direction       =   ParameterDirection.Output,
                SqlDbType       =   SqlDbType.Bit,
                ParameterName   =   "@Validation"
            };
            sqlCommand.Parameters.Add(paramComprobacion);

            connection.Connect();
            sqlCommand.ExecuteNonQuery();
            connection.Disconnect();

            return bool.Parse(sqlCommand.Parameters["@Validation"].Value.ToString());
        }

        public void Read_Id_User_Empresa(int id)
        {
            Connection connection = new Connection();
            SqlCommand sqlCommand = new SqlCommand
            {
                CommandText =   "[dbo].[Read_Id_User_Empresa]",
                CommandType =   CommandType.StoredProcedure,
                Connection  =   connection.connectionString
            };

            SqlParameter paramIdUser_Empresa    =   new SqlParameter() {
                SqlDbType       =   SqlDbType.Int,
                ParameterName   =   "@IdUser_Empresa",
                Value           =   id
            };
            sqlCommand.Parameters.Add(paramIdUser_Empresa);

            connection.Connect();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                IdDatosFox      =   Convert.ToInt16(reader["IdDatosFox"].ToString());
                IdUsuario       =   reader["IdUsuario"].ToString();
                IdAccesosSunat  =   Convert.ToInt32(reader["IdAccesosSunat"].ToString());
                IdUser_Empresa  =   Convert.ToInt32(reader["IdUser_Empresa"].ToString());
            }
            connection.Disconnect();
        }

        /// <summary>
        /// Se empleará más adelante cuando se haga uso de roles
        /// </summary>
        /// <param name="id"></param>
        public void Read_Usuario(int id)
        {
            Connection connection = new Connection();
            SqlCommand sqlCommand = new SqlCommand
            {
                CommandText =   "[dbo].[Read_Usuario]",
                CommandType =   CommandType.StoredProcedure,
                Connection  =   connection.connectionString
            };

            SqlParameter paramIdUsuario     =   new SqlParameter
            {
                SqlDbType       =   SqlDbType.NVarChar,
                ParameterName   =   "@IdUsuario",
                Value           =   IdUsuario
            };
            sqlCommand.Parameters.Add(paramIdUsuario);

            connection.Connect();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                IdDatosFox      =   Convert.ToInt16(reader["IdDatosFox"].ToString());
                //...
            }
            connection.Disconnect();
        }

        public bool Create_User_Empresa()
        {
            Connection connection   = new Connection();
            SqlCommand sqlCommand   = new SqlCommand
            {
                CommandText =   "[dbo].[Create_User_Empresa]",
                CommandType =   CommandType.StoredProcedure,
                Connection  =   connection.connectionString
            };

            SqlParameter paramIdUsuario     =   new SqlParameter
            {
                SqlDbType       =   SqlDbType.NVarChar,
                ParameterName   =   "@IdUsuario",
                Value           =   IdUsuario
            };
            sqlCommand.Parameters.Add(paramIdUsuario);

            SqlParameter paramIdDatosFox    =   new SqlParameter
            {
                SqlDbType       =   SqlDbType.SmallInt,
                ParameterName   =   "@IdDatosFox",
                Value           =   IdDatosFox
            };
            sqlCommand.Parameters.Add(paramIdDatosFox);

            SqlParameter paramComprobacion  =   new SqlParameter
            {
                Direction       =   ParameterDirection.Output,
                SqlDbType       =   SqlDbType.Bit,
                ParameterName   =   "@Validation"
            };
            sqlCommand.Parameters.Add(paramComprobacion);

            connection.Connect();
            sqlCommand.ExecuteNonQuery();
            connection.Disconnect();

            return bool.Parse(sqlCommand.Parameters["@Validation"].Value.ToString());
        }
        
    }
}
