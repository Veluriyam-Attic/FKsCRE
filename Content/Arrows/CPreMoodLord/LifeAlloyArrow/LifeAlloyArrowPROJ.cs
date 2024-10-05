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
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Ranged;
using Terraria.DataStructures;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalamityMod.Particles;
using FKsCRE.CREConfigs;

namespace FKsCRE.Content.Arrows.CPreMoodLord.LifeAlloyArrow
{
    public class LifeAlloyArrowPROJ : ModProjectile
    {
        private Color currentColor = Color.Black; // 初始化为黑色
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
            Projectile.extraUpdates = 4;
        }

        private Color currentColorLeft = Color.Black;
        private Color currentColorRight = Color.Black;

        public override void AI()
        {
            // 调整弹幕的旋转，使其在飞行时保持水平
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;

            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {

                // 初始化随机颜色（仅设置一次）
                if (currentColorLeft == Color.Black && currentColorRight == Color.Black)
                {
                    // 随机为左端和右端选择颜色
                    currentColorLeft = Main.rand.NextBool() ? Color.Red : Color.Green;
                    currentColorRight = Main.rand.NextBool() ? Color.Blue : Color.Yellow;
                }

                // 添加天蓝色光源
                Lighting.AddLight(Projectile.Center, Color.LightSkyBlue.ToVector3() * 0.49f);

                // 获取弹幕的左端和右端位置
                Vector2 leftPosition = Projectile.Center - Projectile.velocity.SafeNormalize(Vector2.Zero) * Projectile.width / 2;
                Vector2 rightPosition = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * Projectile.width / 2;

                // 在左端生成带有随机颜色的粒子
                GlowOrbParticle leftOrb = new GlowOrbParticle(
                    leftPosition, Vector2.Zero, false, 5, 0.55f, currentColorLeft, true, true
                );
                GeneralParticleHandler.SpawnParticle(leftOrb);

                // 在右端生成带有随机颜色的粒子
                GlowOrbParticle rightOrb = new GlowOrbParticle(
                    rightPosition, Vector2.Zero, false, 5, 0.55f, currentColorRight, true, true
                );
                GeneralParticleHandler.SpawnParticle(rightOrb);

                // 生成视觉上的光点效果
                PointParticle leftSpark = new PointParticle(
                    leftPosition, Projectile.velocity, false, 2, 0.6f, currentColorLeft
                );
                GeneralParticleHandler.SpawnParticle(leftSpark);

                PointParticle rightSpark = new PointParticle(
                    rightPosition, Projectile.velocity, false, 2, 0.6f, currentColorRight
                );
                GeneralParticleHandler.SpawnParticle(rightSpark);

            }
          
        }

        //public override void OnKill(int timeLeft)
        //{
        //    for (int b = 0; b < 4; b++) 
        //    {
        //        Projectile.NewProjectile(
        //            Projectile.GetSource_FromThis(),
        //            Projectile.Center,
        //            Projectile.velocity.RotatedByRandom(0.5f) * 0.2f,
        //            ModContent.ProjectileType<HyperiusSplit>(),
        //            0, // 没有伤害
        //            0f,
        //            Projectile.owner
        //        );
        //    }
        //}

        public override void OnKill(int timeLeft)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                for (int b = 0; b < 3; b++)
                {
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        Projectile.velocity.RotatedByRandom(0.5f) * 0.2f,
                        ModContent.ProjectileType<HyperiusSplit>(),
                        0, // 没有伤害
                        0f,
                        Projectile.owner,
                        ai0: 0f,
                        ai1: 0f,
                        ai2: Main.rand.Next(0, 5) // 随机生成 0 到 4 的数值，决定颜色
                    );
                }

            }
           
        }







    }
}