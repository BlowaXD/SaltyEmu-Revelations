﻿using ChickenAPI.Enums.Game.Entity;

namespace ChickenAPI.Game.Packets.Game.Server
{
    [PacketHeader("c_mode")]
    public class CModePacketBase : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long CharacterId { get; set; }

        [PacketIndex(2)]
        public short Morph { get; set; }

        [PacketIndex(3)]
        public byte SpUpgrade { get; set; }

        [PacketIndex(4)]
        public byte SpDesign { get; set; }

        [PacketIndex(5, IsOptional = true)]
        public bool ArenaWinner { get; set; }

        #endregion
    }
}