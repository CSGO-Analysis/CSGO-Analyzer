using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoParser_Model
{
	public class Frag
	{
		public Player Killer { get; set; }
		public Player Victim { get; set; }
		public Player Assist { get; set; }
		public bool Headshot { get; set; }
		public bool Penetrated { get; set; }
		public string Weapon { get; set; }

		public Frag() { }

		public Frag(Player killer, Player victim, bool headshot, bool penetrated, string weapon)
		{
			this.Killer = killer;
			this.Victim = victim;
			this.Headshot = headshot;
			this.Penetrated = penetrated;
			this.Weapon = weapon;
		}
	}
}
