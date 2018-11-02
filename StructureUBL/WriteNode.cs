using System.Collections.Generic;
using System.Xml;

namespace StructureUBL
{
    public class WriteNode
    {
        public string NodeName  { get; set; }
        public string NodeValue { get; set; }
        public List<AttributeValueXml> ValuesAttribute { get; set; }

        /// Jorge Luis | 28/06/2018 | FEI2-752
        /// Método para generar un nodo de xml, con un listado de atributos
        /// Es imperante decir que en este método no cierra el objeto writer, ya que, se debe tomar como un componente
        /// de la generación de xml y no como todo el proceso, en su implementación adicionar los métodos .Flush() y .Close()
        /// XmlWriterSettings, Indent, Create() son métodos que se deben incluir primero antes de instaciar este método.
        public void GenerateNode(XmlWriter writer)
        {
            writer.WriteStartElement(NodeName.ToString());
            {
                if (ValuesAttribute != null)
                    foreach (AttributeValueXml item in ValuesAttribute)
                        writer.WriteAttributeString(item.NameAttribute, item.ValueAttribute);

                writer.WriteValue(NodeValue);
            }
            writer.WriteEndElement();
        }

        /// Jorge Luis | 28/06/2018 | FEI2-752
        /// Clase con los atributos del nodo
        public class AttributeValueXml
        {
            public string NameAttribute { get; set; }
            public string ValueAttribute { get; set; }
        }
    }
}
