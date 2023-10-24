using Microsoft.EntityFrameworkCore;
using WarshipSearchAPI.Data;

namespace WarshipSearchAPI.Interfaces
{
	public interface IWarshipDatabase
	{
		DbSet<Ship> Ships { get; }
	}
}