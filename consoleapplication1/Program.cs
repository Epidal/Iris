#region Includes

using System;

#endregion

namespace ConsoleApplication1
{
	class Program
	{
		static Iris.Communication blah = new Iris.Communication();

		static void Main(string[] args)
		{
			blah.Events.OnReceiveServerList += new Iris.EventHandler.ServerListHandler(Events_OnReceiveServerList);
			blah.Connect("37.187.91.120", 38170);

			while (!blah.IsConnected) { }

			Console.WriteLine(blah.DisconnectPlayers(1, 0));

			Console.ReadLine();

			//for (int i = 1; i < 7; i++)
				Console.WriteLine(blah.SetChannelType(1, 5, 96));
			
			Console.ReadLine();
			
			while(true)
				Console.WriteLine(blah.SendMessage(1, 5, "blah"));
		}

		static void Events_OnReceiveServerList(object sender, Iris.ServerListEventArgs e)
		{
			foreach (var s in e.ServerList)
			{
				Console.WriteLine("Server: {0}", s.ServerID);
				foreach (var c in s.Channels)
					Console.WriteLine("  Channel: {0}  Players: {1}/{2}  Type: {3}", c.ChannelID, c.Players, c.MaxPlayers, (uint)c.Type);
				Console.WriteLine();
			}

			//Console.WriteLine(blah.SendMessage(6, new byte[] {8, 30}, "ignore this", "test"));
		}
	}
}