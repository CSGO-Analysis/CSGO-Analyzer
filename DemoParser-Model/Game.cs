using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoParser_Model
{
	public class Game
	{
		public DateTime Date { get; set; }
		public string Map { get; set; }
		public float Duration { get; set; }
		public List<Round> Rounds { get; set; }

		public Game(DateTime date, string map, float duration)
		{
			this.Date = date;
			this.Duration = duration;
			this.Map = map;
		}
	}
}
