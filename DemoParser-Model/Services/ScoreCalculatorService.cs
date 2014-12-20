using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoParser_Model.Services
{
	public class ScoreCalculatorService : IScoreCalculatorService
	{
		public int ComputeScore(Game game, Player player)
		{
			/*
			2 points for a bomb plant. (Terrorist Only)
			2 points if that bomb explodes. (Terrorist Only)
			2 points for a kill.
			1 point for an assist.
			5 points for defusing a bomb. (Counter-Terrorist Only)
			2 points for rescuing a hostage. (Counter-Terrorist Only)
			-1 point for killing a teammate.
			-1 point for committing suicide.
			*/
			return 0;
		}
	}
}
