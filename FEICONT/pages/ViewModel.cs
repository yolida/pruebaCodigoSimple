using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEICONT.pages
{
    public class ViewModel
    {
        public ObservableCollection<ComboBoxPares> ParesCondicionPercepcion { get; set; }
        public ObservableCollection<ComboBoxPares> ParesCondicionCompra { get; set; }
        public ObservableCollection<ComboBoxPares> ParesTiempoAlerta { get; set; }
        public ViewModel()
        {
            ParesTiempoAlerta = new ObservableCollection<ComboBoxPares>();
            ParesTiempoAlerta.Add(new ComboBoxPares("", "Seleccione"));
            ParesTiempoAlerta.Add(new ComboBoxPares("7", "7 Dias"));
            ParesTiempoAlerta.Add(new ComboBoxPares("15", "15 Dias"));
            ParesTiempoAlerta.Add(new ComboBoxPares("20", "20 Dias"));
            ParesTiempoAlerta.Add(new ComboBoxPares("30", "30 Dias"));

            ParesCondicionPercepcion = new ObservableCollection<ComboBoxPares>();
            ParesCondicionPercepcion.Add(new ComboBoxPares("1", "Si esta Dentro"));
            ParesCondicionPercepcion.Add(new ComboBoxPares("2", "Si esta Fuera"));

            ParesCondicionCompra = new ObservableCollection<ComboBoxPares>();
            ParesCondicionCompra.Add(new ComboBoxPares("CON", "Contado"));
            ParesCondicionCompra.Add(new ComboBoxPares("CRE", "Credito"));
        }
    }
}
