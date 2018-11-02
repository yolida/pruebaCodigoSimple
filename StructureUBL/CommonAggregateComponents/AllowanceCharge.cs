using System;
using StructureUBL.CommonBasicComponents;
using StructureUBL.CommonStaticComponents;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class AllowanceCharge
    {
        /// <summary>
        /// Indicador de cargo
        /// </summary>
        public bool ChargeIndicator { get; set; } // Indicador del cargo/descuento global

        /// <summary>
        /// Catálogo No. 53: Códigos de cargos o descuentos
        /// Código del cargo/descuento del ítem | an2
        /// Listado de errores:
        ///  { { El valor del tag es diferente de '00', '01', '47' y '48' | OBSERV 4268 | El dato ingresado como cargo/descuento no es valido a nivel de ítem.},
        ///  { El valor del tag es distinto al Catálogo 53 | ERROR 2954 | El valor ingresado como codigo de motivo de cargo/descuento por linea no es valido }}
        /// </summary>
        public AllowanceChargeReasonCode AllowanceChargeReasonCode { get; set; }
        public decimal MultiplierFactorNumeric { get; set; } // Factor del cargo/descuento del ítem

        /// Monto del cargo/descuento del ítem
        /// => Código de tipo de moneda del monto de cargo/descuento del ítem
        /// public PayableAmount Amount { get; set; }
        public PayableAmount Amount { get; set; }

        /// Monto de base de cargo/descuento global
        /// => Código de tipo de moneda del monto de base del cargo/descuento global
        public PayableAmount BaseAmount { get; set; }

        public AllowanceCharge()
        {
            Amount      = new PayableAmount();
            BaseAmount  = new PayableAmount();
        }
    }
}
