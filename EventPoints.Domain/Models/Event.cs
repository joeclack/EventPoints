using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPoints.Domain.Models
{
	public class Event
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public List<Team> Teams { get; set; } = [];
		public Event(string name)
		{
			Id = Guid.NewGuid();
			Name = name;
		}
	}
}
