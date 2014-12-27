using DemoParser_Core;
using DemoParser_Core.Events;
using System;
using System.Linq;

namespace DemoParser_Model.Observers
{
	public class GameObserver : Observer
	{
		protected Game game;

		public GameObserver(DemoParser demoParser) : base(demoParser)
		{
			this.game = new Game();

			var eventsManager = parser.EventsManager;

			eventsManager.HeaderParsed += eventsManager_HeaderParsed;
			eventsManager.TeamParsed += eventsManager_TeamParsed;
			eventsManager.PlayerParsed += eventsManager_PlayerParsed;

			eventsManager.MatchStarted += eventsManager_MatchStarted;
			eventsManager.MatchEnded += eventsManager_MatchEnded;
			eventsManager.PlayerKilled += eventsManager_PlayerKilled;
			eventsManager.RoundStart += eventsManager_RoundStart;
			eventsManager.RoundMvp += eventsManager_RoundMvp;
			eventsManager.RoundEnd += eventsManager_RoundEnd;

			eventsManager.BombPlanted += eventsManager_BombPlanted;
			eventsManager.BombDefused += eventsManager_BombDefused;
			eventsManager.BombExploded += eventsManager_BombExploded;
		}

		protected virtual void eventsManager_BombExploded(object sender, BombEventArgs e)
		{
			
		}

		protected virtual void eventsManager_BombDefused(object sender, BombEventArgs e)
		{
			
		}

		protected virtual void eventsManager_BombPlanted(object sender, BombEventArgs e)
		{
			
		}

		protected virtual void eventsManager_PlayerParsed(object sender, PlayerParsedEventArgs e)
		{
			if (e.Player.Team != null)
			{
				Team team = game.GetTeam(e.Player.Team.Id);
				Player player = new Player(e.Player.Name, e.Player.SteamID, team);

				team.AddPlayer(player);
				game.AddPlayer(player);
			}
			else
			{
				Player player = new Player(e.Player.Name, e.Player.SteamID, null);
				game.AddPlayer(player);
			}
		}

		protected virtual void eventsManager_TeamParsed(object sender, TeamParsedEventArgs e)
		{
			Team team = new Team(e.Team.Name, e.Team.Flag);
			team.Id = e.Team.Id;

			game.AddTeam(team);
		}

		protected virtual void eventsManager_HeaderParsed(object sender, DemoParser_Core.Events.HeaderParsedEventArgs e)
		{
			game.Map = e.Header.MapName;
			game.Duration = e.Header.PlaybackTime;
			game.Date = DateTime.Now;
		}

		protected virtual void eventsManager_MatchStarted(object sender, MatchStartedEventArgs e)
		{
			game.IsStarted = true;

			game.Rounds.Clear();
			game.Rounds.Add(new Round());
		}

		protected virtual void eventsManager_MatchEnded(object sender, MatchEndedEventArgs e)
		{
			
		}

		protected virtual void eventsManager_RoundStart(object sender, RoundStartedEventArgs e)
		{
			if (game.IsStarted)
			{
				game.Rounds.Add(new Round());
			}
		}

		protected virtual void eventsManager_RoundEnd(object sender, RoundEndedEventArgs e)
		{
			if (game.IsStarted)
			{
				game.Rounds.Last().Winner = game.GetTeam(e.Team.Id);
				game.Rounds.Last().Reason = e.Reason;
				//game.Rounds.Last().Duration = e.
			}
		}

		protected virtual void eventsManager_RoundMvp(object sender, RoundMvpEventArgs e)
		{
			if (game.IsStarted)
			{
				game.Rounds.Last().Mvp = game.GetPlayer(e.Player.SteamID);
				game.Rounds.Last().Reason = e.Reason;
			}
		}

		protected virtual void eventsManager_PlayerKilled(object sender, PlayerKilledEventArgs e)
		{
			if (game.IsStarted)
			{
				Frag frag = new Frag();
				frag.Killer = game.GetPlayer(e.Killer.SteamID);
				frag.Victim = game.GetPlayer(e.Victim.SteamID);
				frag.Assist = (e.Assist != null) ? game.GetPlayer(e.Assist.SteamID) : null;
				frag.Headshot = e.Headshot;
				frag.Penetrated = (e.PenetratedObjects > 0) ? true : false;
				frag.Weapon = e.Weapon.OriginalString;

				game.Rounds.Last().Frags.Add(frag);
				game.GetPlayer(e.Killer.SteamID).Frags.Add(frag);
				game.Frags.Add(frag);
			}
		}
	}
}
