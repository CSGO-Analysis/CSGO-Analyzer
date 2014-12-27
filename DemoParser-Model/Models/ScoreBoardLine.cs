using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoParser_Model.Models
{
	public class ScoreBoardLine
	{
		public string TeamName { get; set; }
		public string PlayerName { get; set; }
		public int Kills { get; set; }
		public int Assist { get; set; }
		public int Deaths { get; set; }
		//public int Score { get { return (2 * Kills + Assist); } } TODO compute CSGO Score (bomb plant/defuse)
		public int Mvp { get; set; }
		public int Headshot { get; set; }
		public double HSR { get { return Math.Round((double)Headshot / Kills, 2); } }
		public double KDR { get { return Math.Round((double)Kills / Deaths, 2); } }
		public int KDdiff { get { return Kills - Deaths; } }
		public int _1K { get; set; }
		public int _2K { get; set; }
		public int _3K { get; set; }
		public int _4K { get; set; }
		public int _5K { get; set; }
		//public int _1vX { get; set; }
		public double Rating { get; set; }
	}
}
