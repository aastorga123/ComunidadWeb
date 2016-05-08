using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace MiComunidad.Models
{
    public class MiComunidadContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public MiComunidadContext() : base("name=MiComunidadContext")
        {
        }

        public void Detach(object entity)
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            objectContext.Detach(entity);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
        public System.Data.Entity.DbSet<MiComunidad.Models.Cliente> Clientes { get; set; }
        public object Cliente { get; internal set; }

        public System.Data.Entity.DbSet<MiComunidad.Models.Programa> Programas { get; set; }

        public System.Data.Entity.DbSet<MiComunidad.Models.Usuario> Usuarios { get; set; }

        public System.Data.Entity.DbSet<MiComunidad.Models.RolUsuario> RolUsuarios { get; set; }

        public System.Data.Entity.DbSet<MiComunidad.Models.Login> logins { get; set; }

        public System.Data.Entity.DbSet<MiComunidad.Models.UsuarioLogin> UsuarioLogins { get; set; }
    }
}
