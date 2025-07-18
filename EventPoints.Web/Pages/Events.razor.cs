using EventPoints.Domain.DTOs;
using EventPoints.Domain.Models;
using EventPoints.Web.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace EventPoints.Web.Pages
{
	public partial class Events
	{
		[Inject] private EventsService EventsService { get; set; } = default!;

		public ObservableCollection<EventDto>? EventsList { get; set; } = null;
		public Guid SelectedEventId { get; set; } = Guid.Empty;
		public EventDto? SelectedEvent { get; set; } = null;
		public TeamDto? SelectedTeam { get; set; } = null;

		protected override async Task OnInitializedAsync()
		{
			try
			{
				await EventsService.GetEventsAsync();
				EventsList = EventsService.Events;
				SelectedEvent = EventsList.FirstOrDefault();
				SelectedTeam = SelectedEvent.Teams.FirstOrDefault();
			} catch (Exception ex)
			{

			}
		}

		private void OnEventChanged(EventDto e)
		{
			SelectedEvent = EventsList.FirstOrDefault(ev => ev == e);
		}

		private void OnTeamChanged(TeamDto e)
		{
			SelectedTeam = SelectedEvent.Teams.FirstOrDefault(x => x.Id == e.Id);
		}
	}
}
