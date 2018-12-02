﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.BCard;
using ChickenAPI.Enums.Game.BCard;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Args;
using ChickenAPI.Game.Inventory.ItemUsage.Handling;
using NLog.Targets;

namespace ChickenAPI.Game.BCards.Attributes
{
    public class BasicBCardHandlerContainer : IBCardHandlerContainer
    {
        private static readonly Logger Log = Logger.GetLogger<BaseUseItemHandler>();

        protected readonly Dictionary<BCardType, IBCardEffectHandler> Useitem = new Dictionary<BCardType, IBCardEffectHandler>();

        public BasicBCardHandlerContainer()
        {
            Assembly currentAsm = Assembly.GetAssembly(typeof(BasicBCardHandlerContainer));
            // get types
            foreach (Type type in currentAsm.GetTypes().Where(s => s.GetMethods().Any(m => m.GetCustomAttribute<BCardEffectHandlerAttribute>() != null)))
            {
                // each method for a type
                foreach (MethodInfo method in type.GetMethods().Where(s => s.GetCustomAttribute<BCardEffectHandlerAttribute>() != null))
                {
                    Register(new BasicBCardHandler(method));
                }
            }
        }

        public void Register(IBCardEffectHandler handler)
        {
            if (Useitem.ContainsKey(handler.HandledType))
            {
                return;
            }

            Useitem.Add(handler.HandledType, handler);
            Log.Info($"[REGISTER_HANDLER] BCARD_TYPE : {handler.HandledType} REGISTERED !");
        }

        public void Handle(IBattleEntity target, IBattleEntity sender, BCardDto bcard)
        {
            if (target == null)
            {
                return;
            }

            if (!Useitem.TryGetValue(bcard.Type, out IBCardEffectHandler handler))
            {
                return;
            }

            handler.Handle(target, sender, bcard);
        }
    }
}