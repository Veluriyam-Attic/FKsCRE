using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;


namespace FKsCRE.Content.Arrows.WulfrimArrow
{
    internal class WulfrimArrowEBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true; // 设定为debuff
            Main.buffNoTimeDisplay[Type] = false; // 不显示时间
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            // 减少移动速度到5%
            npc.velocity *= 0.05f;

            // 每两帧生成一个 WulfrumBolt 粒子特效
            if (npc.buffTime[buffIndex] % 2 == 0) // 每2帧检查一次
            {
                // 在 NPC 的位置生成 WulfrumBolt 粒子特效
                Dust wulfrumDust = Dust.NewDustPerfect(npc.Center, 267, Vector2.Zero); // 267 是 WulfrumBolt 的粒子
                wulfrumDust.noGravity = true; // 无重力效果
            }
        }
    }
}