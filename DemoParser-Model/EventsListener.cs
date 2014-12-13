using DemoParser_Core;
using DemoParser_Core.Events;
using DemoParser_Model.Models;
using DemoParser_Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoParser_Model
{
	public class EventsListener
	{
		private IScoreBoardService scoreBoardService = new ScoreBoardService();

		//private DemoParser demoParser;
		private EventsManager eventsManager;

		private Game game = new Game();
		public ScoreBoard scoreBoard = new ScoreBoard();

		public EventsListener(DemoParser demoParser)
		{
			this.eventsManager = demoParser.EventsManager;

			eventsManager.HeaderParsed += eventsManager_HeaderParsed;
			eventsManager.TeamParsed += eventsManager_TeamParsed;
			eventsManager.PlayerParsed += eventsManager_PlayerParsed;

			eventsManager.MatchStarted += eventsManager_MatchStarted;
			eventsManager.MatchEnded += eventsManager_MatchEnded;
			eventsManager.PlayerKilled += eventsManager_PlayerKilled;
			eventsManager.RoundStart += eventsManager_RoundStart;
			eventsManager.RoundMvp += eventsManager_RoundMvp;
			eventsManager.RoundEnd += eventsManager_RoundEnd;
		}

		void eventsManager_PlayerParsed(object sender, PlayerParsedEventArgs e)
		{
			if (e.Player.Team != null)
			{
				Team team = game.GetTeam(e.Player.Team.Id);
				Player player = new Player(e.Player.Name, e.Player.SteamID, team);

				team.Players.Add(player);
				game.AddPlayer(player);
			}
			else
			{
				Player player = new Player(e.Player.Name, e.Player.SteamID, null);
				game.AddPlayer(player);
			}
		}

		void eventsManager_TeamParsed(object sender, TeamParsedEventArgs e)
		{
			Team team = new Team(e.Team.Name, e.Team.Flag);
			team.Id = e.Team.Id;

			game.Teams.Add(team);
		}

		void eventsManager_HeaderParsed(object sender, DemoParser_Core.Events.HeaderParsedEventArgs e)
		{
			game.Map = e.Header.MapName;
			game.Duration = e.Header.PlaybackTime;
			game.Date = DateTime.Now;
			game.Rounds = new List<Round>();
		}

		void eventsManager_MatchStarted(object sender, MatchStartedEventArgs e)
		{
			this.game.IsStarted = true;
			game.Rounds.Add(new Round());
		}

		void eventsManager_MatchEnded(object sender, MatchEndedEventArgs e)
		{
			this.scoreBoard = scoreBoardService.GetScoreBoard(game);
			return;
		}

		void eventsManager_RoundStart(object sender, RoundStartedEventArgs e)
		{
			if (game.IsStarted)
			{
				game.Rounds.Add(new Round());
			}
		}

		void eventsManager_RoundEnd(object sender, RoundEndedEventArgs e)
		{
			if (game.IsStarted)
			{
				game.Rounds.Last().Winner = this.game.GetTeam(e.Team.Id);
				game.Rounds.Last().Reason = e.Reason;
				//game.Rounds.Last().Duration = e.
			}
		}

		void eventsManager_RoundMvp(object sender, RoundMvpEventArgs e)
		{
			if (game.IsStarted)
			{
				game.Rounds.Last().Mvp = this.game.GetPlayer(e.Player.SteamID);
			}
		}

		void eventsManager_PlayerKilled(object sender, PlayerKilledEventArgs e)
		{
			if (game.IsStarted)
			{
				Frag frag = new Frag();
				frag.Killer = this.game.GetPlayer(e.Killer.SteamID);
				frag.Victim = this.game.GetPlayer(e.Victim.SteamID);
				frag.Assist = (e.Assist != null) ? this.game.GetPlayer(e.Assist.SteamID) : null;
				frag.Headshot = e.Headshot;
				frag.Penetrated = (e.PenetratedObjects > 0) ? true : false;
				frag.Weapon = e.Weapon.OriginalString;

				game.Rounds.Last().Frags.Add(frag);

				this.game.GetPlayer(e.Killer.SteamID).Frags.Add(frag);

				this.game.Frags.Add(frag);
			}
		}
	}
}
