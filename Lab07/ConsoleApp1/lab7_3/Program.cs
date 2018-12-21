using System;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;
namespace lab7_3
{


    [Table(Name = "Cars")]
    public class Cars
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int CarId { get; set; }
        [Column(Name = "CarName")]
        public string CarName { get; set; }
        [Column(Name = "CarVin")]
        public string CarVin { get; set; }
        [Column(Name = "CarDate")]
        public int CarDate { get; set; }
        [Column(Name = "CarBody")]
        public string CarBody { get; set; }   
    }



    [Table(Name = "Accidents")]
    public class Accidents
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int AccidentId { get; set; }
        [Column(Name = "AmountOfParticipants")]
        public int AmountOfParticipants { get; set; }
        [Column(Name = "AmountOfDied")]
        public int AmountOfDied { get; set; }
        [Column(Name = "DateOfAccident")]
        public int DateOfAccident { get; set; }
        [Column(Name = "CarId")]
        public int CarId { get; set; }

    }


    [Table(Name = "Drivers")]
    public class Drivers
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int DriverId { get; set; }
        [Column(Name = "Experience")]
        public int Experience { get; set; }
    }

    [Table(Name = "Fines")]
    public class Fines
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int FineId { get; set; }

        [Column(Name = "Cost")]
        public int Cost { get; set; }

        [Column(Name = "Cause")]
        public string Cause { get; set; }

        [Column(Name = "IsPaid")]
        public bool IsPaid { get; set; }

        [Column(Name = "DriverId")]
        public int DriverId { get; set; }
    }

    [Table(Name = "CarsDrivers")]
    public class CarsDrivers
    {
        [Column(Name = "DriverId")]
        public int DriverId { get; set; }
        [Column(Name = "CarId")]
        public int CarId { get; set; }
        [Column(Name = "IsOwner")]
        public bool IsOwner { get; set; }
    }

    class MDbContext : DataContext
    {
        public MDbContext(string dbname) : base(dbname) { }

        [Function(Name = "dbo.getDriversWithExperience")]
        [return: Parameter(DbType = "Int")]
        public int AvgExperience([Parameter(Name = "Did", DbType = "Int")] int i)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), i);
            return ((int)(result.ReturnValue));
        }
    }

    class Program
    {
        static void Main(string[] args) 
        {
            //GetUnpaidFines();
            //GetFinesWithExperience(3);
            //UpdateFines();
            //InsertFines();
            //DeleteFirstPaid();
            execProc();
        }

        static void GetUnpaidFines()
        {
            string connectionString = @"Data Source=KONSTANTIN39EC;Initial Catalog=GIBDD;Integrated Security=True";
            DataContext db = new DataContext(connectionString);

            var query = from fines in db.GetTable<Fines>()
                        where fines.IsPaid 
                        select new { fId = fines.FineId, fCause = fines.Cause };
            foreach (var q in query)
            {
                Console.WriteLine("{0,15} \t{1,15}", q.fId, q.fCause);
            }
            Console.WriteLine("End");
            Console.Read();
        }


        static void GetFinesWithExperience(int exp)
        {
            string connectionString = @"Data Source=KONSTANTIN39EC;Initial Catalog=GIBDD;Integrated Security=True";
            DataContext db = new DataContext(connectionString);

            var query = from f in db.GetTable<Fines>()
                        join d in db.GetTable<Drivers>() on f.DriverId equals d.DriverId
                        where d.Experience > exp
                        select new { FId = f.FineId, DId = d.DriverId, Cause = f.Cause };

            foreach (var q in query)
            {
                Console.WriteLine("{0}  {1} {2}", q.FId, q.DId, q.Cause);
            }
            Console.WriteLine("End");
            Console.Read();
        }

        static void getFines()
        {
            string connectionString = @"Data Source=KONSTANTIN39EC;Initial Catalog=GIBDD;Integrated Security=True";
            DataContext db = new DataContext(connectionString);

            var query = from f in db.GetTable<Fines>()
                        select new { FId = f.FineId, Fcost = f.Cost };

            foreach (var q in query)
            {
                Console.WriteLine("{0}  {1}", q.FId, q.Fcost);
            }
            Console.WriteLine("End");
            Console.Read();
        }

        static void UpdateFines()
        {
            getFines();

            string connectionString = @"Data Source=KONSTANTIN39EC;Initial Catalog=GIBDD;Integrated Security=True";
            DataContext db = new DataContext(connectionString);

            var query = from f in db.GetTable<Fines>() where f.IsPaid select f;
            foreach (var q in query) q.Cost = q.Cost * 2;
            db.SubmitChanges();

            getFines();

            Console.Read();
        }

        static void InsertFines()
        {
            string connectionString = @"Data Source=KONSTANTIN39EC;Initial Catalog=GIBDD;Integrated Security=True";
            DataContext db = new DataContext(connectionString);

            db.GetTable<Fines>().InsertOnSubmit(new Fines()
            {
               Cost = 1200,
               Cause = "bus line",
               IsPaid = true,
               DriverId = 5
            });

            db.SubmitChanges();

            Console.Read();
        }

        static void DeleteFirstPaid()
        {
            string connectionString = @"Data Source=KONSTANTIN39EC;Initial Catalog=GIBDD;Integrated Security=True";
            DataContext db = new DataContext(connectionString);

            var item = (from f in db.GetTable<Fines>() where f.IsPaid select f).First();
            db.GetTable<Fines>().DeleteOnSubmit(item);
            db.SubmitChanges();
        }

        static void execProc()
        {
            string connectionString = @"Data Source=KONSTANTIN39EC;Initial Catalog=GIBDD;Integrated Security=True";
            MDbContext db = new MDbContext(connectionString);

            Console.Write("Avg is: " + db.AvgExperience(5));
            Console.Read();
        }



    }
}
