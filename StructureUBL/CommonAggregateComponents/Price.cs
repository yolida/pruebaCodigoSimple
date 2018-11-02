using System;
using StructureUBL.CommonBasicComponents;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class Price
    {
        /// <summary>
        /// Valor unitario del ítem
        /// @currencyID => Código de tipo de moneda del valor unitario del ítem
        /// an3
        /// Listado de errores para Value:
        /// No existe el Tag UBL | ERROR	2068 | El XML no contiene el tag cac:Price/cbc:PriceAmount en el detalle de los Items
        /// El formato del Tag UBL es diferente de decimal positivo de 12 enteros y hasta 10 decimales y diferente de cero 
        /// | ERROR	2369 | El dato ingresado en PriceAmount del Valor de venta unitario por item no cumple con el formato establecido
        /// Si existe en la línea un cac:TaxSubTotal con 'Código de tributo por línea' igual a '9996' cuyo 'Monto base' es mayor a cero(cbc:TaxableAmount > 0), el valor del Tag UBL es mayor a 0 (cero) 
        /// | ERROR	2640 | Operacion gratuita, solo debe consignar un monto referencial
        /// Listado de errores para currencyID:
        /// Si existe el atributo, el valor del atributo es diferente al ingresado en 'Tipo de moneda' 
        /// | ERROR 2071 | La moneda debe ser la misma en todo el documento. Salvo las percepciones que sólo son en moneda nacional.
        /// <summary>
        public PayableAmount PriceAmount { get; set; }

        public Price()
        {
            PriceAmount = new PayableAmount();
        }
    }
}
