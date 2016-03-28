using System.Data.Entity.ModelConfiguration;
using Expenses.Core;

namespace Expenses.Data.Mappings
{
    internal class CategoryMapping : EntityTypeConfiguration<Category>
    {
        public CategoryMapping()
        {
            HasKey(c => c.Id);
            Property(e => e.Name).IsRequired().HasMaxLength(100);
        }
    }
}