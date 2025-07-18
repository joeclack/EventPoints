using EventPoints.Domain.DTOs;
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

		public byte[]? TeamImage { get; set; }
		public string? ImageMimeType { get; set; }

		public TeamDto CreateDto(Team team)
		{
			return new TeamDto
			{
				Id = team.Id,
				EventId = team.EventId,
				Name = team.Name,
				Points = team.Points,
				ImageUrl = $"https://localhost:7174/api/Events/teams/{team.Id}/image"
			};
		}
	}
}
