using Common;
using System;
using System.IO;

namespace DemoParser_Core
{
	public class DemoHeader : Stringify
	{
		const int MAX_OSPATH = 260;

		public string Filestamp { get; private set; }		// Should be HL2DEMO
		public int DemoProtocol { get; private set; }		// Should be DEMO_PROTOCOL
		public int NetworkProtocol { get; private set; }	// Should be PROTOCOL_VERSION
		public string ServerName { get; private set; }		// Name of server
		public string ClientName { get; private set; }		// Name of client who recorded the game
		public string MapName { get; private set; }			// Name of map
		public string GameDirectory { get; private set; }	// Name of game directory (com_gamedir)
		public float PlaybackTime { get; private set; }		// Time of track (in seconds)
		public int PlaybackTicks { get; private set; }		// # of ticks in track
		public int PlaybackFrames { get; private set; }		// # of frames in track
		public int SignonLength { get; private set; }		// length of sigondata in bytes

		public float TickRate
		{
			get { return this.PlaybackTicks / this.PlaybackTime; }
		}

		public float TickTime
		{
			get { return this.PlaybackTime / this.PlaybackTicks; }
		}

		public double FrameRate
		{
			get { return Math.Round((double)this.PlaybackFrames / this.PlaybackTime); }
		}

		public float FrameTime
		{
			get { return this.PlaybackTime / this.PlaybackFrames; }
		}

		public static DemoHeader ParseFrom(BinaryReader reader)
		{
			return new DemoHeader()
			{
				Filestamp = reader.ReadCString(8),
				DemoProtocol = reader.ReadInt32(),
				NetworkProtocol = reader.ReadInt32(),
				ServerName = reader.ReadCString(MAX_OSPATH),
				ClientName = reader.ReadCString(MAX_OSPATH),
				MapName = reader.ReadCString(MAX_OSPATH),
				GameDirectory = reader.ReadCString(MAX_OSPATH),
				PlaybackTime = reader.ReadSingle(),
				PlaybackTicks = reader.ReadInt32(),
				PlaybackFrames = reader.ReadInt32(),
				SignonLength = reader.ReadInt32(),
			};
		}
	}
}
