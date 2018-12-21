using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace task2
{
    [Table(Name = "Department")]
    public class Department
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column(Name = "Title")]
        public string Title { get; set; }
        [Column(Name = "Date_create")]
        public DateTime Date_create { get; set; }
    }

    [Table(Name = "Empl")]
    public class Empl
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column(Name = "FIO")]
        public string FIO { get; set; }
        [Column(Name = "Date_create")]
        public DateTime Date_create { get; set; }
        [Column(Name = "Depart")]
        public int Depart { get; set; }
    }

    [Table(Name = "Uchet")]
    public class Uchet
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column(Name = "id_empl")]
        public int id_empl { get; set; }
        [Column(Name = "sysdate")]
        public DateTime sysdate { get; set; }
        [Column(Name = "day_week")]
        public string day_week { get; set; }
        [Column(Name = "time_")]
        public DateTime time_ { get; set; }
        [Column(Name = "type_")]
        public int type_ { get; set; }
    }

    

    class Program
    {
        static void GetLateEmpl()
        {
            string connectionString = @"Data Source=DESKTOP-3N6D323\SQLEXPRESS;Initial Catalog=RK_exam;Integrated Security=True";
            DataContext db = new DataContext(connectionString);
            TimeSpan ts = TimeSpan.Parse("9:00");
            var query = from uchet_ in db.GetTable<Uchet>()
                        where (uchet_.time_.TimeOfDay > ts) && (uchet_.type_ == 1)
                        join empl in db.GetTable<Empl>() on uchet_.id_empl equals empl.Id
                        join dep in db.GetTable<Department>() on empl.Depart equals dep.Id
                        select new { FIO_ = empl.FIO, Dep = dep.Title };
            query = query.Distinct();
            foreach (var em in query)
            {
                Console.WriteLine("{0,15} \t{1,15}", em.FIO_, em.Dep);
            }
            Console.WriteLine("End");
            Console.Read();
        }

        static void GetLateEmplTenDays()
        {
            string connectionString = @"Data Source=DESKTOP-3N6D323\SQLEXPRESS;Initial Catalog=RK_exam;Integrated Security=True";
            DataContext db = new DataContext(connectionString);
            TimeSpan ts = TimeSpan.Parse("9:00");
            DateTime ts_date = new DateTime(2018, 12, 11, 18, 30, 25);
            
            var query = from dep in db.GetTable<Department>()
                select new {id = dep.Id, tit = dep.Title};
                        
            foreach (var em in query)
            {
                var we = from uchet_ in db.GetTable<Uchet>()
                         join empl in db.GetTable<Empl>() on uchet_.id_empl equals empl.Id
                         where (uchet_.time_.TimeOfDay > ts) && (uchet_.type_ == 1) && (uchet_.sysdate > ts_date.Date) && empl.Depart == em.id
                         select new { id = em.id, tit = em.tit };
                if (we.Count() > 2)
                    Console.WriteLine("{0,15}\n", em.tit);
            }
            Console.WriteLine("End");
            Console.Read();
        }

        static void GetMinWork()
        {
            string connectionString = @"Data Source=DESKTOP-3N6D323\SQLEXPRESS;Initial Catalog=RK_exam;Integrated Security=True";
            DataContext db = new DataContext(connectionString);
            TimeSpan ts = TimeSpan.Parse("9:00");
            DateTime ts_date = DateTime.Today;

            var query = from uchet_ in db.GetTable<Uchet>()
                     join empl in db.GetTable<Empl>() on uchet_.id_empl equals empl.Id
                     join uchet_2 in db.GetTable<Uchet>() on uchet_.id_empl equals uchet_2.id_empl
                        where uchet_2.type_ == 2 && uchet_.type_ == 1 && uchet_.sysdate.Date == ts_date.Date &&
                        uchet_.sysdate.Date == uchet_2.sysdate.Date
                        //orderby uchet_2.time_.TimeOfDay - uchet_.time_.TimeOfDay
                        select new { id = empl.Id, name = empl.FIO, time = uchet_2.time_.TimeOfDay - uchet_.time_.TimeOfDay};

            //query = query.Distinct();
            TimeSpan tspp = query.First().time;
            foreach (var em in query)
            {
                if (tspp != em.time)
                    return;
                Console.WriteLine("{0,15} {1,15}\n", em.name, em.time);
            }
            
            Console.WriteLine("End");
            Console.Read();
        }

        static void Main(string[] args)
        {
            //GetLateEmpl();
            //GetLateEmplTenDays();
            GetMinWork();
        }
    }
}
