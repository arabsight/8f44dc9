using System.Data.Entity.Migrations;
using Expenses.Core;

namespace Expenses.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ExpensesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ExpensesContext context)
        {
            var users = new[]
            {
                new User
                {
                    Fullname = "Admin Doe",
                    Username = "admin",
                    Password = "admin",
                    IsAdmin = true
                },
                new User
                {
                    Fullname = "Rami Clever",
                    Username = "user",
                    Password = "user"
                }
            };

            context.Users.AddOrUpdate(e => e.Username, users);

            var categories = new[]
            {
                new Category {Name = "BUREAUTIQUE"},
                new Category {Name = "CREDIT"},
                new Category {Name = "CUISINE"},
                new Category {Name = "DIVERS"},
                new Category {Name = "FRAIS"},
                new Category {Name = "MANUTENTION"},
                new Category {Name = "PARC"},
                new Category {Name = "PERSONNEL"},
                new Category {Name = "PIECE"},
                new Category {Name = "SABLE"},
                new Category {Name = "SALAIRE"},
                new Category {Name = "TRAVAUX"}
            };

            context.Categories.AddOrUpdate(e => e.Name, categories);

            var receipts = new[]
            {
                new ReceiptType {Name = "Bon de livraison"},
                new ReceiptType {Name = "Décharge"},
                new ReceiptType {Name = "Bon de réception"},
                new ReceiptType {Name = "Facture"}
            };

            context.ReceiptTypes.AddOrUpdate(e => e.Name, receipts);
        }
    }
}