using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq;
using WarshipSearchAPI.DTO;
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
			var connectionString = _configuration.GetConnectionString("DBConnection");
			Log.Information($"Used DB Connection: {connectionString}");
			optionsBuilder.UseSqlServer(connectionString);
		}

		public IEnumerable<Ship> Query(ShipQuery query)
		{
			IQueryable<Ship> linq = Ships;

			if (!string.IsNullOrEmpty(query.ClassName))
				linq = linq.Where(s => s.ClassName.Contains(query.ClassName));

			if (!string.IsNullOrEmpty(query.Nation))
				linq = linq.Where(s => s.ClassName.Contains(query.Nation));

			if (!string.IsNullOrEmpty(query.ClassAbbreviation))
				linq = linq.Where(s => s.ClassName.Contains(query.ClassAbbreviation));


			if (query.MinUnits != null)
				linq = linq.Where(s => s.Units >= query.MinUnits);

			if (query.MaxUnits != null)
				linq = linq.Where(s => s.Units <= query.MaxUnits);


			if (query.MinSpeedIrcwcc != null)
				linq = linq.Where(s => s.SpeedIrcwcc >= query.MinSpeedIrcwcc);

			if (query.MaxSpeedIrcwcc != null)
				linq = linq.Where(s => s.SpeedIrcwcc <= query.MaxSpeedIrcwcc);


			if (query.MinSpeedKnots != null)
				linq = linq.Where(s => s.SpeedKnots >= query.MinSpeedKnots);

			if (query.MaxSpeedKnots != null)
				linq = linq.Where(s => s.SpeedKnots <= query.MaxSpeedKnots);


			if (query.MinLength != null)
				linq = linq.Where(s => s.Length >= query.MinLength);

			if (query.MaxLength != null)
				linq = linq.Where(s => s.Length <= query.MaxLength);


			if (query.MinBeam != null)
				linq = linq.Where(s => s.Beam >= query.MinBeam);

			if (query.MaxBeam != null)
				linq = linq.Where(s => s.Beam <= query.MaxBeam);

			if (query.Skip != null)
				linq = linq.Skip(query.Skip.Value);

			if (query.Take == null)
				query.Take = 25;

			return linq.Take(query.Take.Value).ToList();

		}
	}
}
