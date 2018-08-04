﻿using ChickenAPI.Core.Data.TransferObjects;

namespace ChickenAPI.Game.Data.TransferObjects.NpcMonster
{
    public class NpcMonsterSkillDto : IMappedDto
    {
        public long SkillId { get; set; }
        public short Rate { get; set; }
        public long NpcMonsterId { get; set; }

        /// <summary>
        ///     Can be considered as the skill vnum
        /// </summary>
        public long Id { get; set; }
    }
}