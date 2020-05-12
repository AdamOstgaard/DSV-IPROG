using System;
using System.Windows.Forms;

namespace IPROG.Uppgifter.uppg3_1_2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var connectionStringBuilder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder
            {
                UserID = "usr_19413097",
                Password = "413097",
                Server = "atlas.dsv.su.se",
                Database = "db_19413097",
                Port = 3306
            };

            var guestBook = new GuestBookDbConnector(connectionStringBuilder.ToString());

            Application.Run(new Form1(guestBook));
        }
    }
}
