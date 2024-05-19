using ManagemateAPI.Database.Tables;
using ManagemateAPI.Encryption;
using Microsoft.EntityFrameworkCore;

namespace ManagemateAPI.Database.Context
{
    public class DB_Context : DbContext
    {
        private long user_id;
        private readonly IConfiguration _configuration;


        public DB_Context(long userid, IConfiguration configuration) 
        {
            user_id = userid;
            _configuration = configuration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(

                @"Server=" + _configuration.GetValue<string>("Database:Server") +
                ";Port=" + _configuration.GetValue<string>("Database:Port") + 
                ";Database=" + _configuration.GetValue<string>("Database:DB") + user_id + 
                ";User id=" + _configuration.GetValue<string>("Database:User") + 
                ";Password=" + Crypto.GetPasswd() + ";"
                
                );


        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item_Trading_Type>().HasData(

                new Item_Trading_Type { id = 1, trading_type_pl = "dzierżawa", trading_type_en = "lease" },
                new Item_Trading_Type { id = 2, trading_type_pl = "sprzedaż", trading_type_en = "sale" },
                new Item_Trading_Type { id = 3, trading_type_pl = "usługa", trading_type_en = "service" }

            );


            modelBuilder.Entity<Item_Type>().HasData(

                new Item_Type { id = -1, item_type = new byte[1], rate = new byte[1] }

            );


            modelBuilder.Entity<Item_Counting_Type>().HasData(

                new Item_Counting_Type { id = -1, counting_type = "-" }

            );

        }


        public DbSet<Authorized_Worker> Authorized_Worker { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Receipt> Receipt { get; set; }
        public DbSet<Item_On_Receipt> Item_On_Receipt { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Construction_Site> Construction_Site { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Cities_List> Cities_List { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Item_Counting_Type> Item_Counting_Type { get; set;}
        public DbSet<Item_Type> Item_Type { get; set; }
        public DbSet<Item_Trading_Type> Item_Trading_Type { get; set;}
    }
}
