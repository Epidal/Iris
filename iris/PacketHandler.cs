/*
	Copyright © 2010, The Divinity Project
	All rights reserved.
	http://board.thedivinityproject.com
	http://www.ragezone.com


	This file is part of Iris.

	Iris is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	Iris is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Iris.  If not, see <http://www.gnu.org/licenses/>.
*/

#region Includes

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Timers;

#endregion

namespace Iris
{
	internal class PacketHandler
	{
		Timer heartbeat;
		EventHandler events;

		public bool IsConnected
			{ get; private set; }

		public PacketHandler(ref EventHandler eventsHandler, double interval)
		{
			events = eventsHandler;
			heartbeat = new Timer(interval);
			heartbeat.Elapsed += new ElapsedEventHandler(heartbeat_Elapsed);
		}

		void heartbeat_Elapsed(object sender, ElapsedEventArgs e)
			{ Send(PacketFactory.ServerList()); }

		void Packet0A65(PacketReader reader)
		{
			IsConnected = true;
			events.Connected(this, new ConnectEventArgs());

			Send(PacketFactory.ServerList());

			heartbeat.Start();
		}

		void Packet0A66(PacketReader reader)
		{
			Globals.ServerList = new Dictionary<byte, Server>();

			reader.Skip(2);

			var count = reader.ReadUShort();

			for (int i = 0; i < count - 1; i++)
			{
				var id = reader.ReadByte();

				if (!Globals.ServerList.ContainsKey(id))
					Globals.ServerList.Add(id, new Server(id));

				var channel = new Channel();
				channel.ServerID = id;
				channel.ChannelID = reader.ReadByte();
				reader.Skip(1);

				var ip = String.Format("{0}.{1}.{2}.{3}", reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());

				channel.IP = ip;
				channel.Port = reader.ReadUShort();

				reader.Skip(2);

				channel.BloodyIce = reader.ReadUShort();
				channel.DesertScream = reader.ReadUShort();
				channel.GreenDespair = reader.ReadUShort();
				channel.PortLux = reader.ReadUShort();
				channel.FortRuina = reader.ReadUShort();
				channel.Lakeside = reader.ReadUShort();
				channel.UndeadGround = reader.ReadUShort();
				channel.ForgottenRuin = reader.ReadUShort();
				channel.MutantForest = reader.ReadUShort();
				channel.PontusFerrum = reader.ReadUShort();

				reader.Skip(24);

				channel.ForgottenTempleB1F = reader.ReadUShort();

				reader.Skip(12);

				channel.WarpCentre = reader.ReadUShort();

				channel.Players = (ushort)(channel.BloodyIce +
										   channel.DesertScream +
										   channel.GreenDespair +
										   channel.PortLux +
										   channel.FortRuina +
										   channel.Lakeside +
										   channel.UndeadGround +
										   channel.ForgottenRuin +
										   channel.MutantForest +
										   channel.PontusFerrum +
										   channel.ForgottenTempleB1F +
										   channel.WarpCentre);

				reader.Skip(2);

				channel.MaxPlayers = reader.ReadUShort();
				channel.IsOnline = (reader.ReadUShort() != 0);
				channel.Type = (ChannelType)reader.ReadUShort();

				reader.Skip(2);

				Globals.ServerList[id].Channels.Add(channel);
			}

			events.ReceivedServerList(this, new ServerListEventArgs());
		}

		void Packet65(PacketReader reader)
		{
			reader.Skip(6);

			Globals.ConnectionNumber = reader.ReadUShort();
			Globals.ClientID = reader.ReadUShort();

			events.LoggedIn(this, new LoginEventArgs());

			Send(PacketFactory.Login());
		}

		public void Parse(PacketReader reader)
		{
			switch (reader.Opcode)
			{
				case 0x07:  // Malformed packet error
					Globals.CleanUp();

					var opcode = reader.ReadUShort();
					events.Disconnected(this, new DisconnectEventArgs(DisconnectReason.ClosedByServer, opcode));
					break;
				case 0x0A:  // Login and server info
					switch (reader.ReadUShort())
					{
						case 0x65:  // Login reply
							Packet0A65(reader);
							break;
						case 0x66:  // Server info
							Packet0A66(reader);
							break;
					}
					break;
				case 0x65:  // Handshake reply
					Packet65(reader);
					break;
			}
		}

		public bool Send(byte[] packet, int size)
		{
			try
			{
				Globals.Client.Client.Send(packet, size, SocketFlags.None);
				return true;
			}
			catch { return false; }
		}

		public bool Send(PacketBuilder builder)
			{ return Send(builder.Data, builder.Size); }
	}
}