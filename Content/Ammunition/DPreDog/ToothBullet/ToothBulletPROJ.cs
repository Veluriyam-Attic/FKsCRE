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
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 添加飞行粒子特效
                if (Main.rand.NextBool(2)) // 1/3 概率生成粒子
                {
                    Dust dust = Dust.NewDustPerfect(
                        Projectile.Center,
                        Main.rand.NextBool() ? DustID.WaterCandle : DustID.DynastyShingle_Blue,
                        -Projectile.velocity.RotatedByRandom(0.1f) * Main.rand.NextFloat(0.1f, 0.3f)
                    );
                    dust.noGravity = true; // 无重力
                    dust.scale = Main.rand.NextFloat(0.7f, 1.1f); // 随机大小
                }
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
                float rectWidth = 3f; // 矩形宽度
                float rectHeight = 1f; // 矩形高度

                for (int i = 0; i < numParticles; i++)
                {
                    // 在矩形范围内生成随机位置
                    Vector2 offset = new Vector2(
                        Main.rand.NextFloat(-rectWidth / 2f, rectWidth / 2f),
                        Main.rand.NextFloat(-rectHeight / 2f, rectHeight / 2f)
                    );

                    // 生成速度，偏向正前方
                    Vector2 velocity = (Vector2.UnitX + new Vector2(
                        Main.rand.NextFloat(-0.2f, 0.2f),
                        Main.rand.NextFloat(-0.2f, 0.2f)
                    )) * Main.rand.NextFloat(1f, 3f);

                    Dust dust = Dust.NewDustPerfect(
                        Projectile.Center + offset, // 粒子生成位置
                        DustID.BlueTorch, // 原版粒子类型
                        velocity, // 速度
                        0,
                        Color.Navy, // 粒子颜色
                        Main.rand.NextFloat(1.2f, 1.8f) // 粒子大小
                    );
                    dust.noGravity = true; // 粒子无重力
                }
            }
        }
    }
}
