using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Jordy Amaro 11-01-17 FEICONT2-4
/// Cambio de interfaz - Clase para definir parejas de valores en los select..
/// </summary>
namespace FEICONT
{
    public class ComboBoxPares
    {
        public string _Id { get; set; }
        public string _Value { get; set; }
        //Metodo constructor
        public ComboBoxPares(string _id, string _value)
        {
            _Id = _id;
            _Value = _value;
        }
    }
}
