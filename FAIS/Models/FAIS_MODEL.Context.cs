﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class FAISEntities : DbContext
    {
        public FAISEntities()
            : base("name=FAISEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BO> BO { get; set; }
        public virtual DbSet<BO_CHILDS> BO_CHILDS { get; set; }
        public virtual DbSet<META_BO> META_BO { get; set; }
        public virtual DbSet<META_FIELD> META_FIELD { get; set; }
        public virtual DbSet<VERSIONS> VERSIONS { get; set; }
    
        public virtual int MoveBoToCurrentVersion(Nullable<long> bO_ID)
        {
            var bO_IDParameter = bO_ID.HasValue ?
                new ObjectParameter("BO_ID", bO_ID) :
                new ObjectParameter("BO_ID", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MoveBoToCurrentVersion", bO_IDParameter);
        }
    }
}
