using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Expenses.Core;

namespace Expenses.Data.Mappings
{
    internal class UserMapping : EntityTypeConfiguration<User>
    {
        public UserMapping()
        {
            HasKey(c => c.Id);

            Property(c => c.Fullname)
                .IsRequired()
                .HasMaxLength(100);

            Property(c => c.Username)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("UsernameIndex")
                    {
                        IsUnique = true
                    })
                );

            Property(c => c.Password)
                .IsRequired()
                .HasMaxLength(100);

            Property(c => c.IsAdmin).IsRequired();
        }
    }
}