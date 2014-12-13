using DemoParser_Model.Models;
using System;
using System.Linq;

namespace DemoParser_Model.Services
{
	public class RatingService : IRatingService
	{
		public const double AverageKPR = 0.679; // (average kills per round)
		public const double AverageSPR = 0.317; // (average survived rounds per round)
		public const double AverageRMK = 1.277; // (average value calculated from rounds with multiple kills: (1K + 4*2K + 9*3K + 16*4K + 25*5K)/Rounds) 

		/// <summary>
		/// Compute rating of a player based on the rating formula by hltv.org
		/// </summary>
		/// <param name="game"></param>
		/// <param name="player"></param>
		/// <returns></returns>
		public double ComputeRating(int numberOfRounds, PlayerRatingData playerData)
		{
			int Rounds = numberOfRounds;

			double KillRating = playerData.Kills / (double)Rounds / (double)AverageKPR; // Kills/Rounds/AverageKPR
			double SurvivalRating = (Rounds - playerData.Deaths) / (double)Rounds / (double)AverageSPR; // (Rounds-Deaths)/Rounds/AverageSPR
			double RoundsWithMultipleKillsRating = (playerData.Kill1 + 4 * playerData.Kill2 + 9 * playerData.Kill3 + 16 * playerData.Kill4 + 25 * playerData.Kill5) / (double)Rounds / (double)AverageRMK; // (1K + 4*2K + 9*3K + 16*4K + 25*5K)/Rounds/AverageRMK 

			return Math.Round((KillRating + 0.7 * SurvivalRating + RoundsWithMultipleKillsRating) / 2.7, 2);
		}

		public double ComputeRating(Game game, Player player)
		{
			PlayerRatingData playerData = ComputeRatingData(game, player);

			return ComputeRating(game.Rounds.Count, playerData);
		}

		public PlayerRatingData ComputeRatingData(Game game, Player player)
		{
			PlayerRatingData data = new PlayerRatingData();

			int fragByRound;
			
			foreach (Round round in game.Rounds)
			{
				fragByRound = 0;

				foreach (Frag frag in round.Frags)
				{
					if (frag.Killer.SteamId == player.SteamId)
					{
						data.Kills++;
						fragByRound++;
					}

					if (frag.Victim.SteamId == player.SteamId)
					{
						data.Deaths++;
					}
				}

				switch (fragByRound)
				{
					case 1: data.Kill1++; break;
					case 2: data.Kill2++; break;
					case 3: data.Kill3++; break;
					case 4: data.Kill4++; break;
					case 5: data.Kill5++; break;
				}
			}

			return data;
		}
	}
}
