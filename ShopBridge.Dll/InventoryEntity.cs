using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopBridge.Dll;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ShopBridge.Dll
{
   public  class InventoryEntity : IEntityTypeConfiguration<Inventory>
    {
        void IEntityTypeConfiguration<Inventory>.Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.ToTable("Inventory");
            builder.Property(t => t.Id).HasColumnName("Id").UseIdentityColumn();
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.Price).HasColumnName("Price");
            builder.Property(t => t.Description).HasColumnName("Description");
        }

    }
}
