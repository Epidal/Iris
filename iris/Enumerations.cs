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

#endregion

namespace Iris
{
	/// <summary>
	/// Specifies the channel's type.
	/// <para>Adult channels allow PK'ing, whereas Juniour channels don't.  Normally, only 18+ members can join Adult channels.  However, it seems our server files have this disabled.</para>
	/// </summary>
	[Flags]
	public enum ChannelType : ushort
	{
		/// <summary>Not used.  Just here for consistency, and because the compiler likes it.</summary>
		None = 0x00,
		/// <summary>
		/// PK Channel.
		/// <para>Can also restrict access to players 18+ years of age.  Maybe we can enable this in our servers?</para>
		/// </summary>
		Adult = 0x01,
		/// <summary>Non-PK Channel.</summary>
		Juniour = 0x02,
		/// <summary>Premium Channel.  Only users with premium may join.</summary>
		Premium = 0x04,
		War = 0x08,
		Unk0 = 0x10,
		/// <summary>
		/// Admin-only Channel.
		/// <para>Only IP's specified in AffiliatedCorpIP and AdminIP can connect to a channel with this type.</para>
		/// </summary>
		Private = 0x20,
		Unk1 = 0x40,
		Unk2 = 0x80,
		Unk3 = 0x0100,
		Unk4 = 0x0200,
		Trade = 0x0400
	}

	/// <summary>Specifies a reason for Iris being disconnected.</summary>
	public enum DisconnectReason
	{
		/// <summary>Not used.  Just here for consistency, and because the compiler likes it.</summary>
		None = 0,
		/// <summary>The connection was closed via Iris.Disconnect().  This is normal, so no need to catch this as an error.</summary>
		ClosedByClient,
		/// <summary>The connection was forcibly closed by the server due to some sort of error.</summary>
		ClosedByServer,
		/// <summary>The connection timed out.  Currently not used.  Will add support for this later.</summary>
		TimedOut
	}
}