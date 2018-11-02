using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.CRUD
{
    public interface IGetDataAsync
    {
        Task<List<Data_Documentos>> GetListFiltered(Int16 IdDatosFox, DateTime Start_FechaRegistro, DateTime End_FechaRegistro, int idTipoDocumento);
        Task<GetDataAsync> Read_List_Log(Data_Log data_Log);
    }
}
