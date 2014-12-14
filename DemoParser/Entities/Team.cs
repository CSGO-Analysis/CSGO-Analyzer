using DemoParser_Core.Packets;
using System;

namespace DemoParser_Core.Entities
{
	public class Team : IComparable
	{
		internal Entity Entity;

		public int Id { get { return Entity.ID; } }

		public int Num { get; internal set; }

		public string Name { get; internal set; }

		/// <summary>
		/// ISO Alpha 2 country flag
		/// e.g. US = United States
		/// http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2
		/// </summary>
		public string Flag { get; internal set; }

		public int Score { get; internal set; }
		public int ScoreFirstHalf { get; internal set; }
		public int ScoreSecondHalf { get; internal set; }
		public int ScoreOvertime { get; internal set; }

		public int[] player_array { get; internal set; }

		public TeamSide Side { get { return (TeamSide)Num; } }

		internal Team(Entity entity)
		{
			this.Entity = entity;

			if (entity.Properties.ContainsKey("m_iTeamNum"))
			{
				this.Num = (int)entity.Properties["m_iTeamNum"];

				if (entity.Properties.ContainsKey("m_szTeamname"))
				{
					this.Name = (string)entity.Properties.GetValueOrDefault("m_szClanTeamname", (string)entity.Properties["m_szTeamname"]);
					this.Flag = (string)entity.Properties.GetValueOrDefault("m_szTeamFlagImage", string.Empty);
					//this.player_array = (int[])entity.Properties.GetValueOrDefault>("\"player_array\"", new int[5]);
				}
			}
		}

		internal void Update(Entity entity)
		{
			if (entity.Properties.ContainsKey("m_iTeamNum"))
			{
				this.Num = (int)entity.Properties["m_iTeamNum"];

				if (entity.Properties.ContainsKey("m_szTeamname"))
				{
					this.Name = (string)entity.Properties.GetValueOrDefault("m_szClanTeamname", (string)entity.Properties["m_szTeamname"]);
					this.Flag = (string)entity.Properties.GetValueOrDefault("m_szTeamFlagImage", string.Empty);
					//this.player_array = (int[])entity.Properties.GetValueOrDefault("\"player_array\"", new int[5]);
				}
			}

			this.Score = (int)entity.Properties.GetValueOrDefault("m_scoreTotal", 0);
			this.ScoreFirstHalf = (int)entity.Properties.GetValueOrDefault("m_scoreFirstHalf", 0);
			this.ScoreSecondHalf = (int)entity.Properties.GetValueOrDefault("m_scoreSecondHalf", 0);
			this.ScoreOvertime = (int)entity.Properties.GetValueOrDefault("m_scoreOvertime", 0);
		}

		public override string ToString()
		{
			return Name;
		}

		public int CompareTo(object obj)
		{
			return Name.CompareTo(((Team)obj).Name);
		}

		public enum TeamSide
		{
			Spectator = 1,
			T = 2,
			CT = 3,
		}
	}
}
