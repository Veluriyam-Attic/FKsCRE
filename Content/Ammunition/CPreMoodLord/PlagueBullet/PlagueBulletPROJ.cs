using CalamityMod.Particles;
using CalamityMod;
using FKsCRE.Content.Ammunition.CPreMoodLord.PerennialBullet;
using FKsCRE.CREConfigs;
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
using CalamityMod.Buffs.DamageOverTime;

namespace FKsCRE.Content.Ammunition.CPreMoodLord.PlagueBullet
{
    internal class PlagueBulletPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectile.CPreMoodLord";
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
                CalamityUtils.DrawAfterimagesFromEdge(Projectile, 0, Color.White);
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
            Projectile.timeLeft = 300;
            Projectile.MaxUpdates = 5;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
        }

        public override void AI()
        {
            // 由于我们是水平贴图，因此什么也不需要转动
            Projectile.rotation = Projectile.velocity.ToRotation();

            // 添加光效
            Lighting.AddLight(Projectile.Center, Color.Lerp(Color.Blue, Color.AliceBlue, 0.5f).ToVector3() * 0.49f);

            // 子弹在出现之后很短一段时间会变得可见
            if (Projectile.timeLeft == 296)
                Projectile.alpha = 0;

            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 添加飞行期间的粒子特效
                if (Main.rand.NextBool(3)) // 随机1/3概率生成
                {
                    Dust dust = Dust.NewDustPerfect(
                        Projectile.Center,
                        Main.rand.NextBool() ? 89 : 6, // 改为PlagueTaintedDrone的粒子类型
                        -Projectile.velocity.RotatedByRandom(0.1f) * Main.rand.NextFloat(0.01f, 0.3f) // 随机化速度
                    );
                    dust.noGravity = true; // 粒子无重力
                    dust.scale = Main.rand.NextFloat(0.5f, 0.8f); // 调整随机大小范围
                }
            }
        }

        public override void OnSpawn(IEntitySource source)
        {

        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.HasBuff(ModContent.BuffType<Plague>())) // 检查是否有瘟疫debuff
            {
                modifiers.FinalDamage *= 1.25f; // 增加伤害
            }
            else
            {
                modifiers.FinalDamage *= 0.75f; // 减少伤害
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                int particleCount = target.HasBuff(ModContent.BuffType<Plague>()) ? 10 : 5; // 瘟疫时特效更华丽
                float velocityMultiplier = target.HasBuff(ModContent.BuffType<Plague>()) ? 3f : 1.5f;

                for (int i = 0; i < particleCount; i++)
                {
                    for (int j = 0; j < 2; j++) // 每个顶点生成2个粒子
                    {
                        Vector2 velocity = new Vector2(
                            (float)Math.Cos(MathHelper.TwoPi / 3 * i),
                            (float)Math.Sin(MathHelper.TwoPi / 3 * i)
                        ) * velocityMultiplier + Main.rand.NextVector2Circular(2f, 2f);

                        Dust plague = Dust.NewDustDirect(
                            Projectile.Center,
                            Projectile.width / 2,
                            Projectile.height / 2,
                            DustID.TerraBlade
                        );
                        plague.velocity = velocity;
                        plague.color = Color.Olive;
                        plague.noGravity = true;
                        plague.scale = Main.rand.NextFloat(1f, 1.5f);
                    }
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
               
            }
        }



    }
}
