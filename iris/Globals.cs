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

using System.Collections.Generic;
using System.Net.Sockets;

#endregion

namespace Iris
{
	internal class Globals
	{
		internal static TcpClient Client;
		internal static ushort ConnectionNumber;
		internal static ushort ClientID;
		internal static Dictionary<byte, Server> ServerList;

		internal static void CleanUp()
		{
			Client.Client.Disconnect(true);
			Client.Client.Close();
			Client.Close();
			Client = null;
			ClientID = 0;
			ConnectionNumber = 0;
		}
	}
}