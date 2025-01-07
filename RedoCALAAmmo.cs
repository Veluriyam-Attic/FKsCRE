using CalamityMod.Projectiles.DraedonsArsenal;
using CalamityMod.Projectiles.Ranged;
using FKsCRE.Content.Ammunition.BPrePlantera.CryonicBullet;
using FKsCRE.Content.Ammunition.CPreMoodLord.PerennialBullet;
using FKsCRE.Content.Ammunition.CPreMoodLord.ScoriaBullet;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace FKsCRE
{
    internal class RedoCALAAmmo : GlobalProjectile // 继承全局弹幕类
    {
        public override bool InstancePerEntity => true; // 保证每个弹幕实例独立

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            // 修改 MortarRoundProj（迫击炮） 和 RubberMortarRoundProj（橡胶迫击炮）
            if (projectile.type == ModContent.ProjectileType<MortarRoundProj>() ||
                projectile.type == ModContent.ProjectileType<RubberMortarRoundProj>())
            {
                modifiers.FinalDamage = new Terraria.ModLoader.StatModifier(0, 0, 1, 1); // 只造成一点伤害
            }

            // 修改 AnomalysNanogunPlasmaBeam（纳米枪激光）
            if (projectile.type == ModContent.ProjectileType<AnomalysNanogunPlasmaBeam>())
            {
                projectile.damage *= 2; // 每击中一次敌人伤害翻倍
            }

            // 强化 PiercingBullet（暴政之终狙击弹）
            if (projectile.type == ModContent.ProjectileType<PiercingBullet>())
            {
                modifiers.DefenseEffectiveness *= 0f; // 无视防御
                modifiers.FinalDamage /= 1f - target.Calamity().DR; // 无视伤害减免
            }
        }


        // 为 HyperiusBulletProj（海伯里斯弹） 添加命中之后额外召唤弹幕的逻辑
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.type == ModContent.ProjectileType<HyperiusBulletProj>())
            {
                Vector2 direction = projectile.velocity.SafeNormalize(Vector2.Zero);
                Vector2 spawnCenter = target.Center + direction * 30 * 16; // 前方 10 格位置

                for (int i = 0; i < 3; i++) // 生成 3 个弹幕
                {
                    Vector2 randomOffset = Main.rand.NextVector2Circular(15 * 16, 15 * 16); // 圆形范围
                    Vector2 spawnPosition = spawnCenter + randomOffset;

                    Vector2 targetDirection = (projectile.Center - spawnPosition).SafeNormalize(Vector2.Zero) * projectile.velocity.Length() * 3;

                    Projectile.NewProjectile(
                        projectile.GetSource_FromThis(),
                        spawnPosition,
                        targetDirection, // 朝向自己本体
                        Main.rand.Next(new int[]
                        {
                            ModContent.ProjectileType<CryonicBulletPROJ>(),
                            ModContent.ProjectileType<ScoriaBulletPROJ>(),
                            ModContent.ProjectileType<PerennialBulletPROJ>()
                        }),
                        (int)(projectile.damage * 0.33),
                        projectile.knockBack,
                        projectile.owner
                    );
                }

                // 粒子特效以生成弹幕的位置为中心
                for (int j = 0; j < 15; j++) // 每个方向生成十字型粒子
                {
                    Vector2 particleVelocity = direction.RotatedBy(MathHelper.PiOver2 * j);
                    int dustType = projectile.type switch
                    {
                        var t when t == ModContent.ProjectileType<CryonicBulletPROJ>() => 185,
                        var t when t == ModContent.ProjectileType<ScoriaBulletPROJ>() => DustID.Lava,
                        var t when t == ModContent.ProjectileType<PerennialBulletPROJ>() => DustID.TerraBlade,
                        _ => DustID.Torch
                    };

                    Vector2 randomOffset = Main.rand.NextVector2Circular(1 * 16, 1 * 16); // 圆形范围
                    Vector2 spawnPosition = spawnCenter + randomOffset;

                    Dust.NewDustPerfect(spawnPosition, dustType, particleVelocity * 2, 0, default, 1.2f).noGravity = true;
                }
            }
        }


    }
}
