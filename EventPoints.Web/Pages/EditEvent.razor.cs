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

		protected override async Task OnInitializedAsync()
		{
			SelectedEvent = await GetEvent();
		}

		public async Task<EventDto> GetEvent()
		{
			return await EventsService.GetEventByIdAsync(EventId);
		}

		public async Task SaveEventChanges()
		{
			try
			{
				await EventsService.EditEventName(EventId, SelectedEvent.Name);
			}
			catch ( HttpRequestException ex )
			{
				Console.WriteLine($"Error saving event changes: {ex.Message}");
			}
		}

		public void EventNameChanged(string name)
		{
			SelectedEvent.Name = name;
		}

		public async Task DeleteTeam(TeamDto team)
		{
			await EventsService.DeleteTeam(team.Id, EventId);
			SelectedEvent = await GetEvent();
		}

		public void EditTeam(TeamDto team)
		{
			NavigationManager.NavigateTo($"/edit-team?Teamid={team.Id}");
		}
	}
}