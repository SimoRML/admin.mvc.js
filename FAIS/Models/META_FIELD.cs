//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FAIS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class META_FIELD
    {
        public long META_FIELD_ID { get; set; }
        public long META_BO_ID { get; set; }
        public string DB_NAME { get; set; }
        public string DB_TYPE { get; set; }
        public int DB_NULL { get; set; }
        public string GRID_NAME { get; set; }
        public string GRID_FORMAT { get; set; }
        public Nullable<int> GRID_SHOW { get; set; }
        public string FORM_NAME { get; set; }
        public string FORM_FORMAT { get; set; }
        public string FORM_TYPE { get; set; }
        public string FORM_SOURCE { get; set; }
        public Nullable<int> FORM_SHOW { get; set; }
        public Nullable<int> FORM_OPTIONAL { get; set; }
        public Nullable<int> IS_FILTER { get; set; }
        public string FORM_DEFAULT { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public Nullable<System.DateTime> UPDATED_DATE { get; set; }
        public string STATUS { get; set; }
        public Nullable<int> VERSION { get; set; }
        public string JSON_DATA { get; set; }
    }
}
