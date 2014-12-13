using DemoParser_Model.Models;

namespace DemoParser_Model.Services
{
	public class ScoreBoardService : IScoreBoardService
	{
		private IRatingService ratingService = new RatingService();

		public ScoreBoard GetScoreBoard(Game game) // OrderType : Team, KDR, Rating, ...
		{
			ScoreBoard scoreBoard = new ScoreBoard();

			foreach (Player player in game.Players)
			{
				if (player.Frags.Count == 0) //TODO change to IsSpectator
					continue;

				scoreBoard.AddLine(GetScoreBoardLine(game, player));
			}

			return scoreBoard;
		}

		public ScoreBoardLine GetScoreBoardLine(Game game, Player player)
		{
			ScoreBoardLine line = new ScoreBoardLine();

			line.PlayerName = player.Name;

			foreach (Round round in game.Rounds)
			{
				foreach (Frag frag in round.Frags)
				{
					if (frag.Killer.SteamId == player.SteamId)
					{
						line.Kills++;

						if (frag.Headshot)
						{
							line.Headshot++;
						}
					}

					if (frag.Assist != null && frag.Assist.SteamId == player.SteamId)
					{
						line.Assist++;
					}

					if (frag.Victim.SteamId == player.SteamId)
					{
						line.Deaths++;
					}
				}
			}

			line.Rating = ratingService.ComputeRating(game, player);

			return line;
		}
	}
}
