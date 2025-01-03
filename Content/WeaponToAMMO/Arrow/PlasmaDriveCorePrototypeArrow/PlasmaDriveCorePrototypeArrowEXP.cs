﻿using CalamityMod.Buffs.StatDebuffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FKsCRE.Content.WeaponToAMMO.Arrow.PlasmaDriveCorePrototypeArrow
{
    public class PlasmaDriveCorePrototypeArrowEXP : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "WeaponToAMMO.Arrow.PlasmaDriveCorePrototypeArrow";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 500;
            Projectile.height = 500;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = Main.getGoodWorld ? -1 : 4;
            Projectile.timeLeft = Main.getGoodWorld ? 1200 : 120;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Main.getGoodWorld ? 5 : 15;
        }

        public override void AI()
        {
            float lights = Main.rand.Next(90, 111) * 0.01f;
            lights *= Main.essScale;
            Lighting.AddLight(Projectile.Center, 5f * lights, 1f * lights, 4f * lights);

            float projTimer = 25f;
            if (Projectile.ai[0] > 180f)
            {
                projTimer -= (Projectile.ai[0] - 180f) / 2f;
            }
            if (projTimer <= 0f)
            {
                projTimer = 0f;
                Projectile.Kill();
            }
            projTimer *= 0.7f;
            Projectile.ai[0] += 4f;
            int timerCounter = 0;

            while (timerCounter < projTimer)
            {
                float rando1 = Main.rand.Next(-40, 41);
                float rando2 = Main.rand.Next(-40, 41);
                float rando3 = Main.rand.Next(12, 36);
                float randoAdjust = (float)Math.Sqrt((double)(rando1 * rando1 + rando2 * rando2));
                randoAdjust = rando3 / randoAdjust;
                rando1 *= randoAdjust;
                rando2 *= randoAdjust;

                // 更改粒子类型为 UltraBrightTorch 和 Electric
                int randomDust = Main.rand.Next(2);
                if (randomDust == 0)
                {
                    randomDust = DustID.BlueFlare;
                }
                else
                {
                    randomDust = 206;
                }

                // 创建粒子效果
                int EXPLODE = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, randomDust, 0f, 0f, 100, default, 2f);
                Main.dust[EXPLODE].noGravity = true;
                Main.dust[EXPLODE].position.X = Projectile.Center.X;
                Main.dust[EXPLODE].position.Y = Projectile.Center.Y;
                Main.dust[EXPLODE].position.X += Main.rand.Next(-10, 11);
                Main.dust[EXPLODE].position.Y += Main.rand.Next(-10, 11);
                Main.dust[EXPLODE].velocity.X = rando1;
                Main.dust[EXPLODE].velocity.Y = rando2;

                timerCounter++;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Electrified, 300); // 原版的带电效果
            //target.AddBuff(ModContent.BuffType<GalvanicCorrosion>(), 300); // 电偶腐蚀
        }
    }
}
