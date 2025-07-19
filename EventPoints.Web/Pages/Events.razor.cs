using EventPoints.Domain.DTOs;
using EventPoints.Domain.Models;
using EventPoints.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using static System.Net.WebRequestMethods;

namespace EventPoints.Web.Pages
{
	public partial class Events
	{
		[Inject] private EventsService EventsService { get; set; } = default!;
		[Inject] private NavigationManager Navigation { get; set; } = default!;
		private HubConnection? hubConnection;

		public ObservableCollection<EventDto>? EventsList { get; set; } = null;
		public Guid SelectedEventId { get; set; } = Guid.Empty;
		public EventDto? SelectedEvent { get; set; } = null;
		public TeamDto? SelectedTeam { get; set; } = null;
		public bool IsLoading { get; set; }

		protected override async Task OnInitializedAsync()
		{
			hubConnection = new HubConnectionBuilder()
			.WithUrl(Navigation.ToAbsoluteUri("/pointshub"))
			.Build();

			hubConnection.On<Guid, Guid, int>("PointsUpdated", async (eventId, teamId, points) =>
			{
				if ( eventId == SelectedEventId && SelectedEvent != null )
				{
					var teamToUpdate = SelectedEvent.Teams.FirstOrDefault(t => t.Id == teamId);
					if ( teamToUpdate != null )
					{
						teamToUpdate.Points = points;

						await InvokeAsync(StateHasChanged);
					}
				}
			});

			await hubConnection.StartAsync();

			await GetEvents();
		}

		public async ValueTask DisposeAsync()
		{
			if ( hubConnection is not null )
			{
				await hubConnection.DisposeAsync();
			}
		}

		public async Task GetEvents()
		{
			try
			{
				IsLoading = true;
				using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
				await EventsService.GetEventsAsync(cts.Token);
				cts.Token.ThrowIfCancellationRequested();
				EventsList = EventsService.Events;
				if ( EventsList.Any() )
				{
					SelectedEvent = EventsList.FirstOrDefault();
					SelectedEventId = SelectedEvent?.Id ?? Guid.Empty;
					SelectedTeam = SelectedEvent.Teams.FirstOrDefault();
				}
				IsLoading = false;
			}
			catch ( OperationCanceledException ex )
			{
				IsLoading = false;
				EventsList = null;
			}
			catch ( HttpRequestException ex )
			{
				IsLoading = false;
				Console.WriteLine($"Error fetching events: {ex.Message}");
				EventsList = null;
			}
			catch ( Exception ex )
			{
				IsLoading = false;
				Console.WriteLine($"Unexpected error: {ex.Message}");
				EventsList = null;
			}
		}

		private void OnEventChanged(EventDto e)
		{
			SelectedEvent = EventsList.FirstOrDefault(ev => ev == e);
			SelectedEventId = e.Id;
		}

		private void OnTeamChanged(TeamDto e)
		{
			SelectedTeam = SelectedEvent.Teams.FirstOrDefault(x => x.Id == e.Id);
		}
	}
}
