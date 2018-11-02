using StructureUBL.CommonStaticComponents;
using System;
using System.Collections.Generic;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class Item
    {
        /// <summary>
        /// <para>Descripción detallada del servicio prestado, bien vendido o cedido en uso, indicando las características.</para>
        /// an..250
        /// </summary>
        public List<Description>            Descriptions { get; set; }

        public SellersItemIdentification    SellersItemIdentification { get; set; }
        public AdditionalItemIdentification AdditionalItemIdentification { get; set; }
        public CommodityClassification      CommodityClassification { get; set; }
        public  List<AdditionalItemProperty> AdditionalItemProperties { get; set; }
        public StandardItemIdentification   StandardItemIdentification { get; set; } // [0..1] | No se agregará puesto que Tania no lo incluyo en la documentación

        public Item()
        {
            SellersItemIdentification       = new SellersItemIdentification();
            AdditionalItemIdentification    = new AdditionalItemIdentification();
            CommodityClassification         = new CommodityClassification();
            AdditionalItemProperties        = new List<AdditionalItemProperty>();
            Descriptions                    = new List<Description>();
        }
    }

    [Serializable]
    public class SellersItemIdentification
    {
        /// <summary>
        /// Código de producto del ítem an..30
        /// </summary>
        public string Id { get; set; }
    }

    [Serializable]
    public class CommodityClassification
    {
        public ItemClassificationCode ItemClassificationCode { get; set; }

        public CommodityClassification()
        {
            ItemClassificationCode = new ItemClassificationCode();
        }
    }

    [Serializable]
    public class AdditionalItemProperty
    {
        /// <summary>
        /// <para>Nombre del concepto tributario | [1..1]</para>
        /// Catálogo No. 55 | Código de identificación del concepto tributario | an..100
        /// </summary>
        public string Name { get; set; }

        public NameCode NameCode { get; set; }

        /// <summary>
        /// <para>Número de asiento</para>
        /// /Invoice/cac:InvoiceLine/cac:Item/cac:AdditionalItemProperty/cbc:Value (Número de asiento)
        /// [0..1] | Valor de la propiedad del ítem | an..20
        /// <para>Detracciones - Recursos Hidrobiológicos: /Invoice/cac:InvoiceLine/cac:Item/cac:AdditionalItemProperty/cbc:Value</para>
        /// Matrícula de la Embarcación Pesquera an..15, Nombre de la Embarcación Pesquera an..50, Descripción del Tipo de la Especie vendida an..100, Lugar de descarga an..200
        /// <para> Información Adicional  - Beneficio de hospedaje: Número de documento del huesped | Código de tipo de documento de identidad del huesped | 
        ///  Código país de emisión del pasaporte | Apellidos y Nombres o denominación o razón social del huesped | Código del país de residencia del sujeto no domiciliado </para>
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Por el momento se omitira la propiedad 0..* y se asumirá como 0..1
        /// Código del concepto del ítem 
        /// an..4
        /// </summary>
        public ValueQualifier ValueQualifier { get; set; }
        public UsabilityPeriod UsabilityPeriod { get; set; }

        public ValueQuantity ValueQuantity { get; set; }
        public AdditionalItemProperty()
        {
            UsabilityPeriod = new UsabilityPeriod();
            ValueQualifier  = new ValueQualifier();
            ValueQuantity   = new ValueQuantity();
        }
    }

    [Serializable]
    public class AdditionalItemIdentification
    {
        public string Id { get; set; }
    }

    [Serializable]
    public class StandardItemIdentification // No se agregará puesto que Tania no lo incluyo en la documentación
    {   // Código de producto GS1
        public string Id { get; set; }
        public string SchemeID { get; set; } // Tipo de estructura GTIN
    }
}
