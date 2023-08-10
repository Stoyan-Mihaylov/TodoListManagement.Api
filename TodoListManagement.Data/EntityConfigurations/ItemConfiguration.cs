using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoListManagement.Data.Models;

namespace TodoListManagement.Data.EntityConfigurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Title)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            builder.Property(i => i.Description)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(i => i.Status)
                .HasColumnType("smallint");

            builder
                .HasOne(i => i.Priority)
                .WithMany(p => p.Items);
        }
    }
}
