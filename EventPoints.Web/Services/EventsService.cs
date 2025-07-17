using EventPoints.Domain.Models;
using System.Net.Http.Json;

namespace EventPoints.Web.Services
{
	public class EventsService
	{
		public HttpClient Client { get; set; } 
		public EventsService(HttpClient httpClient)
		{
			Client = httpClient;
		}

		public async Task<List<Event>> GetEventsAsync()
		{
			var response = await Client.GetAsync("Events");
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<Event>>();
			}
			else
			{
				throw new HttpRequestException($"Error fetching events: {response.ReasonPhrase}");
			}
		}

		public async Task<Event> GetEventAsync(Guid id)
		{
			var response = await Client.GetAsync($"Events/{id}");
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<Event>();
			}
			else
			{
				throw new HttpRequestException($"Error fetching event with ID {id}: {response.ReasonPhrase}");
			}
		}

		public async Task UpdateTeamPoints(Guid eventId, Guid teamId, int points)
		{
			var content = JsonContent.Create(new { Points = points });
			var response = await Client.PutAsync($"Events/{eventId}/teams/{teamId}/updatePoints", content);
			if ( !response.IsSuccessStatusCode )
			{
				throw new HttpRequestException($"Error updating team points: {response.ReasonPhrase}");
			}
		}

		public async Task SetTeamPoints(Guid eventId, Guid teamId, int points)
		{
			var content = JsonContent.Create(new { Points = points });
			var response = await Client.PutAsync($"Events/{eventId}/teams/{teamId}/setPoints", content);
			if ( !response.IsSuccessStatusCode )
			{
				throw new HttpRequestException($"Error setting team points: {response.ReasonPhrase}");
			}
		}
	}
}