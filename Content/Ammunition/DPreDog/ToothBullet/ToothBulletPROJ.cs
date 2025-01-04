using CalamityMod;
using FKsCRE.CREConfigs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Buffs.DamageOverTime;

namespace FKsCRE.Content.Ammunition.DPreDog.ToothBullet
{
    internal class ToothBulletPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectile.DPreDog";
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
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects && Projectile.timeLeft % 3 == 0) // 每三帧生成一次泡泡
            {
                float offset = (float)Math.Sin(Projectile.localAI[0] * 0.1f) * 1.5f; // 单螺旋偏移量
                Vector2 bubblePos = Projectile.Center + Projectile.velocity.RotatedBy(MathHelper.PiOver2) * offset;
                Gore bubble = Gore.NewGorePerfect(
                    Projectile.GetSource_FromAI(),
                    bubblePos + Main.rand.NextVector2Circular(3f, 3f), // 随机偏移
                    Projectile.velocity * 0.2f + Main.rand.NextVector2Circular(1f, 1f),
                    Main.rand.NextBool(3) ? 412 : 411 // 随机选择泡泡类型
                );
                bubble.timeLeft = 8 + Main.rand.Next(6);
                bubble.scale = Main.rand.NextFloat(0.6f, 1f);
            }

            Projectile.localAI[0]++; // 更新局部 AI
        }

        public override void OnSpawn(IEntitySource source)
        {

        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            float defenseBonus = Math.Min(target.defense * 0.0075f, 0.5f); // 防御力加成，每点防御增加0.75%，最大50%
            modifiers.SourceDamage *= 1 + defenseBonus; // 增加伤害            
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 300); // 深渊水压
        }


        public override void OnKill(int timeLeft)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                int numParticles = 20; // 粒子数量
                float longAxis = 1.5f; // 椭圆长轴比例
                float shortAxis = 0.8f; // 椭圆短轴比例

                for (int i = 0; i < numParticles; i++)
                {
                    float angle = MathHelper.TwoPi / numParticles * i;
                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle) * longAxis,
                        (float)Math.Sin(angle) * shortAxis
                    ) * Main.rand.NextFloat(1f, 3f); // 椭圆速度偏移

                    Dust dust = Dust.NewDustPerfect(
                        Projectile.Center,
                        DustID.GreenTorch, // 原版粒子类型
                        velocity,
                        0,
                        Color.Lime, // 粒子颜色
                        Main.rand.NextFloat(1.2f, 1.8f) // 粒子大小
                    );
                    dust.noGravity = true; // 粒子无重力
                }
            }
        }
    }
}
