using System.Data.Common;
using System.Data.SqlServerCe;

namespace Expenses.Data
{
    public class DbHelper
    {
        public static DbConnection SqlCeConnection
        {
            get
            {
                var builder = new SqlCeConnectionStringBuilder
                {
                    DataSource = @"C:\Users\Arabsight\documents\visual studio 2015\Projects\Expenses\Expenses.Data\Database.sdf"
                };

                return new SqlCeConnection(builder.ToString());
            }
        }
    }
}
