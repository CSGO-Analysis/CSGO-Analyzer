using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoParser_Model
{
	public class Player
	{
		public string Name { get; set; }
		public long SteamId { get; set; }
		public Team Team { get; set; }
		public List<Frag> Frags { get; set; }

		public Player(string name, long steamId, Team team)
		{
			this.Name = name;
			this.SteamId = steamId;
			this.Team = team;
			this.Frags = new List<Frag>();
		}
	}
}
