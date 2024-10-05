using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Ranged;
using Terraria.DataStructures;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Particles;
using FKsCRE.CREConfigs;

namespace FKsCRE.Content.Arrows.DPreDog.DivineGeodeArrow
{
    public class DivineGeodeArrowPROJ : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
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
            // 设置弹幕的基础属性
            Projectile.width = 11; // 弹幕宽度
            Projectile.height = 24; // 弹幕高度
            Projectile.friendly = true; // 对敌人有效
            Projectile.DamageType = DamageClass.Ranged; // 远程伤害类型
            Projectile.penetrate = 1; // 穿透力为1，击中一个敌人就消失
            Projectile.timeLeft = 600; // 弹幕存在时间为600帧
            Projectile.usesLocalNPCImmunity = true; // 弹幕使用本地无敌帧
            Projectile.localNPCHitCooldown = 14; // 无敌帧冷却时间为14帧
            Projectile.ignoreWater = true; // 弹幕不受水影响
            Projectile.arrow = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            // 调整弹幕的旋转，使其在飞行时保持水平
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;

            // Lighting - 添加土黄色/卡其色光源，光照强度为 0.55
            Lighting.AddLight(Projectile.Center, new Color(189, 183, 107).ToVector3() * 0.55f);

            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 刚生成时释放几个卡其色的小圆圈往外扩散
                if (Projectile.ai[0] == 0f)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Particle pulse = new DirectionalPulseRing(Projectile.Center, Projectile.velocity * 0.75f, Color.Khaki, new Vector2(1f, 2.5f), Projectile.rotation, 0.2f, 0.03f, 20);
                        GeneralParticleHandler.SpawnParticle(pulse);
                    }

                    for (int i = 0; i <= 5; i++)
                    {
                        Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.GoldFlame, Projectile.velocity);
                        dust.scale = Main.rand.NextFloat(1.6f, 2.5f);
                        dust.velocity = Projectile.velocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.3f, 1.6f);
                        dust.noGravity = true;
                    }
                }

                // 为箭矢本体后面添加卡其色光束特效
                if (Projectile.numUpdates % 3 == 0)
                {
                    Color outerSparkColor = new Color(189, 183, 107); // 卡其色
                    float scaleBoost = MathHelper.Clamp(Projectile.ai[0] * 0.005f, 0f, 2f);
                    float outerSparkScale = 1.2f + scaleBoost;
                    SparkParticle spark = new SparkParticle(Projectile.Center, Projectile.velocity, false, 7, outerSparkScale, outerSparkColor);
                    GeneralParticleHandler.SpawnParticle(spark);
                }
            }


            Projectile.ai[0]++;
        }

        public override void OnKill(int timeLeft)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 在结束时释放几个卡其色的小圆圈特效
                for (int i = 0; i < 3; i++)
                {
                    Particle pulse = new DirectionalPulseRing(Projectile.Center, Projectile.velocity * 0.75f, Color.Khaki, new Vector2(1f, 2.5f), Projectile.rotation, 0.2f, 0.03f, 20);
                    GeneralParticleHandler.SpawnParticle(pulse);
                }

                // 在结束时释放浅黄色的小型特效粒子
                for (int i = 0; i <= 5; i++)
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.GoldFlame, Projectile.velocity);
                    dust.scale = Main.rand.NextFloat(1.35f, 2.1f);
                    dust.velocity = Projectile.velocity.RotatedByRandom(0.06f) * Main.rand.NextFloat(0.8f, 3.1f);
                    dust.color = Color.LightYellow;
                    dust.noGravity = true;
                }
            }
              
        }



    }
}