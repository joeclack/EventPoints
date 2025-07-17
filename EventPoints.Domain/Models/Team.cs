using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPoints.Domain.Models
{
	public class Team(Guid eventId, string name)
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public Guid EventId { get; set; } = eventId;
		public string Name { get; set; } = name;
		public int Points { get; set; } = 0;
	}
}
