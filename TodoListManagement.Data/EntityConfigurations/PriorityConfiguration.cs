using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoListManagement.Data.Models;

namespace TodoListManagement.Data.EntityConfigurations
{
    public class PriorityConfiguration : IEntityTypeConfiguration<Priority>
    {
        public void Configure(EntityTypeBuilder<Priority> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .HasColumnType("varchar")
                .HasMaxLength(20);

            builder.HasMany(p => p.Items)
                .WithOne(i => i.Priority);
        }
    }
}
