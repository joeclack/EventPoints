using EventPoints.Domain.DTOs;
using EventPoints.Domain.Models;
using EventPoints.Web.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace EventPoints.Web.Pages
{
	public partial class EditEvent
	{
		[Parameter]
		[SupplyParameterFromQuery]
		public Guid EventId { get; set; }

		[Inject] private EventsService EventsService { get; set; } = default!;
		[Inject] private NavigationManager NavigationManager { get; set; } = default!;
		public EventDto? SelectedEvent { get; set; } = null;
		public bool IsSaving { get; set; }
		public bool IsEditing {get; set;}
		public bool IsLoading { get; set; }

		protected override async Task OnInitializedAsync()
		{
			SelectedEvent = await GetEvent();
		}

		public async Task<EventDto> GetEvent()
		{
			IsLoading = true;
			using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
			var @event = await EventsService.GetEventByIdAsync(EventId, cts.Token);
			IsLoading = false;
			return @event;
		}

		public async Task CreateTeam()
		{
			string name = "New Team";
			using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
			await EventsService.CreateTeam(name, SelectedEvent.Id, cts.Token);
			await RefreshEvent();
		}

		public async Task RefreshEvent()
		{
			IsLoading = true;
			using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
			SelectedEvent = await EventsService.GetEventByIdAsync(SelectedEvent.Id, cts.Token);
			IsLoading = false;
		}

		public async Task SaveEventChanges()
		{
			try
			{
				using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
				IsSaving = true;
				await EventsService.EditEventName(EventId, SelectedEvent.Name, cts.Token);
				IsSaving = false;
				IsEditing = false;
			}
			catch ( HttpRequestException ex )
			{
				Console.WriteLine($"Error saving event changes: {ex.Message}");
			}
		}

		public void EventNameChanged(string name)
		{
			if(name != SelectedEvent.Name) {
				SelectedEvent.Name = name;
				IsEditing = true;
				return;
			}
			IsEditing = false;
		}

		public async Task DeleteTeam(TeamDto team)
		{
			using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
			await EventsService.DeleteTeam(team.Id, EventId, cts.Token);
			SelectedEvent = await GetEvent();
		}

		public void EditTeam(TeamDto team)
		{
			NavigationManager.NavigateTo($"/manage/edit-team?Teamid={team.Id}");
		}
	}
}