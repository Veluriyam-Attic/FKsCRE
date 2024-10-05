﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace FKsCRE.Content.Arrows.DPreDog.UelibloomArrow
{
    internal class UelibloomArrowEBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.velocity *= 0.95f;

            // 绿色粒子效果
            for (int i = 0; i < 1; i++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Grass, 0f, 0f, 100, default, 1f);
            }
        }
    }
}
