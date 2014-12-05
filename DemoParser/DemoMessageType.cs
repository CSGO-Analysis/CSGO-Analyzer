using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoParser_Core
{
	/// <summary>
	/// From hl2sdk-csgo/public/demofile/demoformat.h
	/// </summary>
	public enum DemoMessageType
	{
		/// <summary>
		/// it's a startup message, process as fast as possible
		/// </summary>
		Signon = 1,

		/// <summary>
		// it's a normal network packet that we stored off
		/// </summary>
		Packet,

		/// <summary>
		/// sync client clock to demo tick
		/// </summary>
		Synctick,

		/// <summary>
		/// Console Command
		/// </summary>
		ConsoleCommand,

		/// <summary>
		/// user input command
		/// </summary>
		UserCommand,

		/// <summary>
		///  network data tables
		/// </summary>
		DataTables,

		/// <summary>
		/// end of time.
		/// </summary>
		Stop,

		/// <summary>
		/// a blob of binary data understood by a callback function
		/// </summary>
		CustomData,

		/// <summary>
		/// network string tables
		/// </summary>
		StringTables,

		/// <summary>
		/// Last Command
		/// </summary>
		LastCommand = StringTables,

		/// <summary>
		/// First Command
		/// </summary>
		FirstCommand = Signon
	};
}
