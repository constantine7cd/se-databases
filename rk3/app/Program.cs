using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            //p.first();
            //p.second();
            //p.third();
            p.task3();
        }

        void third()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["try"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                // Открываем подключение
                connection.Open();
                SqlCommand command = new SqlCommand();

                command.CommandText = String.Format("select DATEDIFF(SECOND, U.time_, U2.time_) as diff, " +
         "E.FIO from Uchet as U join Uchet " +
         "as U2 On U.type_ = 1 and U2.type_ = 2 and U.id_empl = U2.id_empl and " +
         "U.sysdate = U2.sysdate join Empl as E On U.id_empl = E.ID " +
         "where DAY(U.sysdate) = DAY(GETDATE()) and " +
          "MONTH(U.sysdate) = MONTH(GETDATE()) and " +
          "YEAR(U.sysdate) = YEAR(GETDATE()) " +
         "ORDER BY(DATEDIFF(HOUR, U.time_, U2.time_))");
                command.Connection = connection;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                Console.WriteLine("Работники с наименьшим количеством времени работы:");
                int min = 0;
                foreach (DataRow row in dt.Rows)
                {
                    // получаем все ячейки строки
                    var cells = row.ItemArray;

                    if (min == 0)
                    {
                        min = (int)cells[0];
                    }
                    else if ((int)cells[0] != min)
                    {
                        break;
                    }

                    Console.Write("\t{0} \t{1} секунд", cells[1], cells[0]);

                    Console.WriteLine();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Что то сломалось..." + ex.Message);

            }
            finally
            {
                Console.ReadLine();
                connection.Close();
            }
        }

        void second()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["try"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                // Открываем подключение
                connection.Open();
                SqlCommand command = new SqlCommand();

                command.CommandText = String.Format("select DEP.Title from Department as DEP where  2 < ( "+
                    "SELECT COUNT(*) from Uchet as U join "+
                    "Empl as E ON E.ID = U.id_empl "+
                    "where sysdate > CAST('2018-12-10' AS Date) and "+
                    "time_ > CAST('9:00:00' AS time) and E.Depart = DEP.ID and "+
                    "U.type_ = 1)");
                command.Connection = connection;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                Console.WriteLine("Найденные отделы:");
                foreach (DataRow row in dt.Rows)
                {
                    // получаем все ячейки строки
                    var cells = row.ItemArray;
                    foreach (object cell in cells)
                        Console.Write("\t{0}", cell);
                    Console.WriteLine();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Что то сломалось..." + ex.Message);

            }
            finally
            {
                Console.ReadLine();
                connection.Close();
            }
        }
        
        void first()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["try"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                // Открываем подключение
                connection.Open();
                SqlCommand command = new SqlCommand();
                
                command.CommandText = String.Format("select distinct E.FIO, D.Title from Uchet as U" +
                                                                                   " join Empl as E ON E.ID = U.id_empl" +
                                                                                   " join Department as D ON D.ID = E.Depart" +
                                                                                   " where U.time_ > CAST('9:00:00' AS time) and" +
                                " U.type_ = 1");
                command.Connection = connection;
                SqlDataReader reader = command.ExecuteReader();
                Console.WriteLine("Вот что удалось найти:");
                while (reader.Read())
                {
                    Console.WriteLine(reader["FIO"]);
                    Console.WriteLine(reader["Title"]);
                }
                

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Что то сломалось..." + ex.Message);
                
            }
            finally
            {
                Console.ReadLine();
                connection.Close();
            }
        }

        public void task3()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["try"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                // Открываем подключение
                connection.Open();

                SqlCommand command1 = new SqlCommand("select * from on_work()", connection);
                SqlDataReader reader = command1.ExecuteReader();
                Console.WriteLine("Вот что удалось найти:");
                while (reader.Read())
                {
                    Console.WriteLine(reader["FIO"]);
                }


            }
            catch (SqlException ex)
            {
                Console.WriteLine("Что то сломалось..." + ex.Message);

            }
            finally
            {
                Console.ReadLine();
                connection.Close();
            }
        }
    }
}
