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

namespace Iris
{
	/// <summary>Handles all events in Iris.</summary>
	public class EventHandler
	{
		public delegate void ConnectHandler(object sender, ConnectEventArgs e);
		public delegate void DisconnectHandler(object sender, DisconnectEventArgs e);
		public delegate void LoginHandler(object sender, LoginEventArgs e);
		public delegate void ServerListHandler(object sender, ServerListEventArgs e);

		/// <summary>Fires when Iris connects to the GlobalMgrSvr.</summary>
		public event ConnectHandler OnConnect;

		/// <summary>Fires when Iris disconnects from the GlobalMgrSvr, either forcibly or intentionally.</summary>
		public event DisconnectHandler OnDisconnect;

		/// <summary>Fires when Iris logs in to and authenticates with the GlobalMgrSvr.</summary>
		public event LoginHandler OnLogin;

		/// <summary>
		/// Fires when Iris receives the server list from the GlobalMgrSvr.
		/// <para>This happens when Iris initially connects, and after every "heartbeat" sent by Iris.</para>
		/// </summary>
		public event ServerListHandler OnReceiveServerList;

		internal void Connected(object sender, ConnectEventArgs e)
			{ if (OnConnect != null) OnConnect(sender, e); }

		internal void Disconnected(object sender, DisconnectEventArgs e)
			{ if (OnDisconnect != null) OnDisconnect(sender, e); }

		internal void LoggedIn(object sender, LoginEventArgs e)
			{ if (OnLogin != null) OnLogin(sender, e); }

		internal void ReceivedServerList(object sender, ServerListEventArgs e)
			{ if (OnReceiveServerList != null) OnReceiveServerList(sender, e); }
	}
}