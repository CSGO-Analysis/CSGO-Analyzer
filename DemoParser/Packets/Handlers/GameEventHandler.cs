using DemoParser_Core.Messages;
using DemoParser_Core.Events;
using DemoParser_Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoParser_Core.Packets.Handlers
{
    class GameEventHandler : IMessageParser
    {
		// contains all game events and their description (name and type of properties)
		private Dictionary<int, CSVCMsg_GameEventList.descriptor_t> gameEventdescriptors = new Dictionary<int, CSVCMsg_GameEventList.descriptor_t>();
		private Dictionary<string, object> data = new Dictionary<string, object>();

        public bool TryApplyMessage(ProtoBuf.IExtensible message, DemoParser parser)
        {
            if (message is CSVCMsg_GameEventList)
            {
				gameEventdescriptors.Clear();

				foreach (var d in ((CSVCMsg_GameEventList)message).descriptors)
				{
					gameEventdescriptors[d.eventid] = d;
				}

				return true;
            }
			
			if (gameEventdescriptors.Count == 0)
				return false;

			var rawEvent = message as CSVCMsg_GameEvent;
			if (rawEvent == null)
				return false;

			var eventDescriptor = gameEventdescriptors[rawEvent.eventid];

			switch (eventDescriptor.name) {
				case "round_announce_match_start":
					parser.EventsManager.RaiseMatchStarted();
					break;
				case "round_start":
					parser.EventsManager.RaiseRoundStart();
					break;
				case "round_mvp":
					MapData(eventDescriptor, rawEvent);

					if (parser.players.ContainsKey((int)data["userid"]))
					{
						RoundMvpEventArgs mvp = new RoundMvpEventArgs();
						mvp.Player = parser.players[(int)data["userid"]];
						mvp.Reason = (int)data["reason"];

						parser.EventsManager.RaiseRoundMvp(mvp);
					}
					break;
				case "round_end":
					if (parser.teams.Count > 2)
					{
						MapData(eventDescriptor, rawEvent);

						RoundEndedEventArgs arg = new RoundEndedEventArgs();
						arg.Team = parser.teams[(int)data["winner"]];
						arg.Reason = (int)data["reason"];

						if ((string)data["message"] != null)
							arg.Message = (string)data["message"];

						parser.EventsManager.RaiseRoundEnd(arg);
					}
					break;
				case "round_freeze_end":
					break;
				case "round_officially_ended":
					parser.EventsManager.RaiseRoundOfficiallyEnded();
					break;
				case "cs_win_panel_match":
					parser.EventsManager.RaiseMatchEnded();
					break;
				/* commit git 25/11 73ac4ba
				case "player_connect":
					data = MapData(eventDescriptor, rawEvent);

					PlayerInfo player = new PlayerInfo();
					player.UserID = (int)data["userid"];
					player.Name = (string)data["name"];
	
					//player.IsFakePlayer = (bool)data["bot"];
	
					int index = (int)data["index"];
	
					if (index < parser.RawPlayers.Count) {
						//Roy said:
						//"only replace existing player slot if the userID is different (very unlikely)"
	
						if (player.UserID != parser.RawPlayers[index].UserID) {
							parser.RawPlayers[index] = player;
						}
	
					} else {
						parser.RawPlayers.Add(player);
					}
	
	
					break;
				case "player_disconnect":
					data = MapData(eventDescriptor, rawEvent);
	
					var user = parser.RawPlayers.Single(a => a.UserID == (int)data["userid"]);
					parser.RawPlayers.Remove(user);
	
					parser.Players[(int)data["userid"]].Disconnected = true;
	
					break;
				 */
				case "player_chat":
					MapData(eventDescriptor, rawEvent);

					if (parser.players.ContainsKey((int)data["userid"]))
					{
						PlayerChatEventArgs chat = new PlayerChatEventArgs();
						chat.Player = parser.players[(int)data["userid"]];
						chat.TeamOnly = (bool)data["teamonly"];
						chat.Message = (string)data["text"];

						parser.EventsManager.RaisePlayerChat(chat);
					}
					break;
				case "weapon_fire":
					MapData (eventDescriptor, rawEvent);

					if (parser.players.ContainsKey ((int)data ["userid"]))
					{
						WeaponFiredEventArgs fire = new WeaponFiredEventArgs ();
						fire.Player = parser.players [(int)data ["userid"]];
						fire.Weapon = new Equipment((string)data ["weapon"]);

						parser.EventsManager.RaiseWeaponFired(fire);
					}
					break;
				case "player_death":
					MapData(eventDescriptor, rawEvent);

					if (parser.players.ContainsKey((int)data["userid"]) && parser.players.ContainsKey((int)data["attacker"]))
					{
						string weapon_itemid = (string)data.GetValueOrDefault<string, object>("weapon_itemid", string.Empty);

						PlayerKilledEventArgs kill = new PlayerKilledEventArgs();
						kill.Victim = parser.players[(int)data["userid"]];
						kill.Killer = parser.players[(int)data["attacker"]];
						kill.Headshot = (bool)data["headshot"];
						kill.Weapon = new Equipment((string)data["weapon"], weapon_itemid);
						kill.PenetratedObjects = (int)data["penetrated"];

						if (parser.players.ContainsKey((int)data["assister"]))
						{
							kill.Assist = parser.players[(int)data["assister"]];
						}

						parser.EventsManager.RaisePlayerKilled(kill);
					}
					break;

				#region Bomb
				case "bomb_planted": //When the bomb has been planted
					MapData(eventDescriptor, rawEvent);

					BombEventArgs bombEventArgs = new BombEventArgs();
					bombEventArgs.Player = parser.players[(int)data["userid"]];
					bombEventArgs.BombSite = (int)data["site"]; //entity index of the bombsite.

					parser.EventsManager.RaiseBombPlanted(bombEventArgs);
					break;
				case "bomb_defused": //When the bomb has been defused
					MapData(eventDescriptor, rawEvent);

					BombEventArgs bombEventArg = new BombEventArgs();
					bombEventArg.Player = parser.players[(int)data["userid"]];
					bombEventArg.BombSite = (int)data["site"]; //entity index of the bombsite.

					parser.EventsManager.RaiseBombDefused(bombEventArg);
					break;
				case "bomb_exploded": //When the bomb has exploded
					MapData(eventDescriptor, rawEvent);

					BombEventArgs bombEvent = new BombEventArgs();
					bombEvent.Player = parser.players[(int)data["userid"]];
					bombEvent.BombSite = (int)data["site"]; //entity index of the bombsite.

					parser.EventsManager.RaiseBombExploded(bombEvent);
					break;

				#endregion

				#region Nades
				/*case "player_blind":
					MapData (eventDescriptor, rawEvent);
					if (parser.Players.ContainsKey((int)data["userid"] - 2))
						blindPlayers.Add(parser.Players[(int)data["userid"] - 2]);
					break;*/
				case "flashbang_detonate":
					MapData(eventDescriptor, rawEvent);
					parser.EventsManager.RaiseFlashExploded(FillNadeEvent<FlashEventArgs>(data, parser));
					break;
				case "hegrenade_detonate":
					MapData(eventDescriptor, rawEvent);
					parser.EventsManager.RaiseGrenadeExploded(FillNadeEvent<GrenadeEventArgs>(data, parser));
					break;
				case "decoy_started":
					MapData(eventDescriptor, rawEvent);
					parser.EventsManager.RaiseDecoyStart(FillNadeEvent<DecoyEventArgs>(data, parser));
					break;
				case "decoy_detonate":
					MapData(eventDescriptor, rawEvent);
					parser.EventsManager.RaiseDecoyEnd(FillNadeEvent<DecoyEventArgs>(data, parser));
					break;
				case "smokegrenade_detonate":
					MapData(eventDescriptor, rawEvent);
					parser.EventsManager.RaiseSmokeStart(FillNadeEvent<SmokeEventArgs>(data, parser));
					break;
				case "smokegrenade_expired":
					MapData(eventDescriptor, rawEvent);
					parser.EventsManager.RaiseSmokeEnd(FillNadeEvent<SmokeEventArgs>(data, parser));
					break;
				case "inferno_startburn":
					MapData(eventDescriptor, rawEvent);
					parser.EventsManager.RaiseFireStart(FillNadeEvent<FireEventArgs>(data, parser));
					break;
				case "inferno_expire":
					MapData(eventDescriptor, rawEvent);
					parser.EventsManager.RaiseFireEnd(FillNadeEvent<FireEventArgs>(data, parser));
					break;
					#endregion
			}

			return true;
        }

		private T FillNadeEvent<T>(Dictionary<string, object> data, DemoParser parser) where T : NadeEventArgs, new()
		{
			var nade = new T();

			if (data.ContainsKey("userid") && parser.players.ContainsKey((int)data["userid"]))
				nade.ThrownBy = parser.players[(int)data["userid"]];
				
			Vector vec = new Vector ();
			vec.X = (float)data["x"];
			vec.Y = (float)data["y"];
			vec.Z = (float)data["z"];

			nade.Position = vec;

			return nade;
		}

		private void MapData(CSVCMsg_GameEventList.descriptor_t eventDescriptor, CSVCMsg_GameEvent rawEvent)
		{
			this.data.Clear();

			var i = 0;
			foreach (var key in eventDescriptor.keys) {
				this.data[key.name] = GetData(rawEvent.keys[i++]);
			}
		}

		private object GetData(CSVCMsg_GameEvent.key_t eventData)
		{
			switch (eventData.type) {
			case 1:
				return eventData.val_string;
			case 2:
				return eventData.val_float;
			case 3:
				return eventData.val_long;
			case 4:
				return eventData.val_short;
			case 5:
				return eventData.val_byte; 
			case 6:
				return eventData.val_bool;
			default:
				break;
			}

			return null;
		}

		public int Priority { get { return 0; } }
    }
}
