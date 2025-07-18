using EventPoints.Domain.DTOs;
using EventPoints.Domain.Models;
using EventPoints.Web.Services;
using Microsoft.AspNetCore.Components;

namespace EventPoints.Web.Pages
{
	public partial class EditTeam
	{
		[Inject] private EventsService EventsService { get; set; } = default!;
		[Inject] private NavigationManager NavigationManager { get; set; } = default!;
		[Parameter]
		[SupplyParameterFromQuery]
		public Guid TeamId { get; set; }
		public bool IsSaving { get; set; }

		public TeamDto? SelectedTeam { get; set; }

		protected override async Task OnInitializedAsync()
		{
			SelectedTeam = await GetTeam();
		}
		public async Task<TeamDto> GetTeam()
		{
			return await EventsService.GetTeamByIdAsync(TeamId);
		}

		public async Task SaveTeamChanges()
		{
			try
			{
				IsSaving = true;
				await EventsService.EditTeamName(TeamId, SelectedTeam.Name);
				IsSaving = false;
			}
			catch ( HttpRequestException ex )
			{
				Console.WriteLine($"Error saving event changes: {ex.Message}");
			}
		}

		public void SelectedTeamNameChanged(string name)
		{
			SelectedTeam.Name = name;
		}
	}
}