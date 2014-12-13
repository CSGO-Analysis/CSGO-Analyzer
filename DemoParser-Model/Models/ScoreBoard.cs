using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoParser_Model.Models
{
	public class ScoreBoard
	{
		private List<ScoreBoardLine> lines;

		public ScoreBoard()
		{
			this.lines = new List<ScoreBoardLine>();
		}

		public void AddLine(ScoreBoardLine line)
		{
			this.lines.Add(line);
		}

		public IReadOnlyList<ScoreBoardLine> GetLines()
		{
			return lines;
		}
	}
}
