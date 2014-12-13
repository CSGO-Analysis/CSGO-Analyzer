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

		public int Health { get; internal set; }
		public int Money { get; internal set; }
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

				if (entity.Properties.ContainsKey("m_vecOrigin[2]"))
					this.Position.Z = (float)entity.Properties.GetValueOrDefault("m_vecOrigin[2]", 0);

				if (entity.Properties.ContainsKey("m_iHealth"))
					this.Health = (int)entity.Properties["m_iHealth"];
				else
					this.Health = -1;

				this.Money = (int)entity.Properties.GetValueOrDefault<string, object>("m_iAccount", 0);

				this.Velocity = new Vector();
				this.Velocity.X = (float)entity.Properties.GetValueOrDefault<string, object>("m_vecVelocity[0]", 0f);
				this.Velocity.Y = (float)entity.Properties.GetValueOrDefault<string, object>("m_vecVelocity[1]", 0f);
				this.Velocity.Z = (float)entity.Properties.GetValueOrDefault<string, object>("m_vecVelocity[2]", 0f);

				if (entity.Properties.ContainsKey("m_angEyeAngles[1]"))
					this.ViewDirectionX = (float)entity.Properties["m_angEyeAngles[1]"];

				if (entity.Properties.ContainsKey("m_angEyeAngles[0]"))
					this.ViewDirectionY = (float)entity.Properties["m_angEyeAngles[0]"];

				if (this.IsAlive)
				{
					this.LastAlivePosition = this.Position;
				}
			}
		}
    }
}
