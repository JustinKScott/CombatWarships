using Microsoft.AspNetCore.Mvc;
using WarshipSearchAPI.Data;
using WarshipSearchAPI.Interfaces;

namespace WarshipSearchAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WarshipSearchController : ControllerBase
	{
		private readonly IWarshipDatabase _database;

		public WarshipSearchController(IWarshipDatabase database)
		{
			_database = database;
		}

		[HttpGet(Name = "SearchWarships")]
		public IEnumerable<Ship> Get()
		{
			return _database.Ships.ToArray();
		}
	}
}