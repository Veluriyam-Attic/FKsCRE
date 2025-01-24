﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace FKsCRE.Content.Gel.EAfterDog.CosmosGel
{
    public class CosmosGelGP : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool IsCosmosGelInfused = false;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo ammoSource && ammoSource.AmmoItemIdUsed == ModContent.ItemType<CosmosGel>())
            {
                IsCosmosGelInfused = true;
                projectile.netUpdate = true;
                projectile.damage = (int)(projectile.damage * 0.70f); // 减少 30% 伤害

                // 检查场上是否已有超过 5 个 某种 弹幕
                int sparkCount = 0;
                foreach (Projectile proj in Main.projectile)
                {
                    if (proj.active && proj.type == ModContent.ProjectileType<CosmosGelEater>())
                    {
                        sparkCount++;
                        if (sparkCount >= 5)
                            return; // 如果已存在 5 个 某种 弹幕，则不释放新的
                    }
                }

                // 在玩家位置附近生成 1~2 个 CosmosGelEater 弹幕
                int extraProjectiles = Main.rand.Next(1, 3);
                for (int i = 0; i < extraProjectiles; i++)
                {
                    Vector2 spawnOffset = new Vector2(Main.rand.Next(-25, 26), Main.rand.Next(-25, 26));
                    float randomAngle = MathHelper.ToRadians(Main.rand.Next(0, 360));
                    Vector2 velocity = new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle)) * 5f; // 初速度为 5f

                    Projectile.NewProjectile(
                        projectile.GetSource_FromThis(),
                        Main.player[projectile.owner].Center + spawnOffset,
                        velocity,
                        ModContent.ProjectileType<CosmosGelEater>(),
                        (int)(projectile.damage / 0.7 * 0.45f), // 伤害为原弹幕的 45%
                        projectile.knockBack,
                        projectile.owner
                    );
                }
            }
            base.OnSpawn(projectile, source);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsCosmosGelInfused && target.active && !target.friendly)
            {
                // 施加 GodSlayerInferno 和 CosmosGelEDebuff
                target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300); // 5 秒
                target.AddBuff(ModContent.BuffType<CosmosGelEDebuff>(), 90); // 1.5 秒

                // 调整伤害为原来的 70%（穿透衰减）
                //projectile.damage = (int)(projectile.damage * 0.7f);
            }
        }
    }
}
