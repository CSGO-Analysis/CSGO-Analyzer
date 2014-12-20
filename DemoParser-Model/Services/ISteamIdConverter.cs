using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoParser_Model.Services
{
	public interface ISteamIdConverter
	{
		long ConvertSteamId(String steamId);
		String ConvertSteamId(long steamId);
	}
}
