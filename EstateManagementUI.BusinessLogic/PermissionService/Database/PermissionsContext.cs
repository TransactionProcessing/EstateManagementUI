using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace EstateManagementUI.BusinessLogic.PermissionService.Database
{
    [ExcludeFromCodeCoverage]
    public class PermissionsContext : DbContext
    {
        public DbSet<ApplicationSection>ApplicationSections { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public string ConnectionString { get; }

        //public PermissionsContext(String connectionString)
        //{
        //    this.ConnectionString = connectionString;
        //}

        public PermissionsContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            //var folder = Environment.SpecialFolder.LocalApplicationData;
            //var path = Environment.GetFolderPath(folder);
            //DbPath = System.IO.Path.Join(path, "permissions.db");
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlite();

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Role>().HasIndex(r => new { r.Name }).IsUnique(true);
            modelBuilder.Entity<ApplicationSection>().HasIndex(r => new { r.Name }).IsUnique(true);
            modelBuilder.Entity<Function>().HasIndex(r => new { r.ApplicationSectionId, r.Name }).IsUnique(true);
            modelBuilder.Entity<UserRole>().HasIndex(ur => new { ur.RoleId, ur.UserName }).IsUnique(true);
        }
    }
}
