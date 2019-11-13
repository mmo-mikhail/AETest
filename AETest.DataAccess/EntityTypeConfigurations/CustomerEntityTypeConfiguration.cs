using AETest.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AETest.DataAccess.EntityTypeConfigurations
{
    class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers", "dbo");

            builder.Property(t => t.FirstName).HasColumnName("FirstName").IsRequired();
            builder.Property(t => t.LastName).HasColumnName("LastName").IsRequired();
            builder.Property(t => t.DateOfBirth).HasColumnName("DateOfBirth").IsRequired();
        }
    }
}
