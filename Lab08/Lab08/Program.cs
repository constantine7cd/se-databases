using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace Lab08
{
    class Program
    {
        static void Main(string[] args)
        {
            Solution solution = new Solution();

            //solution.GetCarByName();
            //solution.GetMaxByCause();
            //solution.MakeConnection(solution.UpdateFinesFactor);
            //solution.MakeConnection(solution.InsertCar);
            //solution.MakeConnection(solution.deleteHalfFromYourList);
            //solution.MakeConnection(solution.GetDriversWithExperienceHigher);
            //solution.MakeConnection(solution.insertDriver);
            //solution.MakeConnection(solution.HatchbackToHatch);
            //solution.MakeConnection(solution.deleteYearLower);
            //solution.MakeConnection(solution.unpaidFinesToXml);

            Console.Read();

        }
    }

    class Solution
    {
        public void MakeConnection(Func<SqlConnection, bool> function)
        {
            string connString = "Data Source=KONSTANTIN39EC;Initial Catalog=GIBDD;Integrated Security=True";

            SqlConnection conn = new SqlConnection(connString);

            try
            {
                conn.Open();
                function(conn);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error occured " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }


        //First
        //Simple query
        public void GetCarByName()
        {
            string connString = "Data Source=KONSTANTIN39EC;Initial Catalog=GIBDD;Integrated Security=True";

            SqlConnection conn = new SqlConnection(connString);

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();

                Console.WriteLine("Input Car Name");
                string name = Console.ReadLine();

                cmd.CommandText = String.Format("select * from Cars " +
                    "where CarName = '{0}'", name);
                cmd.Connection = conn;

                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("Results: ");

                while (reader.Read())
                {
                    Console.WriteLine(reader["CarId"] + "  " + reader["CarVin"] + " " + reader["CarBody"]);
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error occured " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }

        //Second
        //Scalar function
        public void GetMaxByCause()
        {
            string connString = "Data Source=KONSTANTIN39EC;Initial Catalog=GIBDD;Integrated Security=True";

            SqlConnection conn = new SqlConnection(connString);

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();

                Console.WriteLine("Input Fine cause");
                string cause = Console.ReadLine();

                cmd.CommandText = String.Format("select MAX(Cost) from Fines " +
                    "where Cause = '{0}'", cause);
                cmd.Connection = conn;

                Console.WriteLine("Results: ");

                object count = cmd.ExecuteScalar();
                Console.WriteLine(count);

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error occured " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        //third
        //Stored procedure
        public bool UpdateFinesFactor(SqlConnection connection)
        {
            Console.WriteLine("Input Driver Id");
            string did = Console.ReadLine();


            SqlParameter Did = new SqlParameter("@Did", did);
 
            SqlCommand command = new SqlCommand("exec getDriversWithExperience @Did", connection);
            command.Parameters.Add(Did);

            var res = command.ExecuteScalar();

            Console.WriteLine("Avg Experience for drivers with id higher than {0} is {1}", did, res);
            Console.Write(res);

            return true;
        }

        //fourth
        //Insert
        public bool InsertCar(SqlConnection connection)
        {
            int icost = -1;
            string cost = "";
            while (icost < 0)
            {
                Console.WriteLine("Write Cost. For interrupt write any char");
                cost = Console.ReadLine();

                if (!Int32.TryParse(cost, out icost))
                {
                    Console.WriteLine("Exiting");
                    return false;
                }
                else if (icost < 0)
                {
                    Console.WriteLine("Cost have to be the positive number");
                }
            }

            Console.WriteLine("Write Cause: ");
            string cause = Console.ReadLine();

            Console.WriteLine("Write Is Paid? Press y if yes, otherwise other char");
            string isPaid = Console.ReadLine();

            int paid = 0;

            if (isPaid == "y")
                paid = 1;

            string did = "";
            int idid = -1;

            while (idid < 0)
            {
                Console.WriteLine("Write Driver Id: ");
                did = Console.ReadLine();

                if (!Int32.TryParse(did, out idid))
                {
                    Console.WriteLine("Exiting");
                    return false;
                }
                else if (icost < 0)
                {
                    Console.WriteLine("Driver id have to be the positive number");
                }
            }

           
            SqlCommand cmd = new SqlCommand("insert Fines(Cost, Cause, IsPaid, DriverId) " +
                "values(@cost, @cause, @ispaid, @did)", connection);


            SqlParameter cost_p = new SqlParameter("@cost", cost);
            SqlParameter cause_p = new SqlParameter("@cause", cause);
            SqlParameter ispaid_p = new SqlParameter("@ispaid", paid);
            SqlParameter did_p = new SqlParameter("@did", did);

            cmd.Parameters.Add(cost_p);
            cmd.Parameters.Add(cause_p);
            cmd.Parameters.Add(ispaid_p);
            cmd.Parameters.Add(did_p);

            cmd.ExecuteNonQuery();

            return true;
        }

        //fifth 
        //with using transactions
        public bool deleteHalfFromYourList(SqlConnection connection)
        {
            Console.WriteLine("Write driver indexes to delete");
            string idx_raw = Console.ReadLine();

            string[] idxs = idx_raw.Split();

            int idx;

            List<Int32> idxList = new List<int>();

            foreach (string s in idxs)
            {
                if (Int32.TryParse(s, out idx))
                {
                    if (idx > 0 && idx < 103)
                    {
                        idxList.Add(idx);
                    }
                }
            }

            SqlCommand cmd = connection.CreateCommand();
            SqlTransaction transaction = connection.BeginTransaction();
            cmd.Transaction = transaction;

            int i_ = 0;

            foreach (int i in idxList)
            {
                cmd.CommandText = "Delete Fines where FineId = " + i.ToString();             
                cmd.ExecuteNonQuery();
                transaction.Save(i_.ToString());

                Console.WriteLine(i_);

                i_++;

                
            }

            Random rand = new Random();

            List<int> saved = new List<int>();

            foreach (int i in idxList)
            {
                if (rand.Next(2) == 1)
                {
                    transaction.Rollback(idxList.IndexOf(i).ToString());
                    saved.Add(i);
                }
            }

            Console.WriteLine("The rows with the following Fine ID's wasn't deleted");
            foreach (int i in saved)
            {
                Console.Write("\t {0}", i);
            }

            transaction.Commit();

            return true;
        }


        //sixth
        //
        public bool GetDriversWithExperienceHigher(SqlConnection connection)
        {
            int exp = -1;

            while (exp < 0)
            {
                Console.WriteLine("Write Experience. If you want to exit type any char");
                string exp_s = Console.ReadLine();

                if (!Int32.TryParse(exp_s, out exp))
                {
                    Console.WriteLine("Exiting");
                    return false;
                }
                else if (exp < 0)
                {
                    Console.WriteLine("Experience have to be the positive number");
                }
            }

            SqlCommand cmd = new SqlCommand("select DriverId, Experience from " +
                "Drivers where Experience  > @Experience", connection);

            SqlParameter exp_p = new SqlParameter("@Experience", exp);
            cmd.Parameters.Add(exp_p);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            DataTable dt = ds.Tables[0];

            foreach (DataRow row in dt.Rows)
            {
                var cells = row.ItemArray;

                foreach (object cell in cells)
                {
                    Console.Write("\t{0}", cell);
                }
                Console.WriteLine();
            }


            return true;

        }

        //seventh
        //
        public bool insertDriver(SqlConnection connection)
        {
            int exp = -1;

            while (exp < 0)
            {
                Console.WriteLine("Write Experience. If you want to exit type any char");
                string exp_s = Console.ReadLine();

                if (!Int32.TryParse(exp_s, out exp))
                {
                    Console.WriteLine("Exiting");
                    return false;
                }
                else if (exp < 0)
                {
                    Console.WriteLine("Experience have to be the positive number");
                }
            }

            SqlCommand cmd = new SqlCommand("select * from Drivers", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            DataTable dt = ds.Tables[0];

            DataColumn dc = dt.Columns[0];
            dc.AutoIncrement = true;
            dc.AllowDBNull = false;

            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.UpdateCommand = builder.GetUpdateCommand();

            DataRow nRow = dt.NewRow();
            nRow["Experience"] = exp;

            dt.Rows.Add(nRow);

            PrintDataTable(dt);
            adapter.Update(dt);


            return true;
        }

        //eight
        //update
        public bool HatchbackToHatch(SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("select * from Cars where CarBody = 'hatchback'", connection);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.UpdateCommand = builder.GetUpdateCommand();

            DataSet ds = new DataSet();
            adapter.Fill(ds);

            DataTable dt = ds.Tables[0];

            foreach (DataRow row in dt.Rows)
            {
                row["CarBody"] = "hatch";
            }

            adapter.Update(dt);
       
            return true;
        }


        //nineth
        //delete
        public int Year(string date)
        {
            string[] fulldate_s = date.Split(' ');
            string[] date_s = fulldate_s[0].Split('/');

            return Int32.Parse(date_s[2]);
        }

        public void PrintDataTable(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                    continue;

                foreach (object cell in row.ItemArray)
                {

                    Console.Write("\t{0} ", cell);
                }
                Console.WriteLine();
            }
        }

        public bool deleteYearLower(SqlConnection connection)
        {
            int year = -1;

            while (year < 1900 || year > 2019)
            {
                Console.WriteLine("Write year. If you want to exit type any char");
                string year_s = Console.ReadLine();

                if (!Int32.TryParse(year_s, out year))
                {
                    Console.WriteLine("Exiting");
                    return false;
                }
                else if (year < 1900 || year > 2019)
                {
                    Console.WriteLine("Year have to be the number in range 1900..2019");
                }
            }

            SqlCommand cmd = new SqlCommand("select * from Cars",
                connection);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            DataTable dt = ds.Tables[0];

            Console.WriteLine("Before: ");
            PrintDataTable(dt);

            //SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            //adapter.UpdateCommand = builder.GetUpdateCommand();

            foreach (DataRow row in dt.Rows)
            {
                if (Year(row["CarDate"].ToString()) < year)
                {
                    row.Delete();
                }
            }

            //adapter.Update(dt);

            Console.WriteLine("After: ");
            PrintDataTable(dt);

            return true;
        }

        //tenth
        //
        public bool unpaidFinesToXml(SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("select * from Fines where IsPaid = 0",
                connection);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();

            adapter.Fill(ds);
            ds.WriteXml(@"C:\Users\Konstantin\Documents\db\Lab08\Lab08\unpaid.xml");

            return true;
        }

    }
}
