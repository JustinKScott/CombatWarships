using Microsoft.EntityFrameworkCore;
using WarshipSearchAPI.Data;
using WarshipSearchAPI.DTO;

namespace WarshipSearchAPI.Interfaces
{
	public interface IWarshipDatabase
	{
		IEnumerable<Ship> Query(ShipQuery query);
	}
}