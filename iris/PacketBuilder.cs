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
	public class PacketBuilder : IDisposable
	{
		byte[] _data;

		public byte[] Data
		{
			get
			{
				_data[2] = (byte)Size;
				_data[3] = (byte)(Size >> 8);

				return _data;
			}
		}

		public uint Padding
		{
			set
			{
				unsafe
				{
					fixed (byte* pdata = _data)
						*((uint*)&pdata[4]) = value;
				}
			}
		}

		public ushort Opcode
		{
			get
			{
				ushort result = (ushort)_data[8];
				result += (ushort)(_data[9] << 8);

				return result;
			}
		}

		public int Size
			{ get; private set; }

		public PacketBuilder() { }	// Just here for sanity's sake, and so we can optimise using the New() method.

		public void New(int opcode)
		{
			_data = new byte[1024];
			_data[0] = 0xE2;
			_data[1] = 0xB7;

			unchecked
			{
				_data[8] = (byte)opcode;
				_data[9] = (byte)(opcode >> 8);
			}

			Size = 10;
		}

		public void New(int opcode, int opcode2)
		{
			New(opcode);

			unchecked
			{
				_data[10] = (byte)opcode2;
				_data[11] = (byte)(opcode2 >> 8);
			}

			Size += 2;
		}

		/// <summary>Appends data to the end of the packet.</summary>
		/// <param name="packet">this</param>
		/// <param name="value">The data to be added to the end of the packet.</param>
		/// <returns>this</returns>
		public static PacketBuilder operator +(PacketBuilder packet, byte[] value)
		{
			Array.ConstrainedCopy(value, 0, packet._data, packet.Size, value.Length);

			packet.Size += value.Length;

			return packet;
		}

		public static PacketBuilder operator +(PacketBuilder packet, long value)
		{
			unsafe
			{
				fixed (byte* pdata = packet._data)
				{
					byte* pd = pdata;
					pd += packet.Size;

					*((long*)pd) = value;
					packet.Size += 8;
				}
			}

			return packet;
		}

		public static PacketBuilder operator +(PacketBuilder packet, ulong value)
		{
			unsafe
			{
				fixed (byte* pdata = packet._data)
				{
					byte* pd = pdata;
					pd += packet.Size;

					*((ulong*)pd) = value;
					packet.Size += 8;
				}
			}

			return packet;
		}

		public static PacketBuilder operator +(PacketBuilder packet, int value)
		{
			unsafe
			{
				fixed (byte* pdata = packet._data)
				{
					byte* pd = pdata;
					pd += packet.Size;

					*((int*)pd) = value;
					packet.Size += 4;
				}
			}

			return packet;
		}

		public static PacketBuilder operator +(PacketBuilder packet, uint value)
		{
			unsafe
			{
				fixed (byte* pdata = packet._data)
				{
					byte* pd = pdata;
					pd += packet.Size;

					*((uint*)pd) = value;
					packet.Size += 4;
				}
			}

			return packet;
		}

		public static PacketBuilder operator +(PacketBuilder packet, short value)
		{
			unsafe
			{
				fixed (byte* pdata = packet._data)
				{
					byte* pd = pdata;
					pd += packet.Size;

					*((short*)pd) = value;
					packet.Size += 2;
				}
			}

			return packet;
		}

		public static PacketBuilder operator +(PacketBuilder packet, ushort value)
		{
			unsafe
			{
				fixed (byte* pdata = packet._data)
				{
					byte* pd = pdata;
					pd += packet.Size;

					*((ushort*)pd) = value;
					packet.Size += 2;
				}
			}

			return packet;
		}

		public static PacketBuilder operator +(PacketBuilder packet, byte value)
		{
			packet._data[packet.Size] = value;
			packet.Size += 1;

			return packet;
		}

		public static PacketBuilder operator +(PacketBuilder packet, float value)
		{
			unsafe
			{
				fixed (byte* pdata = packet._data)
				{
					byte* pd = pdata;
					pd += packet.Size;

					*((float*)pd) = value;
					packet.Size += 4;
				}
			}

			return packet;
		}

		public static PacketBuilder operator +(PacketBuilder packet, double value)
		{
			unsafe
			{
				fixed (byte* pdata = packet._data)
				{
					byte* pd = pdata;
					pd += packet.Size;

					*((double*)pd) = value;
					packet.Size += 8;
				}
			}

			return packet;
		}

		public static PacketBuilder operator +(PacketBuilder packet, string value)
		{
			packet += System.Text.Encoding.ASCII.GetBytes((string)value);

			return packet;
		}

		public void Dispose()
		{
			_data = null;
		}
	}
}