using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.CRUD
{
    public class Data_Log: Base, IGetDataAsync
    {
        public string   DetalleError    { get; set; }
        public DateTime Fecha           { get; set; }
        public int      IdUser_Empresa  { get; set; }
        public string   Comentario      { get; set; }
        
        public bool Create_Log()
        {
            Connection connection   =   new Connection();

            SqlCommand sqlCommand   =   new SqlCommand() {
                CommandText = "[dbo].[Create_Log]",
                CommandType =   CommandType.StoredProcedure,
                Connection  =   connection.connectionString
            };

            SqlParameter paramDetalleError      =   new SqlParameter() {
                SqlDbType       =   SqlDbType.NVarChar,
                ParameterName   =   "@DetalleError",
                Value           =   DetalleError,
            };
            sqlCommand.Parameters.Add(paramDetalleError);

            SqlParameter paramComentario        =   new SqlParameter() {
                SqlDbType       =   SqlDbType.NVarChar,
                ParameterName   =   "@Comentario",
                Value           =   Comentario
            };
            sqlCommand.Parameters.Add(paramComentario);

            SqlParameter paramIdUser_Empresa    =   new SqlParameter() {
                SqlDbType       =   SqlDbType.Int,
                ParameterName   =   "@IdUser_Empresa",
                Value           =   IdUser_Empresa
            };
            sqlCommand.Parameters.Add(paramIdUser_Empresa);

            SqlParameter paramComprobacion      =   new SqlParameter() {
                Direction       =   ParameterDirection.Output,
                SqlDbType       =   SqlDbType.Bit,
                ParameterName   =   "@Validation"
            };
            sqlCommand.Parameters.Add(paramComprobacion);
            
            connection.Connect();
            sqlCommand.ExecuteNonQuery();
            connection.Disconnect();

            return Convert.ToBoolean(sqlCommand.Parameters["@Validation"].Value.ToString());
        }

        public Task<List<Data_Documentos>> GetListFiltered(short IdDatosFox, DateTime Start_FechaRegistro, DateTime End_FechaRegistro, int? idTipoDocumento)
        {
            throw new NotImplementedException();
        }

        async Task<GetDataAsync> IGetDataAsync.Read_List_Log(Data_Log data_Log)
        {
            var task = Task.Factory.StartNew(() =>
            {
                DataTable dataTable             =   new DataTable();
                Connection connection           =   new Connection();
                SqlCommand sqlCommand           =   new SqlCommand() {
                    CommandText =   "[dbo].[Read_List_Log]",
                    CommandType =   CommandType.StoredProcedure,
                    Connection  =   connection.connectionString
                };
                SqlDataAdapter sqlDataAdapter   =   new SqlDataAdapter() {
                    SelectCommand   =   sqlCommand
                };
            
                SqlParameter parameterStart_FechaRegistro   =   new SqlParameter
                {
                    SqlDbType       = SqlDbType.DateTime,
                    ParameterName   = "@Start_FechaRegistro",
                    Value           = data_Log.Start_FechaRegistro
                };
                sqlCommand.Parameters.Add(parameterStart_FechaRegistro);

                SqlParameter parameterEnd_FechaRegistro     =   new SqlParameter
                {
                    SqlDbType       = SqlDbType.DateTime,
                    ParameterName   = "@End_FechaRegistro",
                    Value           = data_Log.End_FechaRegistro
                };
                sqlCommand.Parameters.Add(parameterEnd_FechaRegistro);

                SqlParameter paramIdUser_Empresa            =   new SqlParameter() {
                    SqlDbType       =   SqlDbType.Int,
                    ParameterName   =   "@IdUser_Empresa",
                    Value           =   data_Log.IdUser_Empresa
                };
                sqlCommand.Parameters.Add(paramIdUser_Empresa);
            
                connection.Connect();
                sqlDataAdapter.Fill(dataTable);
                connection.Disconnect();

                DataRow row;
                List<Data_Log> data_Logs    =   new List<Data_Log>();
                Data_Log log;

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    row =   dataTable.Rows[i];
                    log =   new Data_Log();
                    log.Fecha       =   Convert.ToDateTime(row["Fecha"].ToString());
                    log.Comentario  =   row["Comentario"].ToString();
                    log.DetalleError=   row["DetalleError"].ToString();
                    log.Selectable  = false;
                    data_Logs.Add(log);
                }
                 var getDataAsync   =   new GetDataAsync() {
                     Data_Logs  =   data_Logs
                 };
                return getDataAsync;

            });
            return await task;
        }
    }
}
