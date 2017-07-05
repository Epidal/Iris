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

#endregion

namespace Iris
{
	/// <summary>Contains information about a server and its channels.</summary>
	public class Server
	{
		/// <summary>The ID of the server.</summary>
		public byte ServerID;

		/// <summary>A list of channels belonging to this server.</summary>
		public List<Channel> Channels;

		/// <summary>Contains information about a server and its channels.</summary>
		/// <param name="id">This server's ID.</param>
		public Server(byte id = 1)
		{
			ServerID = id;
			Channels = new List<Channel>();
		}
	}
}