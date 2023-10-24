using Microsoft.EntityFrameworkCore;

namespace DataUpload.Data
{
	public class Database : DbContext
    {
        string connectionString = @"Server=tcp:warships-sql.database.windows.net,1433;Initial Catalog=WarshipsDB;Persist Security Info=False;User ID=JustinScott;Password=1999Vette;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

      public  DbSet<Ship> Ships {  get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
