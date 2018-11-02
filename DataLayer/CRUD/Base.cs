using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.CRUD
{
    public abstract class Base
    {
        public DateTime Start_FechaRegistro { get; set; }
        public DateTime End_FechaRegistro   { get; set; }
        public int      IdUser_Empresa      { get; set; }
        public Boolean  Selectable          { get; set; }
    }
}
