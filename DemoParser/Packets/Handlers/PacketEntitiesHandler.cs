﻿using DemoParser_Core.Streams.BitStream;
using DemoParser_Core.DataTables;
using DemoParser_Core.Entities;
using DemoParser_Core.Messages;
using System.Diagnostics;
using System.IO;
using DemoParser_Core.Events;

namespace DemoParser_Core.Packets.Handlers
{
	/// <summary>
	/// Manage lifetime of all entities, from creation to update and delete
	/// </summary>
    class PacketEntitiesHandler : IMessageParser
    {
		public int Priority { get { return 0; } }

        public bool TryApplyMessage(ProtoBuf.IExtensible message, DemoParser parser)
        {
			CSVCMsg_PacketEntities packetEntities = message as CSVCMsg_PacketEntities;
			if (packetEntities == null)
				return false;
			
			// TODO check
			// In official C++ source code, loop is from last to first entities in packetEntities -> impact ?
			using (IBitStream reader = BitStreamUtil.Create(packetEntities.entity_data)) {
				int currentEntity = -1;
				
				for (int i = 0; i < packetEntities.updated_entries; i++)
				{
					currentEntity += 1 + (int)reader.ReadUBitInt();
					
					if (!reader.ReadBit())
					{
						Entity entity;
						
						if (reader.ReadBit())
							// EnterPVS = add entity
							entity = EnterPVS(reader, currentEntity, parser);
						else
							// DeltaEnt = update entity
							entity = parser.entities[currentEntity];

						entity.ApplyUpdate(reader);

						UpdateModel(entity, parser);

						Debug.WriteLine("Entity #" + entity.ID + ": " + entity.ServerClass.Name);
					}
					else
					{
						// LeavePVS
						if (reader.ReadBit())
						{
							parser.entities.Remove(currentEntity);
						}
					}
				}
			}

			return true;
        }

        private Entity EnterPVS(IBitStream reader, int id, DemoParser parser)
        {
			int serverClassID = (int)reader.ReadInt(parser.DataTablesParser.ClassBits);
			reader.ReadInt(10); //Entity serial. 

			ServerClass entityClass = parser.DataTablesParser.ServerClasses[serverClassID];
			Entity newEntity = new Entity(id, entityClass);

			parser.entities[newEntity.ID] = newEntity;

			/* commit on trunk
			using (var ms = new MemoryStream(parser.instanceBaseline[serverClassID])) {
				ms.Position = 0;
				newEntity.ApplyUpdate(BitStreamUtil.Create(ms));
			}*/

            return newEntity;
        }

		/// <summary>
		/// Update Team and Player lists based on entity values
		/// </summary>
		/// <param name="entity"></param>
		private void UpdateModel(Entity entity, DemoParser parser)
		{
			// Team
			if (entity.ServerClass.DTName == "DT_CSTeam" && entity.Properties.Count > 0)
			{
				int teamNum = (int)entity.Properties["m_iTeamNum"];

				if (!parser.teams.ContainsKey(teamNum))
				{
					Team team = new Team(entity);
					parser.teams.Add(teamNum, team);
				}
				parser.teams[teamNum].Update(entity);
				parser.EventsManager.RaiseTeamParsed(new TeamParsedEventArgs(parser.teams[teamNum]));
			}
			// Player
			else if (entity.ServerClass.DTName == "DT_CSPlayer")
			{
				// TODO WARN Player should already exists because created when stringtables are received
				// TODO is this really working? O_o
				int index = entity.ID - 1;

				if (!parser.entities.ContainsKey(index) || index >= parser.players.Count)
					return;

				Player p = parser.Players[index];
				p.Update(entity);
				parser.EventsManager.RaisePlayerParsed(new PlayerParsedEventArgs(p));
				
				if (p.Team == null)
				{
					if (entity.Properties.ContainsKey("m_iTeamNum") && parser.teams.ContainsKey((int)entity.Properties["m_iTeamNum"]))
						p.Team = parser.teams[(int)entity.Properties["m_iTeamNum"]];
				}
			}
		}
    }
}
