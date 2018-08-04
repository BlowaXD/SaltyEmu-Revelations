﻿using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Enums;
using ChickenAPI.Game.Data.AccessLayer.Skill;
using ChickenAPI.Game.Data.TransferObjects.Skills;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Skills;
using ChickenAPI.Game.Features.Skills.Args;
using ChickenAPI.Game.Packets;

namespace NosSharp.PacketHandler.Skill.Commands
{
    public class AddSkillCommandHandling
    {
        public static void OnAddSkillCommand(AddSkillCommandPacket packet, IPlayerEntity player)
        {
            var skillService = Container.Instance.Resolve<ISkillService>();
            SkillDto skill = skillService.GetById(packet.SkillId);

            player.NotifySystem<SkillSystem>(new PlayerAddSkillEventArgs
            {
                Skill = skill
            });
        }
    }

    [PacketHeader("$AddSkill", Authority = AuthorityType.GameMaster)]
    public class AddSkillCommandPacket : PacketBase
    {
        [PacketIndex(0)]
        public long SkillId { get; set; }
    }
}
