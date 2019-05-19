﻿using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game;
using ChickenAPI.Game.Configuration;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicPlugin.EventHandlers.Inventory
{
    public class Inventory_MoveItem_Handler : GenericEventPostProcessorBase<InventoryMoveEvent>
    {
        private readonly IGameConfiguration _gameConfiguration;

        public Inventory_MoveItem_Handler(ILogger log, IGameConfiguration gameConfiguration) : base(log) => _gameConfiguration = gameConfiguration;

        protected override async Task Handle(InventoryMoveEvent args, CancellationToken cancellation)
        {
            if (!(args.Sender is IPlayerEntity player))
            {
                return;
            }
            InventoryComponent inv = player.Inventory;

            ItemInstanceDto source = inv.GetSubInvFromInventoryType(args.PocketType)[args.SourceSlot];
            ItemInstanceDto dest = inv.GetSubInvFromInventoryType(args.PocketType)[args.DestinationSlot];

            if (source == null)
            {
                return;
            }

            if (dest != null && (args.PocketType == PocketType.Main || args.PocketType == PocketType.Etc) && dest.ItemId == source.ItemId &&
                dest.Amount + source.Amount > _gameConfiguration.Inventory.MaxItemPerSlot)
            {
                // if both source & dest are stackable && slots combined are > max slots
                // should provide a "fill" possibility
                return;
            }

            if (dest == null)
            {
                await inv.MoveItem(source, args);
            }
            else
            {
                await inv.MoveItems(source, dest);
            }
        }
    }
}