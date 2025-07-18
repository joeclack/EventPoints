using EventPoints.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPoints.Domain.DTOs
{
	public class EventDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public ObservableCollection<TeamDto> Teams { get; set; } = [];
	}
}
