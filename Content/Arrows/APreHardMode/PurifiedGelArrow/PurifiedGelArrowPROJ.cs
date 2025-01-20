using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Graphics.Primitives;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using FKsCRE.CREConfigs;
using CalamityMod.Projectiles.Summon;

namespace FKsCRE.Content.Arrows.APreHardMode.PurifiedGelArrow
{
    public class PurifiedGelArrowPROJ : ModProjectile, ILocalizedModType
    {
        public override string Texture => "FKsCRE/Content/Arrows/APreHardMode/PurifiedGelArrow/PurifiedGelArrow";

        public new string LocalizationCategory => "Projectile.APreHardMode";

        public static int MaxUpdate = 1; // 弹幕每次更新的最大次数
        private int Lifetime = 110; // 弹幕的生命周期为110

        private static Color ShaderColorOne = Color.DarkGreen; // 着色器颜色1，设置为深绿色
        private static Color ShaderColorTwo = Color.Black; // 着色器颜色2，设置为黑色
        private static Color ShaderEndColor = Color.ForestGreen; // 着色器结束颜色，设置为森林绿色（另一种深绿色）

        private Vector2 altSpawn; // 定义备用生成位置向量

        public override void SetStaticDefaults()
        {
            // 保留原有的拖尾绘制设置
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }


        public override void SetDefaults()
        {
            Projectile.width = 11; // 弹幕宽度
            Projectile.height = 24; // 弹幕高度
            Projectile.friendly = true; // 对敌人有效
            Projectile.DamageType = DamageClass.Ranged; // 远程伤害类型
            Projectile.penetrate = 1; // 穿透力为1，击中一个敌人就消失
            Projectile.timeLeft = Lifetime; // 弹幕存在时间
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;
            Projectile.ignoreWater = true;
            Projectile.arrow = true;
            Projectile.extraUpdates = MaxUpdate; // 使用 UelibloomArrowLight 的更新次数
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;

            // 添加粉红色与白色渐变的光源效果
            Lighting.AddLight(Projectile.Center, Color.Pink.ToVector3() * 0.49f);

            // 检查场上是否存在 WitherBlossom 弹幕
            bool hasWitherBlossom = Main.projectile.Any(p => p.active && p.type == ModContent.ProjectileType<WitherBlossom>());

            if (hasWitherBlossom)
            {
                // 前30帧不追踪，之后开始追踪敌人
                if (Projectile.ai[1] > 30)
                {
                    NPC target = Projectile.Center.ClosestNPCAt(1800f); // 查找1800范围内最近的敌人
                    if (target != null)
                    {
                        Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * 12f, 0.08f); // 追踪速度为12
                    }
                }
                else
                {
                    Projectile.ai[1]++;
                }
            }
            else
            {
                // 没有 WitherBlossom 弹幕时直接直线飞行
                Projectile.ai[1]++;
            }
        }

        private float PrimitiveWidthFunction(float completionRatio)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                float arrowheadCutoff = 0.36f;
                float width = 24f;
                float minHeadWidth = 0.03f;
                float maxHeadWidth = width;
                if (completionRatio <= arrowheadCutoff)
                    width = MathHelper.Lerp(minHeadWidth, maxHeadWidth, Utils.GetLerpValue(0f, arrowheadCutoff, completionRatio, true));
                return width;
            }

            // 如果没有启用特效，返回默认宽度
            return 24f;
        }


        private Color PrimitiveColorFunction(float completionRatio)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                float endFadeRatio = 0.41f;
                float completionRatioFactor = 2.7f;
                float globalTimeFactor = 5.3f;
                float endFadeFactor = 3.2f;
                float endFadeTerm = Utils.GetLerpValue(0f, endFadeRatio * 0.5f, completionRatio, true) * endFadeFactor;
                float cosArgument = completionRatio * completionRatioFactor - Main.GlobalTimeWrappedHourly * globalTimeFactor + endFadeTerm;
                float startingInterpolant = (float)Math.Cos(cosArgument) * 0.5f + 0.5f;

                float colorLerpFactor = 0.6f;
                Color startingColor = Color.Lerp(ShaderColorOne, ShaderColorTwo, startingInterpolant * colorLerpFactor);
                return Color.Lerp(startingColor, ShaderEndColor, MathHelper.SmoothStep(0f, 1f, Utils.GetLerpValue(0f, endFadeRatio, completionRatio, true)));
            }

            // 如果没有启用特效，返回默认颜色
            return Color.White;
        }


        public override bool PreDraw(ref Color lightColor)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            { 
                CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Type], lightColor, 1);

                // 渲染带有粉红色渐变效果的光学尾迹
                GameShaders.Misc["CalamityMod:TrailStreak"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/FabstaffStreak"));
                Vector2 overallOffset = Projectile.Size * 0.5f;
                overallOffset += Projectile.velocity * 1.4f;
                int numPoints = 46;
                PrimitiveRenderer.RenderTrail(Projectile.oldPos, new(PrimitiveWidthFunction, PrimitiveColorFunction, (_) => overallOffset, shader: GameShaders.Misc["CalamityMod:TrailStreak"]), numPoints);
                return false;
            }
            return true;

        }

        public override void OnKill(int timeLeft)
        {
            // 检查是否启用了特效
            if (ModContent.GetInstance<CREsConfigs>().EnableSpecialEffects)
            {
                // 粉红色粒子 - 向上和向下发射
                int numParticles = 7;
                for (int i = 0; i < numParticles; i++)
                {
                    float speedFactor = 3f + i * 0.2f; // 控制速度，使粒子平摊成链状

                    // 向上发射
                    Vector2 velocityUp = new Vector2(0, -speedFactor);
                    Dust pinkDustUp = Dust.NewDustPerfect(Projectile.Center, DustID.PinkTorch, velocityUp, 0, Color.Pink, 1.5f);
                    pinkDustUp.noGravity = true;

                    // 向下发射
                    Vector2 velocityDown = new Vector2(0, speedFactor);
                    Dust pinkDustDown = Dust.NewDustPerfect(Projectile.Center, DustID.PinkTorch, velocityDown, 0, Color.Pink, 1.5f);
                    pinkDustDown.noGravity = true;
                }

                // 白色粒子 - 向左和向右发射
                for (int i = 0; i < numParticles; i++)
                {
                    float speedFactor = 1f + i * 0.2f; // 控制速度，使粒子平摊成链状

                    // 向左发射
                    Vector2 velocityLeft = new Vector2(-speedFactor, 0);
                    Dust whiteDustLeft = Dust.NewDustPerfect(Projectile.Center, DustID.WhiteTorch, velocityLeft, 0, Color.White, 1.5f);
                    whiteDustLeft.noGravity = true;

                    // 向右发射
                    Vector2 velocityRight = new Vector2(speedFactor, 0);
                    Dust whiteDustRight = Dust.NewDustPerfect(Projectile.Center, DustID.WhiteTorch, velocityRight, 0, Color.White, 1.5f);
                    whiteDustRight.noGravity = true;
                }

            }
             
        }



    }
}
