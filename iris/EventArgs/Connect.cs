﻿/*
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

#endregion

namespace Iris
{
	/// <summary>Provides data for the Iris.OnConnect event.</summary>
	public class ConnectEventArgs : EventArgs
	{
		/// <summary>The IP of this client.</summary>
		public string IP { get; private set; }

		/// <summary>The time this event was fired.</summary>
		public DateTime Time { get; private set; }

		/// <summary>Provides data for the Iris.OnReceiveServerList event.</summary>
		public ConnectEventArgs()
		{
			IP = Globals.Client.Client.LocalEndPoint.ToString().Split(':')[0];
			Time = DateTime.Now;
		}
	}
}