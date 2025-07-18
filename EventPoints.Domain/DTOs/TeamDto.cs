using EventPoints.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EventPoints.Domain.DTOs
{
	public class TeamDto
	{
		public Guid Id { get; set; }
		public Guid EventId { get; set; } 
		public string Name { get; set; } 
		public int Points { get; set; }

		public string? ImageUrl { get; set; } 

		public void SetImage()
		{
			ImageUrl = $"https://localhost:7174/api/Events/teams/{Id}/image";
		}

		public Team CreateTeam(TeamDto team)
		{
			return new Team(team.EventId, team.Name)
			{
				Id = team.Id,
				Points = team.Points,
				TeamImage = null,
				ImageMimeType = null
			};
		}
	}
}
