using Microsoft.EntityFrameworkCore;
using WarshipSearchAPI.Interfaces;

namespace WarshipSearchAPI.Data
{
	public class WarshipDatabase : DbContext, IWarshipDatabase
	{
		private readonly IConfiguration _configuration;
		
		public WarshipDatabase(IConfiguration configuration)
		{
			_configuration = configuration;
		}


		public DbSet<Ship> Ships { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var connectionString = _configuration.GetConnectionString("DbConnection");
			optionsBuilder.UseSqlServer(connectionString);
		}
	}
}
