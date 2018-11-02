using System;
using StructureUBL.CommonBasicComponents;
using StructureUBL.CommonStaticComponents;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class AlternativeConditionPrice
    {
        /// <summary>
        /// Precio de venta unitario/Valor referencial unitario en operaciones no onerosas
        /// @currencyID => Código de tipo de moneda del precio de venta unitario o valor referencial unitario
        /// Lista de errores:
        /// Si no existe en la misma línea un cac:TaxSubTotal con 'Código de tributo por línea' igual a '9996' cuyo
        /// 'Monto base' es mayor a cero (cbc:TaxableAmount > 0), y el precio unitario es diferente al resultado de 
        /// dividir:  la sumatoria del valor de venta por ítem más los impuestos por línea menos los descuentos que 
        /// no afectan la base imponible del ítem ('Código de motivo de descuento' igual a '01') más los cargos que 
        /// no afectan la base imponible del ítem ('Código de motivo de cargo' igual a '48'),  entre la cantidad de 
        /// ítem (con una tolerancia + -1) | OBSERV	4287 |   El precio unitario de la operación que está informando 
        /// difiere de los cálculos realizados en base a la información remitida.
        /// <summary>
        public PayableAmount PriceAmount { get; set; }

        /// <summary>
        /// Código de tipo de precio
        /// Listado de errores:
        /// El valor del Tag UBL es diferente al listado | ERROR 2410 | Se ha consignado un valor invalido en el campo cbc:PriceTypeCode
        /// El valor del Tag UBL no debe repertirse en el /Invoice/cac:InvoiceLine/cac:PricingReference/cac:AlternativeConditionPrice |
        /// ERROR 2409 | Existe mas de un tag cac:AlternativeConditionPrice con el mismo cbc:PriceTypeCode
        /// </summary>
        public PriceTypeCode PriceTypeCode { get; set; }

        public AlternativeConditionPrice()
        {
            PriceAmount     = new PayableAmount();
            PriceTypeCode   = new PriceTypeCode();
        }
    }
}
