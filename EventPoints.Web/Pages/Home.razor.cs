using EventPoints.Domain.Models;
using EventPoints.Web.Services;
using Microsoft.AspNetCore.Components;
using System.Runtime.InteropServices;

namespace EventPoints.Web.Pages
{
	public partial class Home
	{
		[Inject] private EventsService EventsService { get; set; }

		public List<Event>? Events { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await GetEvents();
		}

		public async Task GetEvents()
		{
			try
			{
				Events = [.. await EventsService.GetEventsAsync()];
			}
			catch ( HttpRequestException ex )
			{
				Console.Error.WriteLine($"Error fetching events: {ex.Message}");
			}
		}

		public async Task UpdatePoints(Guid eventId, Guid teamId, int points)
		{
			try
			{
				await EventsService.UpdateTeamPoints(eventId, teamId, points);
				await GetEvents();
			}
			catch ( HttpRequestException ex )
			{
				Console.Error.WriteLine($"Error updating points: {ex.Message}");
			}
		}
	}
}
