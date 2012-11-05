using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HealthCheckMonitor
{
	class Program
	{
		static void Main(string[] args)
		{
			Process process = Process.GetProcessesByName("HangUIThread").FirstOrDefault();

			if (process != null)
			{
				while (true)
				{
					System.Threading.Thread.Sleep(3000);
					Console.WriteLine("Is responding: " + process.Responding);
				}
				
			}
		}
	}
}
