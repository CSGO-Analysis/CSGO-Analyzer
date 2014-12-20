using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoParser_Model.Services
{
	public interface IScoreCalculatorService
	{
		int ComputeScore(Game game, Player player);
	}
}
