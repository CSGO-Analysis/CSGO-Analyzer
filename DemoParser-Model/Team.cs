using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoParser_Model
{
	public class Team
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public string Flag { get; set; }
		public int Score { get; set; }
		public int[] Scores { get; set; }
		public HashSet<Player> Players { get; set; }

		public Team()
		{
			this.Players = new HashSet<Player>();
		}

		public Team(string name, string flag) : this()
		{
			this.Name = name;
			this.Flag = flag;
			this.Score = 0;
		}

		public void AddPlayer(Player player)
		{
			if (!Players.Contains(player))
				Players.Add(player);
		}
	}
}
