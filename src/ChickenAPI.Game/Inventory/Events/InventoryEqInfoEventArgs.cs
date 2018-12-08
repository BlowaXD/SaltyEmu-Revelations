﻿using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Inventory.Events
{
    public class InventoryEqInfoEventArgs : ChickenEventArgs
    {
        public byte Type { get; set; } // todo more information

        public short Slot { get; set; }

        public long? ShopOwnerId { get; set; }
    }
}