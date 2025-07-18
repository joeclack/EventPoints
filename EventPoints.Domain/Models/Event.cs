using EventPoints.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPoints.Domain.Models
{
	public class Event
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public ObservableCollection<Team> Teams { get; set; } = [];
		public Event(string name)
		{
			Id = Guid.NewGuid();
			Name = name;
		}

		public EventDto CreateDto(Event @event)
		{
			return new EventDto
			{
				Id = @event.Id,
				Name = @event.Name,
				Teams = (ObservableCollection<TeamDto>)@event.Teams.Select(t => t.CreateDto(t))
			};
		}
	}
}
