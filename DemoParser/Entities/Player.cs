using DemoParser_Core.Packets;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DemoParser_Core.Entities
{
	public class Player
    {
		internal Entity Entity;

		public string Name { get; internal set; }
		public int UserID { get; internal set; }
		public long SteamID { get; internal set; }
		public Team Team { get; internal set; }

		public int Money { get; internal set; }
		public int Health { get; internal set; }
		public int Armor { get; internal set; }
		public bool HasHelmet { get; internal set; }
		public bool HasDefuseKit { get; internal set; }
		
		public Vector Position { get; internal set; }
		public Vector LastAlivePosition { get; internal set; }
		public Vector Velocity { get; internal set; }

		public float ViewDirectionX { get; internal set; }
		public float ViewDirectionY { get; internal set; }

		public bool IsAlive
		{
			get { return Health > 0; }
		}

		internal Player(Entity entity)
		{
			this.Entity = entity;
		}

		internal Player(PlayerInfo info)
		{
			Name = info.Name;
			UserID = info.UserID;
			SteamID = info.XUID;
		}

		internal void Update(PlayerInfo info)
		{
			Name = info.Name;
		}

		internal void Update(Entity entity)
		{
			this.Entity = entity;

			if (entity.Properties.ContainsKey("m_vecOrigin"))
			{
				this.Position = (Vector)entity.Properties["m_vecOrigin"];
				this.Position.Z = (float)entity.Properties.GetValueOrDefault("m_vecOrigin[2]", 0f);
				
				this.Health = (int)entity.Properties.GetValueOrDefault("m_iHealth", -1);
				this.Armor = (int)entity.Properties.GetValueOrDefault("m_ArmorValue", -1);
				this.HasDefuseKit = ((int)entity.Properties.GetValueOrDefault("m_bHasDefuser", 0)) == 1;
				this.HasHelmet = ((int)entity.Properties.GetValueOrDefault("m_bHasHelmet", 0)) == 1;

				this.Money = (int)entity.Properties.GetValueOrDefault("m_iAccount", 0);

				this.Velocity = new Vector();
				this.Velocity.X = (float)entity.Properties.GetValueOrDefault("m_vecVelocity[0]", 0f);
				this.Velocity.Y = (float)entity.Properties.GetValueOrDefault("m_vecVelocity[1]", 0f);
				this.Velocity.Z = (float)entity.Properties.GetValueOrDefault("m_vecVelocity[2]", 0f);

				this.ViewDirectionX = (float)entity.Properties.GetValueOrDefault("m_angEyeAngles[1]", 0f);
				this.ViewDirectionY = (float)entity.Properties.GetValueOrDefault("m_angEyeAngles[0]", 0f);

				if (this.IsAlive)
				{
					this.LastAlivePosition = this.Position;
				}
			}
		}
    }
}
