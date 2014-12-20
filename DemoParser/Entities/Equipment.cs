using System.Diagnostics;

namespace DemoParser_Core.Entities
{
	public class Equipment
	{
		public EquipmentElement Weapon { get; set; }
		public EquipmentClass Class
		{
			get
			{
				return (EquipmentClass)(((int)Weapon / 100) + 1);
			}
		}

		public string OriginalString { get; set; }

		public string SkinID { get; set; }

		internal Equipment()
		{

		}

		internal Equipment(string originalString)
		{
			OriginalString = originalString;

			MapEquipment(originalString);

		}

		internal Equipment(string originalString, string skin)
		{
			OriginalString = originalString;

			MapEquipment(originalString);

			SkinID = skin;
		}

		private void MapEquipment(string OriginalString)
		{
			EquipmentElement weapon = EquipmentElement.Unknown;

			if (OriginalString.Contains("knife") || OriginalString == "bayonet")
			{
				weapon = EquipmentElement.Knife;
			}

			if (weapon == EquipmentElement.Unknown)
			{
				switch (OriginalString)
				{
					case "ak47":
						weapon = EquipmentElement.AK47;
						break;
					case "aug":
						weapon = EquipmentElement.AUG;
						break;
					case "awp":
						weapon = EquipmentElement.AWP;
						break;
					case "bizon":
						weapon = EquipmentElement.Bizon;
						break;
					case "c4":
						weapon = EquipmentElement.Bomb;
						break;
					case "deagle":
						weapon = EquipmentElement.Deagle;
						break;
					case "decoy":
						weapon = EquipmentElement.Decoy;
						break;
					case "elite":
						weapon = EquipmentElement.DualBarettas;
						break;
					case "famas":
						weapon = EquipmentElement.Famas;
						break;
					case "fiveseven":
						weapon = EquipmentElement.FiveSeven;
						break;
					case "flashbang":
						weapon = EquipmentElement.Flash;
						break;
					case "g3sg1":
						weapon = EquipmentElement.G3SG1;
						break;
					case "galilar":
						weapon = EquipmentElement.Gallil;
						break;
					case "glock":
						weapon = EquipmentElement.Glock;
						break;
					case "hegrenade":
						weapon = EquipmentElement.HE;
						break;
					case "hkp2000":
						weapon = EquipmentElement.P2000;
						break;
					case "incgrenade":
						weapon = EquipmentElement.Incendiary;
						break;
					case "m249":
						weapon = EquipmentElement.M249;
						break;
					case "m4a1":
						weapon = EquipmentElement.M4A4;
						break;
					case "mac10":
						weapon = EquipmentElement.Mac10;
						break;
					case "mag7":
						weapon = EquipmentElement.Swag7;
						break;
					case "manifest":
						weapon = EquipmentElement.AK47;
						break;
					case "molotov":
						weapon = EquipmentElement.Molotov;
						break;
					case "mp7":
						weapon = EquipmentElement.MP7;
						break;
					case "mp9":
						weapon = EquipmentElement.MP9;
						break;
					case "negev":
						weapon = EquipmentElement.Negev;
						break;
					case "nova":
						weapon = EquipmentElement.Nova;
						break;
					case "p250":
						weapon = EquipmentElement.P250;
						break;
					case "p90":
						weapon = EquipmentElement.P90;
						break;
					case "sawedoff":
						weapon = EquipmentElement.SawedOff;
						break;
					case "scar20":
						weapon = EquipmentElement.Scar20;
						break;
					case "sg556":
						weapon = EquipmentElement.SG556;
						break;
					case "smokegrenade":
						weapon = EquipmentElement.Smoke;
						break;
					case "ssg08":
						weapon = EquipmentElement.Scout;
						break;
					case "taser":
						weapon = EquipmentElement.Zeus;
						break;
					case "tec9":
						weapon = EquipmentElement.Tec9;
						break;
					case "ump45":
						weapon = EquipmentElement.UMP;
						break;
					case "xm1014":
						weapon = EquipmentElement.XM1014;
						break;
					case "m4a1_silencer":
					case "m4a1_silencer_off":
						weapon = EquipmentElement.M4A1;
						break;
					case "cz75a":
						weapon = EquipmentElement.CZ;
						break;
					case "usp_silencer":
						weapon = EquipmentElement.USP;
						break;
					case "world":
						weapon = EquipmentElement.World;
						break;
					case "inferno":
						weapon = EquipmentElement.Incendiary;
						break;
					case "usp_silencer_off":
						weapon = EquipmentElement.USP;
						break;
					default:
						Trace.WriteLine("Unknown weapon. " + OriginalString, "Equipment.MapEquipment()");
						break;
				}
			}

			this.Weapon = weapon;
		}
	}

	public enum EquipmentElement
	{
		Unknown = 0,

		//Pistoles
		P2000 = 1,
		Glock = 2,
		P250 = 3,
		Deagle = 4,
		FiveSeven = 5,
		DualBarettas = 6,
		Tec9 = 7,
		CZ = 8,
		USP = 9,

		//SMGs
		MP7 = 101,
		MP9 = 102,
		Bizon = 103,
		Mac10 = 104,
		UMP = 105,
		P90 = 106,

		//Heavy
		SawedOff = 201,
		Nova = 202,
		Swag7 = 203,
		XM1014 = 204,
		M249 = 205,
		Negev = 206,

		//Rifle
		Gallil = 301,
		Famas = 302,
		AK47 = 303,
		M4A4 = 304,
		M4A1 = 305,
		Scout = 306,
		SG556 = 307,
		AUG = 308,
		AWP = 309,
		Scar20 = 310,
		G3SG1 = 311,

		//Equipment
		Zeus = 401,
		Kevlar = 402,
		Helmet = 403,
		Bomb = 404,
		Knife = 405,
		DefuseKit = 406,
		World = 407,

		//Grenades
		Decoy = 501,
		Molotov = 502,
		Incendiary = 503,
		Flash = 504,
		Smoke = 505,
		HE = 506
	}

	public enum EquipmentClass
	{
		Unknown = 0,
		Pistol = 1,
		SMG = 2,
		Heavy = 3,
		Rifle = 4,
		Equipment = 5,
		Grenade = 6,

	}
}
