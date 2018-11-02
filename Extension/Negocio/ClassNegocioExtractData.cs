using Extension.Base;
using Extension.Datos;
using System.Data;
using System.Data.Odbc;

namespace Extension.Negocio
{
    public class ClassNegocioExtractData : clsBaseEntidad
    {
        //private clsEntityDatabaseLocal localDB;
        //Jorge Luis|23/10/2017|RW-19
        /*Método para extraer un dato de tipo string, mediante una consulta con un parámetro*/
        public string ExtractUniqueValue(string query)
        {
            OdbcConnection odbcConnection = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
            string valor    = string.Empty;
            //string algoQueNoSe1 = localDB.cs_prConexioncadenabasedatos().ToString();
            //string algoQueNoSe2 = localDB.cs_prConexioncadenabasedatos().ToString();
            //odbcConnection.ConnectionString = localDB.cs_prConexioncadenabasedatos();
            odbcConnection.Open();
            OdbcCommand cmd = new OdbcCommand();
            cmd.CommandText = query;
            cmd.Connection  = new OdbcConnection();
            cmd.CommandType = CommandType.Text;
            valor = (string)cmd.ExecuteScalar();
            odbcConnection.Close();
            return valor;
        }

    }
}
