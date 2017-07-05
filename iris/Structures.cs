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
	/// <summary>Contains all information about a given channel.</summary>
	public struct Channel
	{
		/// <summary>The ID of the server this channel belongs to.</summary>
		public byte ServerID;
		/// <summary>This channel's ID.</summary>
		public byte ChannelID;
		/// <summary>The IP used to connect to this channel.</summary>
		public string IP;
		/// <summary>The port this channel's WorldSvr is listening on.</summary>
		public ushort Port;
		/// <summary>The number of players currently in Bloody Ice.</summary>
		public ushort BloodyIce;
		/// <summary>The number of players currently in Desert Scream.</summary>
		public ushort DesertScream;
		/// <summary>The number of players currently in Green Despair.</summary>
		public ushort GreenDespair;
		/// <summary>The number of players currently in Port Lux.</summary>
		public ushort PortLux;
		/// <summary>The number of players currently in Fort Ruina.</summary>
		public ushort FortRuina;
		/// <summary>The number of players currently in Lakeside.</summary>
		public ushort Lakeside;
		/// <summary>The number of players currently in Undead Ground.</summary>
		public ushort UndeadGround;
		/// <summary>The number of players currently in Forgotten Ruin.</summary>
		public ushort ForgottenRuin;
		/// <summary>The number of players currently in Mutant Forest.</summary>
		public ushort MutantForest;
		/// <summary>The number of players currently in Pontus Ferrum.</summary>
		public ushort PontusFerrum;
		public ushort FloodedOldCity;
		public ushort Giant;
		public ushort FrozenTowerofUndead;
		/// <summary>The number of players currently in Ruina Station.</summary>
		public ushort RuinaStation;
		public ushort MAPHEL;
		public ushort AUTRA;
		public ushort AGRANE;
		public ushort OTAM;
		/// <summary>The number of players currently in Jail.</summary>
		public ushort Jail;
		public ushort ChaosArena;
		/// <summary>The number of players currently in Tierra Gloriosa.</summary>
		public ushort TierradelBruto;
		public ushort Unknown;
		public ushort ForgottenTempleB1F;
		/// <summary>The number of players currently in the Volcanic Citadel.</summary>
		public ushort TheVolcanicCitadel;
		/// <summary>The number of players currently in Exilian Volcano.</summary>
		public ushort ExilianVolcano;
		/// <summary>The number of players currently in Lake in Dusk.</summary>
		public ushort LakeinDusk;
		public ushort DungeonWorld3;
		public ushort DungeonWorld2;
		public ushort DungeonWorld1;
		/// <summary>The number of players currently in the Warp Centre.</summary>
		public ushort WarpCentre;
		/// <summary>The number of players currently in Tierra Gloriosa's lobby.</summary>
		public ushort LBS;
		/// <summary>The number of players currently online.</summary>
		public ushort Players;
		/// <summary>The maximum number of allowed players.</summary>
		public ushort MaxPlayers;
		/// <summary>This channel's type.</summary>
		public ChannelType Type;
		/// <summary>Returns a boolean that indicates whether or not this channel is currently online.</summary>
		public bool IsOnline;
	}
}