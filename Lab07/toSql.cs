class ToSql {
    class MyDbContext : DataContext
    {
        public MyDbContext(string dbname) : base(dbname) { }
        [Function(Name = "dbo.FactorialProc")]
        [return: Parameter(DbType = "Int")]
        public int FactorialProc([Parameter(Name = "i", DbType = "Int")] string i)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), i);
            return ((int)(result.ReturnValue));
        }

        public Table<Doctor> tblDoctors;
        public Table<SpecialtyName> tblSpecialtyNames;
    }

    [Table(Name = "tblDoctors")] public class Doctor { [Column(IsPrimaryKey = true)]
    public int idDoctor; 
    [Column()] public string firstName;
    [Column()] public string lastName;
    [Column()] public int idSpec;
    [Column()] public int experience;
    }


    [Table(Name = "tblSpecialtyNames")] public class SpecialtyName {
        [Column(IsPrimaryKey = true)]
        public int idSpec;

        [Column()] public string specialtyName;
    }

    MyDbContext db = new MyDbContext(@"C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Clinic.mdf");
    public ToSql()
    {
        db.tblDoctors = db.GetTable<Doctor>(); db.tblSpecialtyNames = db.GetTable<SpecialtyName>();
    }

    public void QueryAll() {
        Query1();
        Console.ReadLine();
        Query2();
        Console.ReadLine();
        Update();
        Console.ReadLine();
        Insert();
        Console.ReadLine();
        Delete();
        Console.ReadLine();
        QueryProc();
        Console.ReadLine();
    }

    void Query1()
    {
        var query = from d in db.tblDoctors where d.experience > 35 select d;
        foreach (var d in query) {
            Console.WriteLine("Doctor[{0}] has more than 35 years experience({1} exp)", d.idDoctor, d.experience);
        }  Console.WriteLine(); } void Query2() { var tblDoctors = db.tblDoctors;
        var tblSpecialtyNames = db.tblSpecialtyNames;

        var query = from d in tblDoctors join sn in tblSpecialtyNames on d.idSpec equals sn.idSpec where d.experience > 48 select new { d, sn };

        foreach (var r in query) { Console.WriteLine("Doctor[{0}] has more than 48 years exp in specialty {1}", r.d.lastName, r.sn.specialtyName);
        }
    }

    private void Update() {
        var query = from d in db.tblDoctors where d.experience > 35 select d;
        foreach (var d in query) d.experience = 35;
        db.SubmitChanges();
        Query1();
    }

    private void Insert() {
        db.tblDoctors.InsertOnSubmit(new Doctor() {
            idDoctor = 666, firstName = "ktulhu", lastName = "Uthor", experience = 1984
        });

        db.SubmitChanges();
        Query1();
        Console.ReadKey();
    }

    private void Delete() {
        var item = (from d in db.tblDoctors where d.experience > 10 select d).First();
        db.tblDoctors.DeleteOnSubmit(item); db.SubmitChanges();
        Query1();
    }

    void QueryProc() {
        Console.WriteLine("Factorial(5) == " + db.FactorialProc("5")); Console.WriteLine();
    }
}