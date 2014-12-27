using System;

namespace DemoParser_Core.Events
{
	public class EventsManager
	{
		#region 
		public event EventHandler<HeaderParsedEventArgs> HeaderParsed;

		public event EventHandler<TickDoneEventArgs> TickDone;
		public event EventHandler<TeamParsedEventArgs> TeamParsed;
		public event EventHandler<TeamUpdateParsedEventArgs> TeamUpdateParsed;
		public event EventHandler<PlayerParsedEventArgs> PlayerParsed;
		public event EventHandler<PlayerUpdateParsedEventArgs> PlayerUpdateParsed;

		#region GameEvents
		public event EventHandler<MatchStartedEventArgs> MatchStarted;
		public event EventHandler<MatchEndedEventArgs> MatchEnded;
		public event EventHandler<RoundStartedEventArgs> RoundStart;
		public event EventHandler<RoundEndedEventArgs> RoundEnd;
		public event EventHandler<RoundMvpEventArgs> RoundMvp;
		public event EventHandler<RoundOfficiallyEndedEventArgs> RoundOfficiallyEnded;

		public event EventHandler<PlayerKilledEventArgs> PlayerKilled;
		public event EventHandler<PlayerChatEventArgs> PlayerChat;
		public event EventHandler<WeaponFiredEventArgs> WeaponFired;

		public event EventHandler<BombEventArgs> BombPlanted;
		public event EventHandler<BombEventArgs> BombDefused;
		public event EventHandler<BombEventArgs> BombExploded;

		public event EventHandler<SmokeEventArgs> SmokeNadeStarted;
		public event EventHandler<SmokeEventArgs> SmokeNadeEnded;

		public event EventHandler<DecoyEventArgs> DecoyNadeStarted;
		public event EventHandler<DecoyEventArgs> DecoyNadeEnded;

		public event EventHandler<FireEventArgs> FireNadeStarted;
		public event EventHandler<FireEventArgs> FireNadeEnded;

		public event EventHandler<FlashEventArgs> FlashNadeExploded;
		public event EventHandler<GrenadeEventArgs> ExplosiveNadeExploded;

		public event EventHandler<NadeEventArgs> NadeReachedTarget;
		#endregion
		#endregion

		#region EventCaller
		#region ParserEventCaller
		internal void RaiseTickDone()
		{
			if (TickDone != null)
				TickDone(this, null);
		}

		internal void RaiseHeaderParsed(HeaderParsedEventArgs arg)
		{
			if (HeaderParsed != null)
				HeaderParsed(this, arg);
		}

		internal void RaiseTeamParsed(TeamParsedEventArgs arg)
		{
			if (TeamParsed != null)
				TeamParsed(this, arg);
		}

		internal void RaisePlayerParsed(PlayerParsedEventArgs arg)
		{
			if (PlayerParsed != null)
				PlayerParsed(this, arg);
		}

		#endregion

		#region GameEventCaller
		internal void RaiseMatchStarted()
		{
			if (MatchStarted != null)
				MatchStarted(this, null);
		}

		internal void RaiseMatchEnded()
		{
			if (MatchEnded != null)
				MatchEnded(this, null);
		}

		internal void RaisePlayerChat(PlayerChatEventArgs chat)
		{
			if (PlayerChat != null)
				PlayerChat(this, chat);
		}

		internal void RaiseRoundStart()
		{
			if (RoundStart != null)
				RoundStart(this, null);

		}

		internal void RaiseRoundEnd(RoundEndedEventArgs e)
		{
			if (RoundEnd != null)
				RoundEnd(this, e);
		}

		internal void RaiseRoundMvp(RoundMvpEventArgs mvp)
		{
			if (RoundMvp != null)
				RoundMvp(this, mvp);
		}

		internal void RaiseRoundOfficiallyEnded()
		{
			if (RoundOfficiallyEnded != null)
				RoundOfficiallyEnded(this, null);
		}

		internal void RaisePlayerKilled(PlayerKilledEventArgs kill)
		{
			if (PlayerKilled != null)
				PlayerKilled(this, kill);
		}

		internal void RaiseWeaponFired(WeaponFiredEventArgs fire)
		{
			if (WeaponFired != null)
				WeaponFired(this, fire);
		}

		internal void RaiseBombPlanted(BombEventArgs args)
		{
			if (BombPlanted != null)
				BombPlanted(this, args);
		}

		internal void RaiseBombDefused(BombEventArgs args)
		{
			if (BombDefused != null)
				BombDefused(this, args);
		}

		internal void RaiseBombExploded(BombEventArgs args)
		{
			if (BombExploded != null)
				BombExploded(this, args);
		}

		internal void RaiseSmokeStart(SmokeEventArgs args)
		{
			if (SmokeNadeStarted != null)
				SmokeNadeStarted(this, args);

			if (NadeReachedTarget != null)
				NadeReachedTarget(this, args);
		}

		internal void RaiseSmokeEnd(SmokeEventArgs args)
		{
			if (SmokeNadeEnded != null)
				SmokeNadeEnded(this, args);

			if (NadeReachedTarget != null)
				NadeReachedTarget(this, args);
		}

		internal void RaiseDecoyStart(DecoyEventArgs args)
		{
			if (DecoyNadeStarted != null)
				DecoyNadeStarted(this, args);

			if (NadeReachedTarget != null)
				NadeReachedTarget(this, args);
		}

		internal void RaiseDecoyEnd(DecoyEventArgs args)
		{
			if (DecoyNadeEnded != null)
				DecoyNadeEnded(this, args);

			if (NadeReachedTarget != null)
				NadeReachedTarget(this, args);
		}

		internal void RaiseFireStart(FireEventArgs args)
		{
			if (FireNadeStarted != null)
				FireNadeStarted(this, args);

			if (NadeReachedTarget != null)
				NadeReachedTarget(this, args);
		}

		internal void RaiseFireEnd(FireEventArgs args)
		{
			if (FireNadeEnded != null)
				FireNadeEnded(this, args);

			if (NadeReachedTarget != null)
				NadeReachedTarget(this, args);
		}

		internal void RaiseFlashExploded(FlashEventArgs args)
		{
			if (FlashNadeExploded != null)
				FlashNadeExploded(this, args);

			if (NadeReachedTarget != null)
				NadeReachedTarget(this, args);
		}

		internal void RaiseGrenadeExploded(GrenadeEventArgs args)
		{
			if (ExplosiveNadeExploded != null)
				ExplosiveNadeExploded(this, args);

			if (NadeReachedTarget != null)
				NadeReachedTarget(this, args);
		}
		#endregion
		#endregion
	}
}
