using EventPoints.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPoints.API.Database
{
	public class EPDbContext(DbContextOptions<EPDbContext> options) : DbContext(options)  
	{
		public DbSet<Event> Events { get; set; }
		public DbSet<Team> Teams { get; set; }
	}
}
