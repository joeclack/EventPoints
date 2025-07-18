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

		public ObservableCollection<EventDto> Events { get; set; } = [];

		public async Task GetEventsAsync()
		{
			var response = await Client.GetAsync("Events");
			if (response.IsSuccessStatusCode)
			{
				var events = await response.Content.ReadFromJsonAsync<ObservableCollection<EventDto>>();
				if ( events is null )
				{
					throw new HttpRequestException("Error fetching events: Response content was null.");
				}
				foreach ( var eventDto in events )
				{
					if ( eventDto.Teams != null )
					{
						foreach ( var team in eventDto.Teams )
						{
							team.SetImage();
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

		public async Task<EventDto> GetEventByIdAsync(Guid id)
		{
			var response = await Client.GetAsync($"Events/{id}");
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<EventDto>();
			}
			else
			{
				throw new HttpRequestException($"Error fetching event with ID {id}: {response.ReasonPhrase}");
			}
		}

		public async Task IncrementTeamPoints(TeamDto team, int points)
		{
			var content = JsonContent.Create(new { Points = points });
			var response = await Client.PutAsync($"Events/{team.EventId}/teams/{team.Id}/updatePoints", content);
			if ( !response.IsSuccessStatusCode )
			{
				throw new HttpRequestException($"Error updating team points: {response.ReasonPhrase}");
			}
		}

		public async Task SetTeamPoints(TeamDto team, int points)
		{
			var content = JsonContent.Create(new { Points = points });
			var response = await Client.PutAsync($"Events/{team.EventId}/teams/{team.Id}/setPoints", content);
			if ( !response.IsSuccessStatusCode )
			{
				throw new HttpRequestException($"Error setting team points: {response.ReasonPhrase}");
			}
		}

		internal async Task DeleteTeam(Guid teamId, Guid eventId)
		{
			var response = await Client.DeleteAsync($"Events/{eventId}/teams/{teamId}");
			if ( !response.IsSuccessStatusCode )
			{
				throw new HttpRequestException($"Error deleting team {teamId}: {response.ReasonPhrase}");
			}
		}

		internal async Task DeleteEvent(Guid id)
		{
			var response = await Client.DeleteAsync($"Events/{id}");
			if ( !response.IsSuccessStatusCode )
			{
				throw new HttpRequestException($"Error deleting event {id}: {response.ReasonPhrase}");
			}
		}

		internal async Task<TeamDto> GetTeamByIdAsync(Guid teamId)
		{
			var response = await Client.GetAsync($"Events/teams/{teamId}");
			if (response.IsSuccessStatusCode)
			{
				Team team = await response.Content.ReadFromJsonAsync<Team>();
				TeamDto dto = team.CreateDto(team);
				return dto;
			}
			else
			{
				throw new HttpRequestException($"Error fetching team with ID {teamId}: {response.ReasonPhrase}");
			}
		}

		internal async Task EditEventName(Guid eventId, string name)
		{
			var content = JsonContent.Create(new { Name = name });
			var response = await Client.PutAsync($"Events/{eventId}/edit-event-name", content);
			if ( !response.IsSuccessStatusCode )
			{
				throw new HttpRequestException($"Error editing event name for event {eventId}: {response.ReasonPhrase}");
			}
		}

		internal async Task EditTeamName(Guid teamId, string name)
		{
			var content = JsonContent.Create(new { Name = name });
			var response = await Client.PutAsync($"Events/teams/{teamId}/edit-team-name", content);
			if ( !response.IsSuccessStatusCode )
			{
				throw new HttpRequestException($"Error editing event name for event {teamId}: {response.ReasonPhrase}");
			}
		}
	}
}