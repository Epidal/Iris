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
	abstract class PacketFactory
	{
		static PacketBuilder builder = new PacketBuilder();

		public static PacketBuilder ChannelType(byte server, byte channel, uint type)
		{
			/*	**STRUCTURE**
				-------------
				u2 header
				u2 size
				u4 unk
				u2 opcode
				u2 opcode2
				u1 server
				u1 channel
				u1 unk
				u2 unk
				u4 type
				u4 unk     */

			builder.New(0x0A, 0x6A);
			builder.Padding = 0x0D0000;
			builder += server;
			builder += channel;
			builder += (byte)0;
			builder += (ushort)0xFF00;
			builder += type;
			builder += 0xFE01FE01;

			return builder;
		}

		public static PacketBuilder Disconnect(byte server, byte channel)
		{
			/*	**STRUCTURE**
				-------------
				u2 header
				u2 size
				u4 unk
				u2 opcode
				u2 opcode2
				u1 server
				u1 channel
				u8 unk
				u8 unk
				u8 unk
				u8 unk
				u8 unk     */

			builder.New(0x0A, 0x69);
			builder.Padding = 0x0FF4;
			builder += server;
			builder += channel;
			builder += 0;
			builder += 0x2045434100240400;
			builder += 0x203A545245535341;
			builder += 0x734D747069726353;
			builder += 0x694C207070632E67;
			builder += 0x606034343120656E;

			return builder;
		}

		public static PacketBuilder Handshake()
		{
			/*	**STRUCTURE**
				-------------
				u2 header
				u2 size
				u4 padding
				u2 opcode
				u4 unk     */

			builder.New(0x65);
			builder += 0;

			return builder;
		}

		public static PacketBuilder Login()
		{
			/*	**STRUCTURE**
				-------------
				u2 header
				u2 size
				u4 padding
				u2 opcode
				u2 opcode2
				u2 unk     */

			builder.New(0x0A, 0x65);
			builder += 0x3DA74E9C;

			return builder;
		}

		public static PacketBuilder Message(byte server, byte channel, string message)
		{
			/*	**STRUCTURE**
				-------------
				u2 header
				u2 size
				u4 unk
				u2 opcode
				u2 opcode2
				u1 server
				u1 channel
				u2 unk
				u4 unk
				u2 msgsize
				u? msg     */

			builder.New(0x0A, 0x66);
			builder.Padding = 0x8C;
			builder += server;
			builder += channel;
			builder += (ushort)0;
			builder += 0;
			builder += (ushort)(message.Length + 2);
			builder += "``";
			builder += message;

			return builder;
		}

		public static PacketBuilder Message(byte server, byte channel, string title, string message)
		{
			/*	**STRUCTURE**
				-------------
				u2 header
				u2 size
				u4 unk
				u2 opcode
				u2 opcode2
				u1 server
				u1 channel
				u2 unk
				u4 unk
				u2 titlesize
				u? title
				u1 unk
				u? msg     */

			builder.New(0x0A, 0x66);
			builder.Padding = 0x8C;
			builder += server;
			builder += channel;
			builder += (ushort)0;
			builder += 0;
			builder += (ushort)(title.Length + message.Length + 3);
			builder += title;
			builder += "`";
			builder += (byte)0x0A;
			builder += message;
			builder += "`";

			return builder;
		}

		public static PacketBuilder ServerList()
		{
			/*	**STRUCTURE**
				-------------
				u2 header
				u2 size
				u4 unk
				u2 opcode
				u2 opcode2
				u4 unk     */

			builder.New(0x0A, 0x66);
			builder.Padding = 0xCEBA0018;
			builder += (ushort)0;

			return builder;
		}
	}
}