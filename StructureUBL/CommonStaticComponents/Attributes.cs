using System;

namespace StructureUBL.CommonStaticComponents
{
    public class Attributes
    {
        /// <summary>
        /// The identification of the identification scheme.
        /// </summary>
        public string SchemeID          { get; set; }

        /// <summary>
        /// The name of the identification scheme.
        /// </summary>
        public string SchemeName        { get; set; }

        /// <summary>
        /// The identification of the agency that maintains the identification scheme.
        /// </summary>
        public string SchemeAgencyID    { get; set; }

        /// <summary>
        /// The name of the agency that maintains the identification scheme.
        /// </summary>
        public string SchemeAgencyName  { get; set; }

        /// <summary>
        /// The version of the identification scheme.
        /// </summary>
        public string SchemeVersionID   { get; set; }

        /// <summary>
        /// The Uniform Resource Identifier that identifies where the identification scheme data is located.
        /// </summary>
        public string SchemeDataURI     { get; set; }

        /// <summary>
        /// The Uniform Resource Identifier that identifies where the identification scheme is located.
        /// </summary>
        public string SchemeURI         { get; set; }

        /// <summary>
        /// The identification of a list of codes.
        /// </summary>
        public string ListID            { get; set; }

        /// <summary>
        /// An agency that maintains one or more lists of codes.
        /// </summary>
        public string ListAgencyID      { get; set; }

        /// <summary>
        /// The name of the agency that maintains the list of codes.
        /// </summary>
        public string ListAgencyName    { get; set; }

        /// <summary>
        /// The name of a list of codes.
        /// </summary>
        public string ListName          { get; set; }

        /// <summary>
        /// The version of the list of codes.
        /// </summary>
        public string ListVersionID     { get; set; }

        /// <summary>
        /// The textual equivalent of the code content component.
        /// </summary>
        public string Name              { get; set; }

        /// <summary>
        /// The identifier of the language used in the code name.
        /// </summary>
        public string LanguageID        { get; set; }

        /// <summary>
        /// The Uniform Resource Identifier that identifies where the code list is located.
        /// </summary>
        public string ListURI           { get; set; }

        /// <summary>
        /// The Uniform Resource Identifier that identifies where the code list scheme is located.
        /// </summary>
        public string ListSchemeURI     { get; set; }

        /// <summary>
        /// The unit of the quantity
        /// </summary>
        public string UnitCode          { get; set; }

        /// <summary>
        /// The quantity unit code list.
        /// </summary>
        public string UnitCodeListID    { get; set; }

        /// <summary>
        /// The identification of the agency that maintains the quantity unit code list
        /// </summary>
        public string unitCodeListAgencyID { get; set; }

        /// <summary>
        /// The name of the agency which maintains the quantity unit code list
        /// </summary>
        public string unitCodeListAgencyName { get; set; }
    }
}
