using Serilog;
using Serilog.Context;
using Serilog.Events;
using Serilog.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using Serilog.Enrichers;
using System.Globalization;
using System.Threading;
using Serilog.Sinks.MSSqlServer;
using System.Linq;

namespace ConsoleApp1
{
    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
    }


    class Person
    {
        public string Name { get; set; }
        public List<Pet> Pets { get; set; } = new List<Pet>();
    }
    class Pet
    {
        public string PetName { get; set; }
        public Person Master { get; set; }
    }

    class DateTimeFormatProvider : IFormatProvider
    {

        public object GetFormat(Type formatType)
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {


        static void Main(string[] args)
        {

            var logDB = @"Data Source=.;Initial Catalog=serilog_mssqlserver_2;User ID=sa;Password=123123;";
            var logTable = "Log4";
            var options = new ColumnOptions();
            options.Store.Remove(StandardColumn.Properties);
            options.Store.Add(StandardColumn.LogEvent);
            options.LogEvent.DataLength = 2048;
            //options.PrimaryKey = options.TimeStamp;
            //options.TimeStamp.NonClusteredIndex = true;




            SqlColumn sqlColumn = new SqlColumn("", System.Data.SqlDbType.DateTime, true);

            Serilog.Debugging.SelfLog.Enable(Console.Error);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                //.Filter.ByExcluding(Matching.WithProperty<int>("Count", p => p > 10))
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Destructure.ToMaximumDepth(4)
                //.WriteTo.MySink()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Verbose)
                .WriteTo.Async(cfg =>
                {
                    cfg.File(path: "logs/myapp.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 100, rollOnFileSizeLimit: true, formatProvider: new CultureInfo("zh-CN"));
                })
                //.WriteTo.MSSqlServer(
                //    connectionString: @"Data Source=.;Initial Catalog=serilog_mssqlserver_1;User ID=sa;Password=123123;",
                //    tableName: "loginfo1",
                //    batchPostingLimit: 50,
                //    period: TimeSpan.FromSeconds(5),
                //    autoCreateSqlTable: true
                //)
                .WriteTo.MSSqlServer(
                    connectionString: logDB,
                    tableName: logTable,
                    columnOptions: options,
                    batchPostingLimit: 50,
                    period: TimeSpan.FromSeconds(5),
                    autoCreateSqlTable: true
                )
                .CreateLogger();


            //var exampleUser = new User { Id = 1, Name = "Adam", Created = DateTime.Now };

            //var myLog = Log.ForContext<Program>();
            //for (int i = 0; i < 1000; i++)
            //{
            //    exampleUser.Id += 1;
            //    Log.Information("Created {@User} on {Created}", exampleUser, DateTime.Now);

            //    Log.Information("num :{@Count}", 100);
            //    Log.Information("num :{@Count}", 8);
            //    myLog.Information("Hello! {@SourceContext}");

            //    Log.Debug("Debug log~~~~~~~~~~~~");
            //    Log.Verbose("Verbose log~~~~~~~~~~~~~~~");
            //    //Thread.Sleep(500);
            //}


            Person p1 = new Person { Name = "person1" };

            Pet pet1 = new Pet { PetName = "Pet1", Master = p1 };
            Pet pet2 = new Pet { PetName = "Pet2", Master = p1 };

            p1.Pets.Add(pet1);
            p1.Pets.Add(pet2);

            Person p2 = new Person { Name = "person2" };


            Log.Information("{@Pet1}", pet1);

            Log.CloseAndFlush();



            Console.WriteLine("-------------Done--------------");
            Console.ReadKey();
        }
    }
}
