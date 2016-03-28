using System.Data.Entity.ModelConfiguration;
using Expenses.Core;

namespace Expenses.Data.Mappings
{
    class ExpenseMapping : EntityTypeConfiguration<Expense>
    {
        public ExpenseMapping()
        {
            HasKey(c => c.Id);

            Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(e => e.Date)
                .IsRequired();

            Property(e => e.Amount)
                .IsRequired();

            Property(e => e.IsValidated)
                .IsRequired();

            Property(e => e.ReceiptNumber)
                .IsRequired()
                .HasMaxLength(20);

            Property(e => e.Description)
                .HasMaxLength(255);

            HasRequired(e => e.Exercise)
                .WithMany(e => e.Expenses)
                .HasForeignKey(e => e.ExerciseId);

            HasRequired(e => e.Category)
                .WithMany(e => e.Expenses)
                .HasForeignKey(e => e.CategoryId);

            HasRequired(e => e.ReceiptType)
                .WithMany(e => e.Expenses)
                .HasForeignKey(e => e.ReceiptTypeId);
        }
    }
}