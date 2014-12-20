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
			DateTime begin;
			DateTime end;
			
			Stream inputStream = new FileStream(args[0], FileMode.Open);

			Console.WriteLine("Parsing...");

			begin = DateTime.Now;

			DemoParser parser = new DemoParser(inputStream);
			parser.ParseDemo(true);

			end = DateTime.Now;

			Console.WriteLine(String.Format("Started: {0}", begin.ToString("HH:mm:ss.ffffff")));
			Console.WriteLine(String.Format("Finished: {0}", end.ToString("HH:mm:ss.ffffff")));
			Console.WriteLine(String.Format("\nTotal: {0}", (end - begin)));

			Console.ReadKey();
		}
	}
}
