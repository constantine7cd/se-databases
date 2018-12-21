using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Lab7_1
{
    [Serializable]
    public class Cars
    {
        public Cars(int id, string name, string vin, string body, int year)
        {
            Id = id;
            Name = name;
            Vin = vin;
            Body = body;
            Year = year;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Vin { get; set; }
        public string Body { get; set; }
        public int Year { get; set; }

    }

    public class Fines
    {
        public Fines(int id, int cost, string cause, bool isPaid, int driverId)
        {
            Id = id;
            Cost = cost;
            Cause = cause;
            IsPaid = isPaid;
            DriverId = driverId;
        }

        public int Id { get; set; }
        public int Cost { get; set; }
        public string Cause { get; set; }
        public bool IsPaid { get; set; }
        public int DriverId { get; set; }
    }

    public class Accidents
    {
        public Accidents(int id, int amountParticip, int amountDied, int date)
        {
            Id = id;
            AmountParticip = amountParticip;
            AmountDied = amountDied;
            Date = date;
        }

        public int Id { get; set; }
        public int AmountParticip { get; set; }
        public int AmountDied { get; set; }
        public int Date { get; set; }

    }

    public class Drivers
    {
        public Drivers(int id, int experience)
        {
            Id = id;
            Experience = experience;
        }

        public int Id { get; set; }
        public int Experience { get; set; }
    }

    public class Gibdd
    {
        private static List<Accidents> accts;
        private static List<Cars> cars;
        private static List<Fines> fines;
        private static List<Drivers> drivers;

        public static IList<Accidents> GetAccidents()
        {
            if (accts == null)
            {
                accts = new List<Accidents>(3);
                accts.Add(new Accidents(1, 2, 0, 2018));
                accts.Add(new Accidents(2, 5, 0, 2017));
                accts.Add(new Accidents(3, 3, 2, 2018));
            }
            return accts;
        }

        public static IList<Cars> GetCars()
        {
            if (cars == null)
            {
                cars = new List<Cars>(3);
                cars.Add(new Cars(1,"lincoln", "wdd8ca8bb8", "limousine", 1969));
                cars.Add(new Cars(2, "maybach", "wdd8789bb8", "sedan", 2008));
                cars.Add(new Cars(3, "lada", "wqwe32428bb8", "coupe", 1978));

            }
            return cars;
        }

        public static IList<Fines> GetFines()
        {
            if (fines == null)
            {
                fines = new List<Fines>(3);
                fines.Add(new Fines(1, 2999, "busline", false, 3));
                fines.Add(new Fines(2, 1999, "red_light", true, 1));
                fines.Add(new Fines(3, 5999, "over100", false, 1));

            }
            return fines;
        }

        public static IList<Drivers> GetDrivers()
        {
            if (drivers == null)
            {
                drivers = new List<Drivers>(3);
                drivers.Add(new Drivers(1, 5));
                drivers.Add(new Drivers(2, 2));
                drivers.Add(new Drivers(3, 6));
            }
            return drivers;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            //GetUnpaidFines();
            //GetFinesForExperience(1);
            //CalcAvgFineByDriver();
            //GetCarsByBody("Coupe");
            //GetOrderedCars();
            Console.ReadLine();
        }
        static void GetUnpaidFines()
        {
            var fines = from p in Gibdd.GetFines()
                       where p.IsPaid == true
                       select p;

            foreach (var f in fines)
            {
                Console.WriteLine("{0} {1} {2}", f.DriverId, f.Cause, f.Cost);
            }
        }

        static void GetFinesForExperience(int exp)
        {
            IList<Fines> fines = Gibdd.GetFines();
            IList<Drivers> drivers = Gibdd.GetDrivers();
            var query = from f in fines
                        join d in drivers on f.DriverId equals d.Id
                        where d.Experience > exp
                        select new { FId = f.Id, DId = d.Id, Cause = f.Cause };

            foreach(var q in query)
            {
                Console.WriteLine("{0} {1} {2}", q.FId, q.DId, q.Cause);
            }
            Console.WriteLine("End");
            Console.Read();
        }

        static void CalcAvgFineByDriver()
        {
            var query = from f in Gibdd.GetFines()
                       group f by f.DriverId into g
                       select new {Did = g.Key,  AvgFines = g.Average(k => k.Cost) };

            foreach (var q in query)
            {
                Console.WriteLine("{0}  {1}", q.Did, q.AvgFines);
            }
        }

        static void GetCarsByBody(string name)
        {
            var query = from c in Gibdd.GetCars()
                        let carBody = c.Body.ToLower()
                       where carBody == name.ToLower()
                       select c.Name;

            foreach (var q in query)
            {
                Console.WriteLine("{0}", q);
            }
        }

        static void GetOrderedCars()
        {
            var query = from c in Gibdd.GetCars()
                       orderby c.Body
                       select new {Name = c.Name, Body = c.Body };
                        
            foreach (var q in query)
            {
                Console.WriteLine("{0} {1}", q.Name, q.Body);
            }
        }

    }
    
}
