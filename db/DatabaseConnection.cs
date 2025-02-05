using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP1_BDAvancee.db
{
    internal class DatabaseConnection
    {
        public static SqlConnection connection;

        public static void Connexion()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "LAPTOP";
                builder.InitialCatalog = "ArticlesDB";
                builder.IntegratedSecurity = true;
                connection = new SqlConnection();
                connection.ConnectionString = builder.ConnectionString;
                connection.Open();
                MessageBox.Show("Connexion réussie");
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur de connexion à la base de données");
            }
        }
    }
}
