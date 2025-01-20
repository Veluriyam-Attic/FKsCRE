﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace FKsCRE.Content.Ammunition.CPreMoodLord.AstralBullet
{
    public class AstralBulletEBuff : ModBuff, ILocalizedModType
    {
        public new string LocalizationCategory => "Buffs";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true; // 确保这个buff是一个debuff
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.HasBuff(ModContent.BuffType<AstralBulletEBuff>()))
            {
                npc.GetGlobalNPC<AstralBulletGlobalNPC>().hasAstralBulletBuff = true;
            }
        }
    }
}

