using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Context;
using WarshipSearchAPI.Data;
using WarshipSearchAPI.DTO;
using WarshipSearchAPI.Interfaces;

namespace WarshipSearchAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SearchWarshipsController : ControllerBase
	{
		private readonly IWarshipDatabase _database;

		public SearchWarshipsController(IWarshipDatabase database)
		{
			_database = database;
		}

		[HttpPost(Name = "Search")]
		public IEnumerable<Ship> Search(ShipQuery query)
		{
			using (LogContext.PushProperty("Query", query))
			{
				Log.Information("Search Request");
				var result = _database.Query(query);
				Log.Information($"{result.Count()} results were found.");

				// TODO: Send through automapper
				return result;
			}
		}
	}
}