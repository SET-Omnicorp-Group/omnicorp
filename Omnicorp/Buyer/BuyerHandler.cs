/*
* FILE          :   BuyerHandler.cs
* PROJECT       :   SENG2020 - Omnicorp  project
* PROGRAMMERS   :   - Ali Anwar - 8765779
*                   - Bruno Borges Russian - 8717542
*                   - Dhruvkumar Patel - 8777164
*                   - Thalys Baiao Lopes - 8760875
* FIRST VERSION :   Nov, 19, 2022
* DESCRIPTION   :   The file is used to declare the  BuyerHandler Class
*/
using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Windows.Controls;
using System.Data;
using System.Windows;
using Org.BouncyCastle.Asn1.X500;
using System.Windows.Forms;
using System.IO;

namespace Omnicorp.Buyer
{
    /*
    * CLASS NAME	:   BuyerHandler
    * DESCRIPTION	:   The purpose of this class is to get the order from the marketplace and handler the order
    *
    */
    internal class BuyerHandler
    {

        /*
        * METHOD		:  GetContractsFromMarketplaceDatabase
        * DESCRIPTION	: try to get contract data from the database
        * PARAMETERS    : None
        *
        * RETURNS       :
        *                   - data in form of data table
        */
        public DataTable GetContractsFromMarketplaceDatabase()
        {
            // Query data from contracts
            string query = $"SELECT Client_Name, Origin, Destination, Quantity," +
                           $"CASE " +
                           $"WHEN Job_Type = 0 THEN \"FTL\"" +
                           $"WHEN Job_Type = 1 THEN \"LTL\"" +
                           $"END AS 'Job Type'," +
                           $"CASE " +
                           $"WHEN Van_Type = 0 THEN \"Dry\"" +
                           $"WHEN Van_Type = 1 THEN \"Reefer\"" +
                           $"END AS 'Van Type'" +
                           $"FROM Contract;";

            MarketplaceQuery mpQuery = new MarketplaceQuery();
            MySqlCommand cmd = new MySqlCommand(query, mpQuery.conn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            mpQuery.Close();

            return dt;
        }


        /*
        * METHOD		:  GetOrdersFromDatabase
        * DESCRIPTION	: try to get active order contract data from the database
        * PARAMETERS    : None
        *
        * RETURNS       :
        *                   - active order data in form of data table
        */
        public DataTable GetOrdersFromDatabase()
        {
            // Query data from contracts
            string query = $"SELECT * FROM orders ";
            if(status != null)
            {
                query += $"WHERE status = '{status}'" ;
            }
            query += ";";


            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(query, myQuery.conn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            myQuery.Close();

            return dt;
        }

        public void InsertContractsToOrderDatabase(
            string customer,
            string jobType,
            int quantity,
            string vanType,
            string origin,
            string destination,
            string status
        )
        {
            // Query data from contracts
            string addQuery = $"INSERT INTO orders " +
                              $"(customer, jobType, quantity, vanType, origin, destination, status) " +
                              $"VALUES  (\"{customer}\", \"{jobType}\", {quantity}, \"{vanType}\", \"{origin}\", \"{destination}\", \"{status}\");";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(addQuery, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();
        }

        public void GenerateInvoice(string orderId)
        {
            decimal amount = CalculateInvoiceAmount(orderId);

            string addQuery = $"INSERT INTO invoices (orderId, amount) VALUES ('{orderId}', '{amount}');";
            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(addQuery, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();

            SetOrderStatus(orderId, "Completed");
        }


        public void SetOrderStatus(string orderId, string status)
        {
            string updateQuery = $"UPDATE orders SET status = '{status}' WHERE id = '{orderId}';";
            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(updateQuery, myQuery.conn);
            cmd.ExecuteNonQuery();

            myQuery.Close();
        }


        public decimal CalculateInvoiceAmount(string orderId)
        {
            string query =  $"SELECT " +
                            $"o.jobType, o.quantity, o.vanType, " +
                            $"r.distance, r.totalHours, " +
                            $"c.ftlRate, c.ltlRate, c.reefCharge " +
                            $"FROM routes r " +
                            $"INNER JOIN orders o ON r.orderId = o.id " +
                            $"INNER JOIN carriers c ON r.carrierId = c.id " +
                            $"WHERE o.id = '{orderId}';";

            MyQuery myQuery = new MyQuery();
            MySqlDataReader rdr = myQuery.DataReader(query);
            rdr.Read();

            string jobType = rdr.GetString(0);
            int quantity = rdr.GetInt32(1);
            string vanType = rdr.GetString(2);
            int distance = rdr.GetInt32(3);
            decimal totalHours = rdr.GetDecimal(4);
            decimal ftlRate = rdr.GetDecimal(5);
            decimal ltlRate = rdr.GetDecimal(6);
            decimal reefCharge = rdr.GetDecimal(7);
            decimal carrierCost = decimal.Zero;
            decimal addReefCost = decimal.Zero;
            decimal amount = decimal.Zero;
            decimal tmsFee = decimal.Zero;

            decimal addDaysCharge = CalculateAddDaysCharge(totalHours);
            carrierCost = addDaysCharge;


            if (jobType == "FTL")
            {
                carrierCost += (ftlRate * distance);
            }
            else // LTL
            {
                carrierCost += (ltlRate * distance * quantity);
            }

            if (vanType == "Reefer")
            {
                addReefCost = carrierCost * reefCharge;
            }

            carrierCost += addReefCost;
            tmsFee = CalculteTMSFee(carrierCost, jobType);
            amount = carrierCost + tmsFee;

            myQuery.Close();

            return amount;
        }


        public decimal CalculteTMSFee(decimal carrierCost, string jobType)
        {
            decimal feePercent = 0.05m; // Assume its a LTL

            if(jobType == "FTL")
            {
                feePercent = 0.08m;
            }

            return carrierCost * feePercent;
        }


        public decimal CalculateAddDaysCharge(decimal totalHours)
        {
            decimal fractionDays = Math.Floor(totalHours / 12);
            int numDays = Convert.ToInt32(fractionDays);
            return numDays * 150;
        }


        public void SaveInvoiceFile(string orderId)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt) | *.txt";
            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string query = $"SELECT o.customer, o.origin, o.destination, o.jobType, o.vanType, o.quantity, i.amount, i.id, r.distance, r.totalHours " +
                            $"FROM orders o " +
                            $"INNER JOIN invoices i ON i.orderId = o.id " +
                            $"INNER JOIN routes r ON r.orderId = o.id " +
                            $"WHERE o.id = '{orderId}';";

                MyQuery myQuery = new MyQuery();
                MySqlDataReader rdr = myQuery.DataReader(query);
                rdr.Read();

                string customer = rdr.GetString(0);
                string origin = rdr.GetString(1);
                string destination = rdr.GetString(2);
                string jobType = rdr.GetString(3);
                string vanType = rdr.GetString(4);
                string quantity = rdr.GetString(5);
                decimal amount = rdr.GetDecimal(6);
                string invoiceId = rdr.GetString(7);
                string distance = rdr.GetString(8);
                decimal totalHours = rdr.GetDecimal(9);


                string content =    $"Invoice Id: {invoiceId}\n" +
                                    $"Order Id: {orderId}\n" +
                                    $"Customer: {customer}\n" +
                                    $"Route: {origin} -> {destination}\n" +
                                    $"Job Description: {jobType} - {vanType}\n";

                if (jobType == "LTL")
                {
                    content += $"Quantity: {quantity} pallets\n";
                }

                content += $"Total Distance: {distance} kms\n" +
                            $"Total Hours: {String.Format("{0:0.00}",totalHours)} hrs\n" +
                            $"Total Amount: {String.Format("{0:0.00}", amount)} CAD\n";


                File.WriteAllText(saveFileDialog.FileName, content);

                myQuery.Close();
            }






        }
    }
}
