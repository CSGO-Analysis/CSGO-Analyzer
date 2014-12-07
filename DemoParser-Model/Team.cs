using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoParser_Model
{
	public class Team
	{
		public string Name { get; set; }
		public string Flag { get; set; }
		public int Score { get; set; }
		public int[] Scores { get; set; }
		public HashSet<Player> Players { get; set; }

		public Team(string name, string flag)
		{
			this.Name = name;
			this.Flag = flag;
			this.Score = 0;
			this.Players = new HashSet<Player>();
		}
	}
}
