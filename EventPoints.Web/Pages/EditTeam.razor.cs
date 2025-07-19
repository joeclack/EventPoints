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
		public bool IsEditing { get; set; }
		public bool IsLoading { get; set; }


		public TeamDto? SelectedTeam { get; set; }

		protected override async Task OnInitializedAsync()
		{
			SelectedTeam = await GetTeam();
		}
		public async Task<TeamDto?> GetTeam()
		{
			try
			{
				IsLoading = true;
				using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
				var team = await EventsService.GetTeamByIdAsync(TeamId, cts.Token);
				IsLoading = false;
				return team;
			}
			catch (OperationCanceledException ex)
			{
				IsLoading = false;
				return null;
			}
			catch (HttpRequestException ex)
			{
				IsLoading = false;
				Console.WriteLine($"Error fetching team: {ex.Message}");
				return null;
			}
			catch (Exception ex)
			{
				IsLoading = false;
				Console.WriteLine($"Unexpected error: {ex.Message}");
				return null;
			}
		}

		public void Refresh()
		{
			NavigationManager.NavigateTo(NavigationManager.Uri);
		}

		public async Task SaveTeamChanges()
		{
			try
			{
				IsSaving = true;
				IsEditing = true;
				using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
				await EventsService.EditTeamName(TeamId, SelectedTeam.Name, cts.Token);
				IsSaving = false;
				IsEditing = false;
			}
			catch (HttpRequestException ex)
			{
			}
			catch (OperationCanceledException ex)
			{
			}
		}

		public void SelectedTeamNameChanged(string name)
		{

			if (name != SelectedTeam.Name)
			{
				SelectedTeam.Name = name;
				IsEditing = true;
				return;
			}
			IsEditing = false;
		}
	}
}