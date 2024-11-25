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
using FKsCRE.Content.Arrows.CPreMoodLord.PerennialArrow;
using FKsCRE.Content.Arrows.CPreMoodLord.ScoriaArrow;

namespace FKsCRE.Content.Arrows.CPreMoodLord.LifeAlloyArrow
{
    public class LifeAlloyArrowPROJ : ModProjectile, ILocalizedModType
    {
        public override string Texture => "FKsCRE/Content/Arrows/CPreMoodLord/LifeAlloyArrow/LifeAlloyArrow";

        public new string LocalizationCategory => "Projectile.CPreMoodLord";
        private Color currentColor = Color.Black; // 初始化为黑色
        private float bendAngle = 0f;
        private bool growing = false;
        private float variance;

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
            Projectile.extraUpdates = 3;
            Projectile.velocity *= 0.4f;
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
                // 初始化弯折控制变量
                if (variance == 0)
                {
                    variance = Main.rand.NextFloat(0.3f, 1.7f); // 随机控制弯折的幅度
                }

                // 控制弯折角度的变化
                if (bendAngle <= -0.5f)
                {
                    growing = true;
                }
                if (bendAngle >= 0.5f)
                {
                    growing = false;
                }
                bendAngle += (growing ? 0.07f * variance : -0.07f * variance);

                // 计算粒子生成的偏移位置（保持箭矢直线飞行）
                Vector2 offset = Projectile.velocity.RotatedBy(bendAngle) * 5f; // 控制弯折幅度
                Vector2 particlePosition = Projectile.Center + offset;

                // 检查是否启用了特效
                if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
                {
                    // 初始化随机颜色（仅设置一次）
                    if (currentColorLeft == Color.Black && currentColorRight == Color.Black)
                    {
                        currentColorLeft = Main.rand.NextBool() ? Color.Red : Color.Green;
                        currentColorRight = Main.rand.NextBool() ? Color.Blue : Color.Yellow;
                    }

                    // 添加天蓝色光源
                    Lighting.AddLight(Projectile.Center, Color.LightSkyBlue.ToVector3() * 0.49f);

                    // 获取偏移位置的左右两侧位置
                    Vector2 leftPosition = particlePosition - Projectile.velocity.SafeNormalize(Vector2.Zero) * Projectile.width / 2;
                    Vector2 rightPosition = particlePosition + Projectile.velocity.SafeNormalize(Vector2.Zero) * Projectile.width / 2;

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


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 circleCenter = player.Center; // 圆心为玩家位置
            float circleRadius = 5 * 16f; // 半径为5个方块，转换为像素

            // 随机选择一个角度
            float randomAngle = Main.rand.NextFloat(0, MathHelper.TwoPi);
            Vector2 spawnPosition = circleCenter + circleRadius * new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle));

            // 瞄准主弹幕击中敌人的位置
            Vector2 directionToTarget = Vector2.Normalize(target.Center - spawnPosition) * Projectile.velocity.Length(); // 方向向量，长度等于主弹幕的速度

            // 随机选择一种弹幕类型
            int selectedProjectileType;
            Color lineColor;
            Color smokeColor;

            switch (Main.rand.Next(3))
            {
                case 0: // PerennialArrowPROJ
                    selectedProjectileType = ModContent.ProjectileType<PerennialArrowPROJ>();
                    lineColor = Color.Green;
                    smokeColor = Color.Green;
                    break;
                case 1: // ScoriaArrowPROJ
                    selectedProjectileType = ModContent.ProjectileType<ScoriaArrowPROJ>();
                    lineColor = Color.OrangeRed;
                    smokeColor = Color.OrangeRed;
                    break;
                default: // VeriumBoltProj
                    selectedProjectileType = ModContent.ProjectileType<VeriumBoltProj>();
                    lineColor = Color.LightBlue;
                    smokeColor = Color.LightBlue;
                    break;
            }

            // 发射弹幕，让新生成的弹幕速度是本体速度的x倍
            Vector2 adjustedVelocity = directionToTarget * 50f;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPosition, adjustedVelocity, selectedProjectileType, Projectile.damage, Projectile.knockBack, Projectile.owner);

            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 画粒子特效连接线
                for (float i = 0; i <= 1; i += 0.05f) // 线条由粒子连接组成
                {
                    Vector2 positionOnLine = Vector2.Lerp(Projectile.Center, spawnPosition, i);
                    Particle lineParticle = new GlowOrbParticle(positionOnLine, Vector2.Zero, false, 3, 0.4f, lineColor, true, true);
                    GeneralParticleHandler.SpawnParticle(lineParticle);
                }

                // 在弹幕发射位置释放轻型烟雾
                for (int i = 0; i < 50; i++)
                {
                    Vector2 dustVelocity = Main.rand.NextVector2Circular(2f, 2f); // 随机速度
                    Particle smoke = new HeavySmokeParticle(spawnPosition, dustVelocity * Main.rand.NextFloat(1f, 2.6f), smokeColor, 18, Main.rand.NextFloat(0.9f, 1.6f), 0.35f, Main.rand.NextFloat(-1, 1), true);
                    GeneralParticleHandler.SpawnParticle(smoke);
                }
            }
              
        }





    }
}