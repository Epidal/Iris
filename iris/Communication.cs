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
using System.Net.Sockets;

#endregion

namespace Iris
{
	/// <summary>Handles communications to and from CABAL Online's GlobalMgrSvr.</summary>
	public class Communication
	{
		PacketHandler packets;
		PacketReader reader = new PacketReader();

		/// <summary>
		/// The EventHandler associated with this class instance.
		/// <para>All events will be fired by this class, so register your event listeners with it.</para>
		/// </summary>
		public EventHandler Events;

		public int ClientID
			{ get { return Globals.ClientID; } }

		public int ConnectionNumber
			{ get { return Globals.ConnectionNumber; } }

		/// <summary>Returns TRUE if Iris is currently logged in to the GlobalMgrSvr.</summary>
		public bool IsConnected
			{ get { return packets.IsConnected; } }

		/// <summary>Handles communications to and from CABAL Online's GlobalMgrSvr</summary>
		/// <param name="interval">
		/// The amount of time between each "heartbeat" Iris sends.
		/// <para>Each "heartbeat" results in the GlobalMgrSvr sending a server list to Iris.</para>
		/// </param>
		public Communication(double interval = 30000)
		{
			Events = new EventHandler();
			packets = new PacketHandler(ref Events, interval);
		}

		void Receive(IAsyncResult ar)
		{
			if (ar == null || Globals.Client == null || !Globals.Client.Connected)
				return;

			byte[] buf = (byte[])ar.AsyncState;
			int read = Globals.Client.Client.EndReceive(ar);

			if (read != 0)
			{
				reader.New(ref buf, 0, read);
				packets.Parse(reader);

				buf = new byte[0x2000];
				Globals.Client.Client.BeginReceive(buf, 0, 0x2000, SocketFlags.None, Receive, buf);
			}
			else
			{
				Events.Disconnected(this, new DisconnectEventArgs(DisconnectReason.ClosedByServer));

				Globals.CleanUp();
			}
		}

		/// <summary>Connects and logs Iris in to the specified GlobalMgrSvr.</summary>
		/// <param name="ip">The IP of the GlobalMgrSvr to connect to.</param>
		/// <param name="port">The port that the GlobalMgrSvr listens on.</param>
		/// <returns>TRUE if succeeded, else FALSE.</returns>
		public bool Connect(string ip, int port)
		{
			try
			{
				if (Globals.Client != null)
				{
					Disconnect();
					Globals.Client = null;
				}

				Globals.Client = new TcpClient();
				Globals.Client.Connect(ip, port);
				byte[] buf = new byte[0x2000];
				Globals.Client.Client.BeginReceive(buf, 0, 0x2000, SocketFlags.None, Receive, buf);

				packets.Send(PacketFactory.Handshake());

				return true;
			}
			catch { return false; }
		}

		/// <summary>
		/// Disconnects Iris from the GlobalMgrSvr.
		/// <para>This needs to be done before before you close the host application!  If you don't disconnect from the GlobalMgrSvr, Iris' user session will persist, causing one of the GlobalMgrSvr's available user slots to be used up.  This can only then be resolved by restarting the GlobalMgrSvr</para>
		/// </summary>
		/// <returns>TRUE if succeeded, else FALSE.</returns>
		public bool Disconnect()
		{
			try
			{
				if (Globals.Client.Connected == true)
				{
					Globals.CleanUp();

					Events.Disconnected(this, new DisconnectEventArgs(DisconnectReason.ClosedByClient));

					return true;
				}

				return false;
			}
			catch { return false; }
		}

		/// <summary>
		/// Disconnects all players connected to the specified channel.
		/// <para>This is very useful if you plan on shutting down the server for maintenance, as it allows all users to be stored to the database, avoiding accidental rollbacks and loss of data.</para>
		/// </summary>
		/// <param name="server">The ID of the server.</param>
		/// <param name="channel">
		/// The ID of the channel.
		/// <para>Use '0' to specify all channels.</para>
		/// </param>
		/// <returns>TRUE if succeeded, else FALSE.</returns>
		public bool DisconnectPlayers(byte server, byte channel)
		{
			try
			{
				if (channel == 0)
				{
					bool result = true;

					foreach (var c in Globals.ServerList[server].Channels)
						if (c.ChannelID != 0)
							result = DisconnectPlayers(server, c.ChannelID) && result;

					return result;
				}

				return packets.Send(PacketFactory.Disconnect(server, channel));
			}
			catch { return false; }
		}
		/// <summary>
		/// Disconnects all players connected to the specified channels.
		/// <para>This is very useful if you plan on shutting down the server for maintenance, as it allows all users to be stored to the database, avoiding accidental rollbacks/loss of data.</para>
		/// </summary>
		/// <param name="server">The ID of the server.</param>
		/// <param name="channel">An array of channel ID's.</param>
		/// <returns>TRUE if succeeded, else FALSE.</returns>
		public bool DisconnectPlayers(byte server, byte[] channels)
		{
			if (channels.Length == 0) return false;

			bool result = true;

			foreach (var c in channels)
				result = DisconnectPlayers(server, c) && result;

			return result;
		}

		/// <summary>
		/// Broadcasts a message to the specified channel.
		/// <para>This message will display at the center-top and bottom-right of each player's screen.</para>
		/// </summary>
		/// <param name="server">The ID of the server.</param>
		/// <param name="channel">
		/// The ID of the channel.
		/// <para>Use '0' to specify all channels.</para>
		/// </param>
		/// <param name="message">The message to broadcast.  Colours are supported.</param>
		/// <returns>TRUE if succeeded, else FALSE.</returns>
		public bool SendMessage(byte server, byte channel, string message)
		{
			try
			{
				if (channel == 0)
				{
					bool result = true;

					foreach (var c in Globals.ServerList[server].Channels)
						if (c.ChannelID != 0)
							result = SendMessage(server, c.ChannelID, message) && result;

					return result;
				}

				return packets.Send(PacketFactory.Message(server, channel, message));
			}
			catch { return false; }
		}
		/// <summary>
		/// Sends a GM message to the specified channel.
		/// <para>This message will display a notification at the bottom-right of each user's screen, which they can then click to show the whole message.</para>
		/// </summary>
		/// <param name="server">The ID of the server.</param>
		/// <param name="channel">
		/// The ID of the channel.
		/// <para>Use '0' to specify all channels.</para>
		/// </param>
		/// <param name="title">The title of the GM message.  Colours are NOT supported.</param>
		/// <param name="message">The message to broadcast.  Colours are supported.</param>
		/// <returns>TRUE if succeeded, else FALSE.</returns>
		public bool SendMessage(byte server, byte channel, string title, string message)
		{
			try
			{
				if (channel == 0)
				{
					bool result = true;

					foreach (var c in Globals.ServerList[server].Channels)
						if (c.ChannelID != 0)
							result = SendMessage(server, c.ChannelID, title, message) && result;

					return result;
				}

				return packets.Send(PacketFactory.Message(server, channel, title, message));
			}
			catch { return false; }
		}
		/// <summary>
		/// Broadcasts a message to the specified channels.
		/// <para>This message will display at the center-top and bottom-right of each player's screen.</para>
		/// </summary>
		/// <param name="server">The ID of the server.</param>
		/// <param name="channel">An array of channel ID's.</param>
		/// <param name="message">The message to broadcast.  Colours are supported.</param>
		/// <returns>TRUE if succeeded, else FALSE.</returns>
		public bool SendMessage(byte server, byte[] channels, string message)
		{
			try
			{
				if (channels.Length == 0) return false;

				bool result = true;

				foreach (var c in channels)
					result = SendMessage(server, c, message) && result;

				return result;
			}
			catch { return false; }
		}
		/// <summary>
		/// Sends a GM message to the specified channels.
		/// <para>This message will display a notification at the bottom-right of each user's screen, which they can then click to show the whole message.</para>
		/// </summary>
		/// <param name="server">The ID of the server.</param>
		/// <param name="channel">An array of channel ID's.</param>
		/// <param name="title">The title of the GM message.  Colours are NOT supported.</param>
		/// <param name="message">The message to broadcast.  Colours are supported.</param>
		/// <returns>TRUE if succeeded, else FALSE.</returns>
		public bool SendMessage(byte server, byte[] channels, string title, string message)
		{
			try
			{
				if (channels.Length == 0) return false;

				bool result = true;

				foreach (var c in channels)
					result = SendMessage(server, c, title, message) && result;

				return result;
			}
			catch { return false; }
		}

		/// <summary>Sets the type of a channel.</summary>
		/// <param name="server">The server that the channel belongs to.</param>
		/// <param name="channel">The channel to edit.</param>
		/// <param name="type">The new channel type.</param>
		/// <returns></returns>
		public bool SetChannelType(byte server, byte channel, uint type)
		{
			try { return packets.Send(PacketFactory.ChannelType(server, channel, type)); }
			catch { return false; }
		}
	}
}