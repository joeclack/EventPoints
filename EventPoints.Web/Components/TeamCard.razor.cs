using EventPoints.Domain.DTOs;
using EventPoints.Domain.Models;
using EventPoints.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace EventPoints.Web.Components
{
	public partial class TeamCard
	{
		[Inject] private EventsService EventsService { get; set; } = default!;
		[Parameter] public TeamDto? Team { get; set; }
		public int PointsInput { get; set; }

		public void PointsChanged(int points)
		{
			PointsInput = points;
			StateHasChanged();
		}

		public async Task UpdatePoints()
		{
			try
			{
				await EventsService.IncrementTeamPoints(Team ,PointsInput);
				Team.Points += PointsInput;
				StateHasChanged();
			}
			catch ( HttpRequestException ex )
			{
				Console.Error.WriteLine(ex.Message);
			}
		}

		public async Task ResetPoints()
		{
			try
			{
				await EventsService.SetTeamPoints(Team, 0);
				Team.Points = 0;
				StateHasChanged();
			}
			catch ( HttpRequestException ex )
			{
				Console.Error.WriteLine(ex.Message);
			}
		}
	}
}