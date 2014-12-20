using DemoParser_Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoParser_Console
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(DateTime.Now);
			Stream inputStream = new FileStream(args[0], FileMode.Open);
			DemoParser parser = new DemoParser(inputStream);
			parser.ParseDemo(true);
			Console.WriteLine(DateTime.Now);
			Console.ReadKey();
		}
	}
}
