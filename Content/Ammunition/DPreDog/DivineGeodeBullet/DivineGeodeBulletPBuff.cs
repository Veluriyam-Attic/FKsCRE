﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using CalamityMod.Particles;

namespace FKsCRE.Content.Ammunition.DPreDog.DivineGeodeBullet
{
    internal class DivineGeodeBulletPBuff : ModBuff, ILocalizedModType
    {
        public new string LocalizationCategory => "Buffs";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false; // Buff 显示时间
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // 增加飞行时间
            player.wingTimeMax = (int)(player.wingTimeMax * 1.17f); // 增加 17% 的最大飞行时间

            // 每帧生成金黄色粒子特效
            if (Main.rand.NextBool(2)) // 50% 概率生成粒子
            {
                GenericSparkle impactParticle = new GenericSparkle(
                    player.Center + Main.rand.NextVector2Circular(10f, 10f), // 随机偏移，围绕玩家
                    Vector2.Zero, // 粒子无初始速度
                    Color.Goldenrod, // 金黄色
                    Color.White, // 白色渐变
                    Main.rand.NextFloat(0.7f, 1.2f), // 粒子大小
                    12 // 粒子生命周期
                );
                GeneralParticleHandler.SpawnParticle(impactParticle); // 生成粒子
            }
        }
    }
}
