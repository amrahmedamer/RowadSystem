using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RowadSystem.API.Entity;

namespace RowadSystem.Persistence.Configrations;

public class ExpenseConfigration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
      
        builder
           .Property(i => i.Amount)
           .HasPrecision(18, 4);

    }
}
