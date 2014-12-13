using DemoParser_Core.Entities;
using System;

namespace DemoParser_Core.Events
{
	public abstract class GameEventArgs : EventArgs
	{ }

    public class HeaderParsedEventArgs : EventArgs
    {
        public DemoHeader Header { get; private set; }

        public HeaderParsedEventArgs(DemoHeader header)
        {
            this.Header = header;
        }
    }

    public class TickDoneEventArgs : EventArgs
    { }

	public class TeamParsedEventArgs : EventArgs
	{
		public Team Team { get; internal set; }

		public TeamParsedEventArgs(Team team)
		{
			this.Team = team;
		}
	}

	public class PlayerParsedEventArgs : EventArgs
	{
		public Player Player { get; internal set; }

		public PlayerParsedEventArgs(Player player)
		{
			this.Player = player;
		}
	}

    public class MatchStartedEventArgs : EventArgs
    { }

	public class MatchEndedEventArgs : EventArgs
	{ }

	public class PlayerChatEventArgs : EventArgs
	{
		public Player Player { get; internal set; }
		public bool TeamOnly { get; internal set; }
		public string Message { get; internal set; }
	}

	public class RoundStartedEventArgs : EventArgs
	{ }

	public class RoundEndedEventArgs : EventArgs
	{
		public Team Team { get; internal set; }
		public int Reason { get; internal set; }
		public string Message { get; internal set; }
	}

	public class RoundOfficiallyEndedEventArgs : EventArgs
	{ }

	public class RoundMvpEventArgs : EventArgs
	{
		public Player Player { get; internal set; }
		public int Reason { get; internal set; }
	}

	public class PlayerKilledEventArgs : EventArgs
	{
		public Player Victim { get; internal set; }
		public Player Killer { get; internal set; }
		public Player Assist { get; internal set; }
		public Equipment Weapon { get; internal set; }
		public bool Headshot { get; internal set; }
		public int PenetratedObjects { get; internal set; }
	}

	public class WeaponFiredEventArgs : EventArgs
	{
		public Equipment Weapon { get; internal set; }
		public Player Player { get; internal set; }
	}

	public class NadeEventArgs : EventArgs
	{
		public Vector Position { get; internal set; }
		public EquipmentElement NadeType { get; internal set; }
		public Player ThrownBy { get; internal set; }

		internal NadeEventArgs ()
		{
		
		}

		internal NadeEventArgs (EquipmentElement type)
		{
			this.NadeType = type;
		}
	}

	public class FireEventArgs : NadeEventArgs
	{
		public FireEventArgs () : base(EquipmentElement.Incendiary)
		{
			
		}
  	}
	public class SmokeEventArgs : NadeEventArgs
	{
		public SmokeEventArgs () : base(EquipmentElement.Smoke)
		{
			
		}
	}
	public class DecoyEventArgs : NadeEventArgs
	{
		public DecoyEventArgs () : base(EquipmentElement.Decoy)
		{
			
		}
	}
	public class FlashEventArgs : NadeEventArgs
	{
		public Player[] FlashedPlayers { get; internal set; }

		public FlashEventArgs () : base(EquipmentElement.Flash)
		{

		}
	}
	public class GrenadeEventArgs : NadeEventArgs
	{
		public GrenadeEventArgs () : base(EquipmentElement.HE)
		{
			
		}
	}
}

