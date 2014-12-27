using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoParser_Model
{
	public class Game : Stringify
	{
		public DateTime Date { get; set; }
		public string Map { get; set; }
		public float Duration { get; set; }

		public List<Team> Teams { get; set; }
		public List<Player> Players { get; set; }
		public List<Frag> Frags { get; set; }
		public List<Round> Rounds { get; set; }

		public bool IsStarted { get; set; }

		public Game()
		{
			this.Teams = new List<Team>();
			this.Rounds = new List<Round>();
			this.Players = new List<Player>();
			this.Frags = new List<Frag>();

			this.IsStarted = false;
		}

		public Game(DateTime date, string map, float duration) : this()
		{
			this.Date = date;
			this.Duration = duration;
			this.Map = map;
		}

		public Team GetTeam(int teamId)
		{
			return Teams.Find(t => t.Id == teamId);
		}

		public Player GetPlayer(long steamId)
		{
			return Players.Find(p => p.SteamId == steamId);
		}

		public void AddPlayer(Player player)
		{
			Player existingPlayer = GetPlayer(player.SteamId);

			if (existingPlayer == null)
			{
				Players.Add(player);
			}
			else
			{
				existingPlayer.Team = player.Team;
			}
		}

		public void AddTeam(Team team)
		{
			Team existingTeam = GetTeam(team.Id);

			if (existingTeam == null)
				Teams.Add(team);
		}
	}
}
