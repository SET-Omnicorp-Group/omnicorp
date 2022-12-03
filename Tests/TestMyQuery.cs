/*
* FILE          :   TestMyQuery.cs
* PROJECT       :   SENG2020 - Omnicorp project
* PROGRAMMERS   :   - Ali Anwar - 8765779
*                   - Bruno Borges Russian - 8717542
*                   - Dhruvkumar Patel - 8777164
*                   - Thalys Baiao Lopes - 8760875
* FIRST VERSION :   Nov, 19, 2022
* DESCRIPTION   :   The file is used to  declare the TestMyQuery Class
*/
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
    /*
    * CLASS NAME	:   TestMyQuery
    * DESCRIPTION	:   The purpose of this class is to test the query 
    *
    */
    public class TestMyQuery
    {

        [Test]
        /*
        * METHOD		:  CheckDatabaseCredentialsFromAppConfig
        * DESCRIPTION	:   try to validate the databse credential from app config file 
        * PARAMETERS    : None
        *                  
        * RETURNS       :
        *                   None
        */
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
        /*
       * METHOD		:  TryConnectionToDatabase
       * DESCRIPTION	:   try to validate the databse credential from app config file 
       * PARAMETERS    : None
       *                  
       * RETURNS       :
       *                   None
       */
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
