using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoParser_Model
{
	public class Round
	{
		public float Duration { get; set; }
		public Team Winner { get; set; }
		public int Reason { get; set; }
		public Player Mvp { get; set; }
		public List<Frag> Frags { get; set; }

		public Round()
		{
			this.Frags = new List<Frag>();
		}

		public Round(float duration, Team winner, int reason, Player mvp) : this()
		{
			this.Duration = duration;
			this.Winner = winner;
			this.Reason = reason;
			this.Mvp = mvp;
			this.Frags = new List<Frag>();
		}
	}
}
