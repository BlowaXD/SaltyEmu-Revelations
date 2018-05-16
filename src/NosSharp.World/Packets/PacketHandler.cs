﻿using System;
using System.Collections.Generic;
using ChickenAPI.ECS.Entities;
using ChickenAPI.Game;
using ChickenAPI.Game.Network;
using ChickenAPI.Packets;

namespace NosSharp.World.Packets
{
    public class PacketHandler : IPacketHandler
    {
        private readonly Dictionary<Type, PacketHandlerMethodReference> _packetHandler = new Dictionary<Type, PacketHandlerMethodReference>();
        private readonly Dictionary<string, PacketHandlerMethodReference> _packetHandlerMethod = new Dictionary<string, PacketHandlerMethodReference>();


        public void Register(PacketHandlerMethodReference method)
        {
            _packetHandlerMethod.TryAdd(method.Identification, method);
            _packetHandler.TryAdd(method.PacketType, method);
        }

        public void Unregister(Type type)
        {
            _packetHandler.Remove(type, out PacketHandlerMethodReference method);
            _packetHandlerMethod.Remove(method.Identification);
        }

        public PacketHandlerMethodReference GetPacketHandlerMethodReference(string header) => !_packetHandlerMethod.TryGetValue(header, out PacketHandlerMethodReference reference) ? null : reference;

        public void Handle(IPacket packet, ISession session, Type type)
        {
            if (packet == null)
            {
                return;
            }

            if (session.Player != null)
            {
                Handle(packet, session.Player, type);
                return;
            }

            if (!_packetHandler.TryGetValue(type, out PacketHandlerMethodReference methodReference))
            {
                return;
            }

            //check for the correct authority
            if (session.IsAuthenticated && (byte)methodReference.Authority > (byte)session.Account.Authority)
            {
                return;
            }

            methodReference.HandlerMethod(packet, session);
        }

        public void Handle(IPacket packet, IPlayerEntity player, Type type)
        {
            if (packet == null)
            {
                return;
            }

            if (player == null)
            {
                return;
            }
            if (!_packetHandler.TryGetValue(type, out PacketHandlerMethodReference methodReference))
            {
                return;
            }

            //check for the correct authority
            if ((byte)methodReference.Authority > (byte)player.Session.Account.Authority)
            {
                return;
            }

            // todo cleanup this
            methodReference.HandlerMethod(packet, player.Session);
        }
    }
}