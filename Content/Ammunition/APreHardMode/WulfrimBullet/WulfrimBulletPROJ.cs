﻿using FKsCRE.Content.Ammunition.APreHardMode.AerialiteBullet;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using FKsCRE.CREConfigs;
using CalamityMod;

namespace FKsCRE.Content.Ammunition.APreHardMode.WulfrimBullet
{
    internal class WulfrimBulletPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectile.APreHardMode";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 画残影效果
                CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
                return false;
            }
            return true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 420;
            Projectile.MaxUpdates = 6;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 2;
        }

        public override void AI()
        {
            // 由于我们是水平贴图，因此什么也不需要转动
            Projectile.rotation = Projectile.velocity.ToRotation();

            // 添加光效
            Lighting.AddLight(Projectile.Center, Color.Lerp(Color.Black, Color.Black, 0.5f).ToVector3() * 0.49f);

            // 子弹在出现之后很短一段时间会变得可见
            if (Projectile.timeLeft == 418)
                Projectile.alpha = 0;

            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 添加飞行粒子特效
                if (Main.rand.NextBool(3)) // 1/3 概率生成粒子
                {
                    Dust dust = Dust.NewDustPerfect(
                        Projectile.Center,
                        Main.rand.NextBool() ? DustID.GreenTorch : DustID.CursedTorch,
                        -Projectile.velocity.RotatedByRandom(0.1f) * Main.rand.NextFloat(0.1f, 0.3f)
                    );
                    dust.noGravity = true; // 无重力
                    dust.scale = Main.rand.NextFloat(0.7f, 1.1f); // 随机大小
                }

            }


         }
        public override void OnSpawn(IEntitySource source)
        {
           

        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner]; // 获取发射弹幕的玩家
            float distance = Vector2.Distance(player.Center, Projectile.Center); // 计算玩家与弹幕的距离

            if (distance <= 3 * 16)
            {
                // 玩家距离小于或等于 3 个 tile（48 像素），伤害倍率为 1.5 倍
                modifiers.FinalDamage *= 1.5f;
            }
            else if (distance > 35 * 16)
            {
                // 玩家距离大于 35 个 tile（560 像素），每多 1 tile（16 像素）伤害降低 5%
                float excessDistance = (distance - 35 * 16) / 16; // 超出部分的 tile 数
                float damageReductionFactor = Math.Max(1f - excessDistance * 0.05f, 0f); // 确保倍率不为负
                modifiers.FinalDamage *= damageReductionFactor;
            }
            else
            {
                // 玩家距离在 20~35 个 tile 之间，伤害倍率保持 1 倍
                modifiers.FinalDamage *= 1f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
           

        }

        public override void OnKill(int timeLeft)
        {

        }
    }
}
