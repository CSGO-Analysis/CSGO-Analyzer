using DemoParser_Model.Models;

namespace DemoParser_Model.Services
{
	public interface IScoreBoardService
	{
		ScoreBoard GetScoreBoard(Game game); // OrderType : Team, KDR, Rating, ...
		ScoreBoardLine GetScoreBoardLine(Game game, Player player);
	}
}
