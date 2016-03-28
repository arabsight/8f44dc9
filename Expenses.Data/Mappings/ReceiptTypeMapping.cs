using System.Data.Entity.ModelConfiguration;
using Expenses.Core;

namespace Expenses.Data.Mappings
{
    internal class ReceiptTypeMapping : EntityTypeConfiguration<ReceiptType>
    {
        public ReceiptTypeMapping()
        {
            HasKey(c => c.Id);
            Property(e => e.Name).IsRequired().HasMaxLength(100);
        }
    }
}