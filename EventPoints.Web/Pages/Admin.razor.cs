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

		protected override async Task OnInitializedAsync()
		{
			try
			{
				await EventsService.GetEventsAsync();
				EventsList = EventsService.Events;
			}
			catch ( Exception ex )
			{

			}
		}

		public async Task RefreshEvents()
		{
			await EventsService.GetEventsAsync();
			EventsList = EventsService.Events;
		}

		public async Task CreateEvent()
		{
			string name = "New Event";
			await EventsService.CreateEvent(name);
			await RefreshEvents();
		}

		public void EditEvent(EventDto e)
		{
			NavigationManager.NavigateTo($"/admin/edit-event?EventId={e.Id}");
		}

		public async Task DeleteEvent(EventDto e)
		{
			await EventsService.DeleteEvent(e.Id);
			EventsList.Remove(e);
		}

	}
}