using EventPoints.Domain.DTOs;
using EventPoints.Domain.Models;
using EventPoints.Web.Pages;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace EventPoints.Web.Services
{
	public class EventsService
	{
		private Task InitialiseTask { get; set; }
		public HttpClient Client { get; set; }

		public EventsService(HttpClient httpClient)
		{
			Client = httpClient;
		}

		public ObservableCollection<EventDto>? Events { get; set; } = [];

		public async Task GetEventsAsync(CancellationToken cancellationToken)
		{
			try
			{
				var response = await Client.GetAsync("Events", cancellationToken);
				if (response.IsSuccessStatusCode)
				{
					var events = await response.Content.ReadFromJsonAsync<ObservableCollection<EventDto>>();
					if (events is null)
					{
						throw new HttpRequestException("Error fetching events: Response content was null.");
					}
					foreach (var eventDto in events)
					{
						if (eventDto.Teams != null)
						{
							foreach (var team in eventDto.Teams)
							{
								team.SetImage(Client.BaseAddress.ToString());
							}
						}
					}
					Events = events;
				}
				else
				{
					throw new HttpRequestException($"Error fetching events: {response.ReasonPhrase}");
				}
			}
			catch (OperationCanceledException ex)
			{
				Events = null;
			}

		}

		public async Task<EventDto?> GetEventByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			try
			{
				var response = await Client.GetAsync($"Events/{id}", cancellationToken);
				if (response.IsSuccessStatusCode)
				{
					return await response.Content.ReadFromJsonAsync<EventDto>();
				}
				else
				{
					throw new HttpRequestException($"Error fetching event with ID {id}: {response.ReasonPhrase}");
				}
			}
			catch (OperationCanceledException ex)
			{
				return null;
			}

		}

		public async Task IncrementTeamPoints(TeamDto team, int points, CancellationToken cancellationToken)
		{
			try
			{
				var content = JsonContent.Create(new { Points = points });
				var response = await Client.PutAsync($"Events/{team.EventId}/teams/{team.Id}/updatePoints", content, cancellationToken);
				if (!response.IsSuccessStatusCode)
				{
					throw new HttpRequestException($"Error updating team points: {response.ReasonPhrase}");
				}
			}
			catch (OperationCanceledException ex)
			{

			}

		}

		public async Task SetTeamPoints(TeamDto team, int points, CancellationToken cancellationToken)
		{
			try
			{
				var content = JsonContent.Create(new { Points = points });
				var response = await Client.PutAsync($"Events/{team.EventId}/teams/{team.Id}/setPoints", content, cancellationToken);
				if (!response.IsSuccessStatusCode)
				{
					throw new HttpRequestException($"Error setting team points: {response.ReasonPhrase}");
				}
			}
			catch (OperationCanceledException ex)
			{

			}

		}

		internal async Task DeleteTeam(Guid teamId, Guid eventId, CancellationToken cancellationToken)
		{
			try
			{
				var response = await Client.DeleteAsync($"Events/{eventId}/teams/{teamId}", cancellationToken);
				if (!response.IsSuccessStatusCode)
				{
					throw new HttpRequestException($"Error deleting team {teamId}: {response.ReasonPhrase}");
				}
			}
			catch (OperationCanceledException ex)
			{

			}

		}

		internal async Task DeleteEvent(Guid id, CancellationToken cancellationToken)
		{
			try
			{
				var response = await Client.DeleteAsync($"Events/{id}", cancellationToken);
				if (!response.IsSuccessStatusCode)
				{
					throw new HttpRequestException($"Error deleting event {id}: {response.ReasonPhrase}");
				}
			}
			catch (OperationCanceledException ex)
			{

			}

		}

		internal async Task<TeamDto?> GetTeamByIdAsync(Guid teamId, CancellationToken cancellationToken)
		{
			try
			{
				var response = await Client.GetAsync($"Events/teams/{teamId}", cancellationToken);
				if (response.IsSuccessStatusCode)
				{
					Team team = await response.Content.ReadFromJsonAsync<Team>();
					TeamDto dto = team.CreateDto(team);
					dto.SetImage(Client.BaseAddress.ToString());
					return dto;
				}
				else
				{
					throw new HttpRequestException($"Error fetching team with ID {teamId}: {response.ReasonPhrase}");
				}
			}
			catch (OperationCanceledException ex)
			{
				return null;
			}

		}

		internal async Task EditEventName(Guid eventId, string name, CancellationToken cancellationToken)
		{
			try
			{
				var content = JsonContent.Create(new { Name = name });
				var response = await Client.PutAsync($"Events/{eventId}/edit-event-name", content, cancellationToken);
				if (!response.IsSuccessStatusCode)
				{
					throw new HttpRequestException($"Error editing event name for event {eventId}: {response.ReasonPhrase}");
				}
			}
			catch (OperationCanceledException ex)
			{

			}

		}

		internal async Task EditTeamName(Guid teamId, string name, CancellationToken cancellationToken)
		{
			try
			{
				var content = JsonContent.Create(new { Name = name });
				var response = await Client.PutAsync($"Events/teams/{teamId}/edit-team-name", content, cancellationToken);
				if (!response.IsSuccessStatusCode)
				{
					throw new HttpRequestException($"Error editing event name for event {teamId}: {response.ReasonPhrase}");
				}
			}
			catch (OperationCanceledException ex)
			{

			}
		}

		internal async Task CreateEvent(string name, CancellationToken cancellationToken)
		{
			try
			{
				var content = JsonContent.Create(new { Name = name });
				var response = await Client.PostAsync($"Events", content, cancellationToken);
				if (!response.IsSuccessStatusCode)
				{
					throw new HttpRequestException($"Error creting event: {response.ReasonPhrase}");
				}
			}
			catch (OperationCanceledException ex)
			{

			}

		}

		internal async Task CreateTeam(string name, Guid eventId, CancellationToken cancellationToken)
		{
			try
			{
				var content = JsonContent.Create(new { Name = name, EventId = eventId });
				var response = await Client.PostAsync($"Events/teams", content, cancellationToken);
				if (!response.IsSuccessStatusCode)
				{
					throw new HttpRequestException($"Error creting event: {response.ReasonPhrase}");
				}
			}
			catch (OperationCanceledException ex)
			{

			}
		}
	}
}