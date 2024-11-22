using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FKsCRE.Content.Ammunition.BPrePlantera.StarblightSootBullet
{
    internal class StarblightSootBulletEBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true; // 是一个负面状态
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.damage = (int)(npc.damage * 0.7);


            //if (npc.justHit) // 仅在受伤时触发
            //{
            //    NPC closest = FindClosestNPC(npc);
            //    if (closest != null && closest != npc)
            //    {
            //        //closest.StrikeNPC(npc.lastDamage, 0f, 0);
            //        npc.DelBuff(buffIndex); // 同步一次后移除 Buff
            //    }
            //}
        }

        //private NPC FindClosestNPC(NPC source)
        //{
        //    NPC closest = null;
        //    float minDistance = float.MaxValue;

        //    foreach (NPC npc in Main.npc)
        //    {
        //        if (npc.active && npc != source && Vector2.Distance(source.Center, npc.Center) < minDistance)
        //        {
        //            closest = npc;
        //            minDistance = Vector2.Distance(source.Center, npc.Center);
        //        }
        //    }

        //    return closest;
        //}
    }
}
