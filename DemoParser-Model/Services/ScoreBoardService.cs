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
			line.TeamName = (player.Team != null) ? player.Team.Name : string.Empty;

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

				if (round.Mvp.SteamId == player.SteamId)
				{
					line.Mvp++;
				}
			}

			PlayerRatingData data = ratingService.ComputeRatingData(game, player);
			line._1K = data._1K;
			line._2K = data._2K;
			line._3K = data._3K;
			line._4K = data._4K;
			line._5K = data._5K;

			line.Rating = ratingService.ComputeRating(game.Rounds.Count, data);

			return line;
		}
	}
}
