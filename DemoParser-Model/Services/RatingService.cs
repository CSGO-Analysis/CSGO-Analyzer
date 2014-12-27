using DemoParser_Model.Models;
using System;
using System.Linq;

namespace DemoParser_Model.Services
{
	public class RatingService : IRatingService
	{
		public const double AVERAGE_KPR = 0.679; // (average kills per round)
		public const double AVERAGE_SPR = 0.317; // (average survived rounds per round)
		public const double AVERAGE_RMK = 1.277; // (average value calculated from rounds with multiple kills: (1K + 4*2K + 9*3K + 16*4K + 25*5K)/Rounds) 

		public enum RatingMethod { HLTV_ORG, CSGO, BuRner };

		public double ComputeRating(int numberOfRounds, PlayerRatingData playerData, RatingMethod method)
		{
			switch (method)
			{
				case RatingMethod.HLTV_ORG: return ComputeRating(numberOfRounds, playerData);
				case RatingMethod.CSGO: throw new NotImplementedException(); // TODO (score in csgo scoreboard)
				case RatingMethod.BuRner: throw new NotImplementedException(); //TODO (assists, clutch)
				default: throw new ArgumentException();
			}
		}

		/// <summary>
		/// Compute rating of a player based on the rating formula by hltv.org
		/// </summary>
		/// <param name="game"></param>
		/// <param name="player"></param>
		/// <returns></returns>
		public double ComputeRating(int numberOfRounds, PlayerRatingData playerData)
		{
			int Rounds = numberOfRounds;

			double KillRating = playerData.Kills / (double)Rounds / (double)AVERAGE_KPR; // Kills/Rounds/AverageKPR
			double SurvivalRating = (Rounds - playerData.Deaths) / (double)Rounds / (double)AVERAGE_SPR; // (Rounds-Deaths)/Rounds/AverageSPR
			double RoundsWithMultipleKillsRating = (playerData._1K + 4 * playerData._2K + 9 * playerData._3K + 16 * playerData._4K + 25 * playerData._5K) / (double)Rounds / (double)AVERAGE_RMK; // (1K + 4*2K + 9*3K + 16*4K + 25*5K)/Rounds/AverageRMK 

			return Math.Round((KillRating + 0.7 * SurvivalRating + RoundsWithMultipleKillsRating) / 2.7, 3);
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
					case 1: data._1K++; break;
					case 2: data._2K++; break;
					case 3: data._3K++; break;
					case 4: data._4K++; break;
					case 5: data._5K++; break;
				}
			}

			return data;
		}
	}
}
