using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Omnicorp;
using NUnit.Framework;
using ClassLibrary;
using MySql.Data.MySqlClient;
using System.Configuration;


namespace Tests
{
    [TestFixture]
    public class TestMyQuery
    {

        [Test]
        public void CheckDatabaseCredentialsFromAppConfig()
        {
            string server = ConfigurationManager.AppSettings["server"];
            string database = ConfigurationManager.AppSettings["database"];
            string uid = ConfigurationManager.AppSettings["uid"];
            string password = ConfigurationManager.AppSettings["password"];

            Assert.NotNull(server);
            Assert.NotNull(database);
            Assert.NotNull(uid);
            Assert.NotNull(password);
        }

        [Test]
        public void TryConnectionToDatabase()
        {
            MyQuery myQuery = new MyQuery();
            string query = "Select version();";

            MySqlDataReader rdr = myQuery.DataReader(query);
            string version = string.Empty;

            if (rdr.Read())
            {
                version = rdr.GetString(0).ToString();
            }
            myQuery.Close();

            Assert.IsNotEmpty(version);
        }
    }
}
