using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using Configuration;
using DbModels;
using Microsoft.Extensions.Hosting.Internal;

namespace DbContext;

//DbContext namespace is a fundamental EFC layer of the database context and is
//used for all Database connection as well as for EFC CodeFirst migration and database updates 
public class MainDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    readonly IConfiguration _configuration;

    #region C# model of database tables
    public DbSet<CreditCardDbM> CreditCards { get; set; }
    #endregion

    #region constructors
    public MainDbContext() { }
    public MainDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
    { 
        _configuration = configuration;
    }
    #endregion

    //Here we can modify the migration building
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region override modelbuilder
        #endregion
        
        base.OnModelCreating(modelBuilder);
    }

    //used by the child DbContexts to retrieve the connection string
    protected string GetConnectionString(string connectionStringName)
    {
        string connectionString = null;

        // Check if configuration is available (runtime) or create one for design time
        if (_configuration != null)
        {
            // Runtime: use configuration service
            connectionString = _configuration.GetConnectionString(connectionStringName);
            System.Console.WriteLine($"Runtime Connection String from config: {connectionString}");
        }
        else
        {
            // Design time: manually create configuration to read appsettings.json
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var config = configBuilder.Build();
            connectionString = config.GetConnectionString(connectionStringName);
            System.Console.WriteLine($"Design time Connection String from appsettings.json: {connectionString}");
        }

        return connectionString;
    }

    #region DbContext for some popular databases
    public class SqlServerDbContext : MainDbContext
    {
        public SqlServerDbContext() { }
        public SqlServerDbContext(DbContextOptions options, IConfiguration configuration) 
            : base(options, configuration) { }


        //Used only for CodeFirst Database Migration and database update commands
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = GetConnectionString("SqlServerDocker");
                optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HaveColumnType("money");
            configurationBuilder.Properties<string>().HaveColumnType("varchar(200)");

            base.ConfigureConventions(configurationBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Add your own modelling based on done migrations
            base.OnModelCreating(modelBuilder);
        }
    }

    public class MySqlDbContext : MainDbContext
    {
        public MySqlDbContext() { }
        public MySqlDbContext(DbContextOptions options) : base(options, null) { }


        //Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = GetConnectionString("MySqlDocker");
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                    b => b.SchemaBehavior(Pomelo.EntityFrameworkCore.MySql.Infrastructure.MySqlSchemaBehavior.Translate, (schema, table) => $"{schema}_{table}"));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveColumnType("varchar(200)");

            base.ConfigureConventions(configurationBuilder);

        }
    }

    public class PostgresDbContext : MainDbContext
    {
        public PostgresDbContext() { }
        public PostgresDbContext(DbContextOptions options) : base(options, null){ }


        //Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = GetConnectionString("PostgreSqlDocker");
                System.Console.WriteLine($"Connection String: {connectionString}");
                
                optionsBuilder.UseNpgsql(connectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveColumnType("varchar(200)");
            base.ConfigureConventions(configurationBuilder);
        }
    }
    #endregion
}
