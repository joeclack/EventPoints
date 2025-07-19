using EventPoints.Domain.DTOs;
using EventPoints.Domain.Models;
using EventPoints.Web.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EventPoints.Web.Pages
{
	public partial class Admin
	{
		[Inject] private EventsService EventsService { get; set; } = default!;
		[Inject] private NavigationManager NavigationManager { get; set; } = default!;

		public ObservableCollection<EventDto>? EventsList { get; set; } = null;
		public bool IsLoading { get; set; }

		protected override async Task OnInitializedAsync()
		{
			try
			{
				await RefreshEvents();
			}
			catch (Exception ex)
			{

			}
		}

		public async Task RefreshEvents()
		{
			try
			{
				IsLoading = true;
				using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
				await EventsService.GetEventsAsync(cts.Token);
				EventsList = EventsService.Events;
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

		public async Task CreateEvent()
		{
			string name = "New Event";
			using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
			await EventsService.CreateEvent(name, cts.Token);
			await RefreshEvents();
		}

		public void EditEvent(EventDto e)
		{
			NavigationManager.NavigateTo($"/manage/edit-event?EventId={e.Id}");
		}

		public async Task DeleteEvent(EventDto e)
		{
			using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
			await EventsService.DeleteEvent(e.Id, cts.Token);
			EventsList.Remove(e);
		}

	}
}