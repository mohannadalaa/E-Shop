using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using TaskBackEnd.DataObjects;

namespace TaskBackEnd.Models
{
    public class MobileServiceContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to alter your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
        //
        // To enable Entity Framework migrations in the cloud, please ensure that the 
        // service name, set by the 'MS_MobileServiceName' AppSettings in the local 
        // Web.config, is the same as the service name when hosted in Azure.

        private const string connectionStringName = "Name=MS_TableConnectionString";

        public MobileServiceContext() : base(connectionStringName)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));

            modelBuilder.Entity<TodoItem>().HasKey(k => k.Id, config => config.IsClustered(false));
            modelBuilder.Entity<Item>().HasKey(k => k.Id, config => config.IsClustered(false));
            modelBuilder.Entity<User>().HasKey(k => k.Id, config => config.IsClustered(false));
            modelBuilder.Entity<Order>().HasKey(k => k.Id, config => config.IsClustered(false));
            modelBuilder.Entity<OrderItems>().HasKey(k => k.Id, config => config.IsClustered(false));
        }
    }
}
