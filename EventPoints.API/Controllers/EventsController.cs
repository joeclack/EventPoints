using EventPoints.API.Database;
using EventPoints.Domain.DTOs;
using EventPoints.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventPoints.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class EventsController : Controller
	{
		private EPDbContext _db { get; }

		public EventsController(EPDbContext dbContext)
		{
			_db = dbContext;
		}

		[HttpPost]
		public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
		{
			if ( string.IsNullOrWhiteSpace(request.Name) )
				return BadRequest("Event name cannot be empty.");

			var newEvent = new Event(request.Name);
			await _db.Events.AddAsync(newEvent);
			await _db.SaveChangesAsync();

			return Ok(newEvent);
		}

		[HttpDelete("{eventId}")]
		public async Task<IActionResult> DeleteEvent(Guid eventId)
		{
			var @event = await _db.Events.Include(e => e.Teams).FirstOrDefaultAsync(e => e.Id == eventId);
			if ( @event == null )
				return NotFound($"Event with ID {eventId} not found.");

			_db.Events.Remove(@event);
			await _db.SaveChangesAsync();
			return Ok();
		}

		[HttpPost("teams")]
		public async Task<IActionResult> AddTeam([FromBody] CreateTeamRequest request)
		{
			if ( string.IsNullOrWhiteSpace(request.Name) )
				return BadRequest("Team name cannot be empty.");

			var team = new Team(request.eventId, request.Name);
			await _db.Teams.AddAsync(team);
			await _db.SaveChangesAsync();

			return Ok(team);
		}

		[HttpDelete("{eventId}/teams/{teamId}")]
		public async Task<IActionResult> DeleteTeam(Guid eventId, Guid teamId)
		{
			var team = await _db.Teams.FirstOrDefaultAsync(t => t.Id == teamId && t.EventId == eventId);
			if ( team == null )
				return NotFound($"Team with ID {teamId} not found.");

			_db.Teams.Remove(team);
			await _db.SaveChangesAsync();
			return Ok();
		}

		[HttpPut("{eventId}/teams/{teamId}/updatePoints")]
		public async Task<IActionResult> UpdatePoints(Guid eventId, Guid teamId, [FromBody] UpdatePointsRequest request)
		{
			var team = await _db.Teams.FirstOrDefaultAsync(t => t.Id == teamId && t.EventId == eventId);
			if ( team == null )
				return NotFound($"Team with ID {teamId} not found.");

			team.Points += request.Points;
			_db.Teams.Update(team);
			await _db.SaveChangesAsync();

			return Ok();
		}

		[HttpPut("{eventId}/teams/{teamId}/setPoints")]
		public async Task<IActionResult> SetPoints(Guid eventId, Guid teamId, [FromBody] SetPointsRequest request)
		{
			var team = await _db.Teams.FirstOrDefaultAsync(t => t.Id == teamId && t.EventId == eventId);
			if ( team == null )
				return NotFound($"Team with ID {teamId} not found.");
			team.Points = request.Points;
			_db.Teams.Update(team);
			await _db.SaveChangesAsync();
			return Ok();
		}

		[HttpGet]
		public async Task<IActionResult> GetEvents()
		{
			var events = await _db.Events.Include(e => e.Teams).ToListAsync();
			return Ok(events);
		}

		[HttpGet("{eventId}")]
		public async Task<IActionResult> GetEvent(Guid eventId)
		{
			var @event = await _db.Events.Include(e => e.Teams).FirstOrDefaultAsync(e => e.Id == eventId);
			if ( @event == null )
				return NotFound($"Event with ID {eventId} not found.");

			return Ok(@event);
		}

		[HttpGet("{eventId}/scoreboard")]
		public async Task<IActionResult> GetScoreboard(Guid eventId)
		{
			var teams = await _db.Teams
			.Where(t => t.EventId == eventId)
			.OrderByDescending(t => t.Points)
			.ToListAsync();

			return Ok(teams);
		}
	}
}
