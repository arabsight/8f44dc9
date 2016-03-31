using System.Data.Entity.ModelConfiguration;
using Expenses.Core;

namespace Expenses.Data.Mappings
{
    internal class WithdrawalMapping : EntityTypeConfiguration<Withdrawal>
    {
        public WithdrawalMapping()
        {
            HasKey(e => e.Id);

            //Property(p => p.Code)
            //    .IsRequired()
            //    .HasMaxLength(100);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(e => e.Date).IsRequired();

            Property(e => e.Amount).IsRequired();

            HasRequired(e => e.Exercise)
                .WithMany(e => e.Withdrawals)
                .HasForeignKey(e => e.ExerciseId);
        }
    }
}