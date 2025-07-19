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
		private HubConnection? hubConnection;

		public ObservableCollection<EventDto>? EventsList { get; set; } = null;
		public Guid SelectedEventId { get; set; } = Guid.Empty;
		public EventDto? SelectedEvent { get; set; } = null;
		public TeamDto? SelectedTeam { get; set; } = null;
		public bool IsLoading { get; set; }

		protected override async Task OnInitializedAsync()
		{
			try
			{
				IsLoading = true;
				using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
				await EventsService.GetEventsAsync(cts.Token);
				cts.Token.ThrowIfCancellationRequested();
				EventsList = EventsService.Events;
				if (EventsList.Any())
				{
					SelectedEvent = EventsList.FirstOrDefault();
					SelectedTeam = SelectedEvent.Teams.FirstOrDefault();
				}
				IsLoading = false;
			}
			catch (OperationCanceledException ex)
			{
				IsLoading = false;
				EventsList = null;
			}
			catch (HttpRequestException ex)
			{
				IsLoading = false;
				Console.WriteLine($"Error fetching events: {ex.Message}");
				EventsList = null;
			}
			catch (Exception ex)
			{
				IsLoading = false;
				Console.WriteLine($"Unexpected error: {ex.Message}");
				EventsList = null;
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
