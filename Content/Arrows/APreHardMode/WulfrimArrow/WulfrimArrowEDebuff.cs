﻿using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Melee;

namespace FKsCRE.Content.Arrows.APreHardMode.WulfrimArrow
{
    public class WulfrimArrowEDebuff : ModBuff
    {
        private static int LastKnownDamage = 10; // 默认值

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true; // 设定为debuff
            Main.buffNoSave[Type] = true; // Buff不保存到存档
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            // 每帧检测场上的 WulfrimArrowPROJ
            foreach (var proj in Main.projectile)
            {
                if (proj.active && proj.type == ModContent.ProjectileType<WulfrimArrowPROJ>())
                {
                    LastKnownDamage = (int)(proj.damage * 0.5f); // 更新伤害值为最后一个 WulfrimArrowPROJ 的 50%
                }
            }

            // 计时器逻辑
            if (npc.buffTime[buffIndex] % 30 == 0) // 每30帧释放一次
            {
                for (int i = 0; i < 8; i++) // 八个方向发射 Spark 弹幕
                {
                    Vector2 velocity = new Vector2(0, -1f).RotatedBy(MathHelper.PiOver4 * i) * 4f; // 均匀分布方向
                    Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center, velocity,
                        ModContent.ProjectileType<Spark>(), LastKnownDamage, 0f, Main.myPlayer);
                }
            }
        }
    }
}
