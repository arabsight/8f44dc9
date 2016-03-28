using System.Data.Entity.ModelConfiguration;
using Expenses.Core;

namespace Expenses.Data.Mappings
{
    internal class ExerciseMapping : EntityTypeConfiguration<Exercise>
    {
        public ExerciseMapping()
        {
            HasKey(c => c.Id);
            Property(c => c.Date).IsRequired();
            Property(c => c.IsCurrent).IsRequired();
            Property(c => c.IsClosed).IsRequired();
            Property(c => c.Balance).IsRequired();

            Ignore(c => c.StartDate);
            Ignore(c => c.EndDate);
        }
    }
}